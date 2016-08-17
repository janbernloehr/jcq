// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseConnector.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Jcq.Core;
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol.Internal
{
    public class BaseConnector : ContextService, IIcqDataTranferService
    {
        private readonly Dictionary<string, List<Delegate>> _snacHandlers;
        private int _flapSequenceNumber;
        private BufferBlock<FlapDataPair> _sendBuffer;

        public BaseConnector(IContext context)
            : base(context)
        {
            _snacHandlers = new Dictionary<string, List<Delegate>>();

            //_sendBuffer = new BufferBlock<FlapDataPair>();
        }

        public bool IsConnected
        {
            get { return TcpContext != null && TcpContext.ConnectionState == TcpConnectionState.Connected; }
        }

        public ITcpContext TcpContext { get; private set; }

        public void RegisterSnacHandler<T>(int serviceId, int subtypeId, Action<T> handler) where T : Snac
        {
            List<Delegate> handlerList;

            string key = string.Format("{0:X2},{1:X2}", serviceId, subtypeId);

            if (!_snacHandlers.TryGetValue(key, out handlerList))
            {
                handlerList = new List<Delegate>();

                _snacHandlers.Add(key, handlerList);
            }

            handlerList.Add(handler);
        }

        protected event EventHandler InternalConnected;
        protected event EventHandler<DisconnectEventArgs> InternalDisconnected;

        protected virtual void InnerConnect()
        {
            string hostName = ConfigurationManager.AppSettings["OscarServer"];
            int hostPort = int.Parse(ConfigurationManager.AppSettings["OscarPort"]);

            IPHostEntry hentry = Dns.GetHostEntry(hostName);
            var endpoint = new IPEndPoint(hentry.AddressList[0], hostPort);

            InnerConnect(endpoint);
        }

        protected virtual void InnerConnect(IPEndPoint endpoint)
        {
            InnerDisconnect();

            TcpContext = new TcpContextNet45();
            TcpContext.Connect(endpoint);

            //TcpContext.DataReceived += OnTcpContextDataReceived;
            TcpContext.Disconnected += OnTcpContextDisconnected;

            OnInternalConnected(EventArgs.Empty);

            _sendBuffer = new BufferBlock<FlapDataPair>();

            Task.Run(() => AnalyzeData(TcpContext));
            Task.Run(() => SendData(TcpContext, _sendBuffer));

            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, "{0} connected to {1}", TcpContext.Id, endpoint);
        }

        protected virtual void InnerDisconnect()
        {
            if (TcpContext != null)
            {
                TcpContext.Disconnected -= OnTcpContextDisconnected;
                TcpContext.Disconnect();
            }

            if (_sendBuffer != null)
            {
                _sendBuffer.Complete();
                _sendBuffer = null;
            }
        }

        protected virtual void OnInternalConnected(EventArgs e)
        {
            if (InternalConnected != null)
            {
                InternalConnected(this, e);
            }
        }

        protected virtual void OnInternalDisconnected(DisconnectEventArgs e)
        {
            if (InternalDisconnected != null)
            {
                InternalDisconnected(this, e);
            }
        }

        protected IPEndPoint ConvertServerAddressToEndPoint(string address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var serverAddressParts = address.Split(':');

            IPAddress serverIp = IPAddress.Parse(serverAddressParts[0]);
            int serverPort = int.Parse(serverAddressParts[1]);

            return new IPEndPoint(serverIp, serverPort);
        }

        private void OnTcpContextDisconnected(object sender, DisconnectEventArgs e)
        {
            try
            {
                if (_sendBuffer != null)
                {
                    _sendBuffer.Complete();
                    _sendBuffer = null;
                }
                TcpContext.Disconnected -= OnTcpContextDisconnected;
                OnInternalDisconnected(e);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void CallSnacHandlers(Snac snac)
        {
            string key = Snac.GetKey(snac);

            try
            {
                Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, "Processed: {0}", key);
                Debug.WriteLine(string.Format("<< Snac {0}", key), "BaseConnector");

                List<Delegate> handlers;

                if (!_snacHandlers.TryGetValue(key, out handlers)) return;

                foreach (Delegate x in handlers)
                {
                    x.DynamicInvoke(snac);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #region Data Receiving

        public event EventHandler<FlapTransportEventArgs> FlapReceived;


        private readonly List<byte> _analyzeBuffer = new List<byte>();

        private async Task AnalyzeData(ITcpContext context)
        {
            // buffer bytes for analysis. if more bytes are needed to decode
            // the received data we wait for another cycle.

            string id = Guid.NewGuid().ToString();

            try
            {
                while (context.ConnectionState == TcpConnectionState.Connected)
                {
                    // This call will return on its own Thread Pool Thread to
                    // process the data.
                    var data = await context.ReceivedDataBuffer.ReceiveAsync();

                    // add data to the buffer.
                    _analyzeBuffer.AddRange(data);

                    int index = 0;
                    int iloop = 0;

                    while (index + 6 <= _analyzeBuffer.Count)
                    {
                        // decode the flap header
                        FlapDescriptor desc = FlapDescriptor.GetDescriptor(index, _analyzeBuffer);

                        if (_analyzeBuffer.Count < index + desc.TotalSize)
                        {
                            // there is more data needed to deserialize the flap. wait for another cycle...
                            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                                "{3}@{4}/{0}: caching {1}, {2} required.", iloop, _analyzeBuffer.Count - index,
                                desc.TotalSize, id, context.Id);

                            break;
                        }

                        Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                            "{2}@{3}/{0}: queuing {1} bytes for analysis", iloop, desc.TotalSize, id, context.Id);

                        ProcessFlap(_analyzeBuffer.GetRange(index, desc.TotalSize));

                        index += desc.TotalSize;
                        iloop += 1;
                    }

                    if (index > 0)
                        _analyzeBuffer.RemoveRange(0, index);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void ProcessFlap(List<byte> flapData)
        {
            try
            {
                var flap = new Flap();

                flap.Deserialize(flapData);

                if (FlapReceived != null)
                {
                    FlapReceived(this, new FlapTransportEventArgs(flap));
                }

                WriteInFlapLog(flapData, flap);

                if (flap.Channel != FlapChannel.SnacData)
                {
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        foreach (Snac x in flap.DataItems.Cast<Snac>())
                        {
                            CallSnacHandlers(x);
                        }
                    }
                    catch (Exception taskEx)
                    {
                        Kernel.Exceptions.PublishException(taskEx);
                    }
                });
            }
            catch (Exception ex)
            {
                // if one flap deserialization fails,
                // the analyze loop should still continue.
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void WriteFlapLog(string modifier, List<byte> flapData, Flap flap)
        {
            try
            {
                string path = string.Format("jcq/dumps/{0}/{3}/{1}_{2}.json", TcpContext.Id, flap.DatagramSequenceNumber,
                    flap.Channel == FlapChannel.SnacData & flap.DataItems.Any()
                        ? flap.DataItems.First().GetType().Name
                        : Enum.GetName(typeof(FlapChannel), flap.Channel),
                    modifier);

                var file =
                    new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path));

                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }

                using (var sw = new StreamWriter(file.FullName))
                {
                    sw.Write(BitConverter.ToString(flapData.ToArray()));
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void WriteInFlapLog(List<byte> flapData, Flap flap)
        {
            WriteFlapLog("in", flapData, flap);
        }

        private void WriteOutFlapLog(List<byte> flapData, Flap flap)
        {
            WriteFlapLog("out", flapData, flap);
        }

        //private void OnTcpContextDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        _analyzeBuffer.Post(e.Data);
        //        //lock (_analyzeLock)
        //        //{
        //        //    if (_analyzeBuffer.Count > 0)
        //        //    {
        //        //        Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
        //        //            "data received. already cached: {0}, new: {1}, ticket: {2}", _analyzeBuffer.Count,
        //        //            data.Count, e.Ticket);
        //        //        data.InsertRange(0, _analyzeBuffer);
        //        //        _analyzeBuffer.Clear();
        //        //    }

        //        //    var index = 0;
        //        //    var iloop = 0;

        //        //    while (index + 6 <= data.Count)
        //        //    {
        //        //        var desc = FlapDescriptor.GetDescriptor(index, data);

        //        //        if (data.Count < index + desc.TotalSize)
        //        //        {
        //        //            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
        //        //                "{0}: caching {1}, {2} more required.", iloop, data.Count - index, desc.TotalSize);
        //        //            _analyzeBuffer.AddRange(data.GetRange(index, data.Count - index));
        //        //        }
        //        //        else
        //        //        {
        //        //            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
        //        //                "{0}: queuing {1} bytes for analyzation", iloop, data.Count - index, desc.TotalSize);
        //        //            ThreadPool.QueueUserWorkItem(AsyncPreAnalyze, data.GetRange(index, desc.TotalSize));
        //        //        }

        //        //        index += desc.TotalSize;
        //        //        iloop += 1;
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Kernel.Exceptions.PublishException(ex);
        //    }
        //}

        //private void AsyncPreAnalyze(object state)
        //{
        //    try
        //    {
        //        var data = (List<byte>)state;

        //        var f = new Flap();
        //        f.Deserialize(data);
        //        if (FlapReceived != null)
        //        {
        //            FlapReceived(this, new FlapTransportEventArgs(f));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Kernel.Exceptions.PublishException(ex);
        //    }
        //}

        //private void OnFlapReceived(object sender, FlapTransportEventArgs e)
        //{
        //    var flap = e.Flap;

        //    try
        //    {
        //        if (flap.Channel != FlapChannel.SnacData) return;

        //        foreach (Snac x in flap.DataItems)
        //        {
        //            CallSnacHandlers(x);

        //            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, "Processed: {0}", Snac.GetKey(x));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Kernel.Exceptions.PublishException(ex);
        //    }
        //}

        #endregion

        #region  Data Sending

        private async Task SendData(ITcpContext context, BufferBlock<FlapDataPair> sendBuffer)
        {
            string id = Guid.NewGuid().ToString();

            try
            {
                while (context.ConnectionState == TcpConnectionState.Connected)
                {
                    FlapDataPair dataItem = await sendBuffer.ReceiveAsync();

                    Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                        "{0}@{1} SendBuffer received data {2} items in buffer",
                        id, context.Id, _sendBuffer.Count);

                    try
                    {
                        if (dataItem.Flap.Channel == FlapChannel.SnacData)
                        {
                            var s = (Snac) dataItem.Flap.DataItems.First();
                            int wait = Context.GetService<IRateLimitsService>().Calculate(s.ServiceId, s.SubtypeId);

                            if (wait > 0)
                                Thread.Sleep(wait);
                        }

                        context.SendData(dataItem.Data);

                        if (FlapSent != null)
                        {
                            FlapSent(this, new FlapTransportEventArgs(dataItem.Flap));
                        }

                        //TODO: For the moment we want it in this way so that this thread can continue to send without being disturbed
                        // by what happens on the other threads
                        Task.Run(() => dataItem.TaskCompletionSource.SetResult(dataItem.Flap.DatagramSequenceNumber));

                        Thread.Sleep(10); //TODO: Implement proper throtteling
                    }
                    catch (Exception sendException)
                    {
                        dataItem.TaskCompletionSource.SetException(sendException);
                        throw; // TODO: Evaluate this
                    }

                    Thread.Sleep(100); // TODO: Implement proper throttling according to rate limits.
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        public event EventHandler<FlapTransportEventArgs> FlapSent;

        //private void OnSendTimerTick(object state)
        //{
        //    try
        //    {
        //        FlapDataPair[] dataToSend;
        //        lock (_sendLock)
        //        {
        //            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, "Send timer tick {0} items in buffer",
        //                _sendBuffer.Count);

        //            if (_sendBuffer.Count == 0)
        //                return;

        //            dataToSend = _sendBuffer.ToArray();
        //            _sendBuffer.Clear();

        //            _sendTimer.Change(Timeout.Infinite, Timeout.Infinite);
        //            _sendTimerRunning = false;
        //        }

        //        foreach (var dataItem in dataToSend)
        //        {
        //            TcpContext.SendData(dataItem.Data);

        //            if (FlapSent != null)
        //            {
        //                FlapSent(this, new FlapTransportEventArgs(dataItem.Flap));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Kernel.Exceptions.PublishException(ex);
        //    }
        //}

        private Task<int> AddItemToSendBuffer(Flap flap)
        {
            //lock (_sendLock)
            //{
            //    _sendBuffer.Add(new FlapDataPair(flap));

            //    if (!_sendTimerRunning)
            //    {
            //        _sendTimer.Change(_sendTimerDue, Timeout.Infinite);
            //        _sendTimerRunning = true;
            //    }
            //}

            var pair = new FlapDataPair(flap);

            _sendBuffer.Post(pair);

            WriteOutFlapLog(pair.Data, flap);

            return pair.TaskCompletionSource.Task;
        }

        private Task<int[]> AddItemsToSendBuffer(IEnumerable<Flap> flaps)
        {
            //lock (_sendLock)
            //{
            //    _sendBuffer.AddRange(from x in flaps select new FlapDataPair(x));

            //    if (!_sendTimerRunning)
            //    {
            //        _sendTimer.Change(_sendTimerDue, Timeout.Infinite);
            //        _sendTimerRunning = true;
            //    }
            //}

            var pairs = flaps.Select(f => new FlapDataPair(f));
            var tasks = new List<Task<int>>();

            foreach (FlapDataPair pair in pairs)
            {
                _sendBuffer.Post(pair);

                WriteOutFlapLog(pair.Data, pair.Flap);

                tasks.Add(pair.TaskCompletionSource.Task);
            }

            return Task.WhenAll(tasks);
        }

        public Task<int> Send(Flap flap)
        {
            //if (!IsConnected)
            //    throw new InvalidOperationException("Invalid try to send data. TcpContext is not connected.");

            flap.DatagramSequenceNumber = Interlocked.Increment(ref _flapSequenceNumber);

            return AddItemToSendBuffer(flap);
        }

        public Task<int[]> Send(params Snac[] snacs)
        {
            return SendList(snacs);
        }

        public Task<int[]> SendList(IEnumerable<Snac> snacs)
        {
            //if (!IsConnected)
            //    throw new InvalidOperationException("Invalid try to send data. TcpContext is not connected.");

            var dataItems = new List<Flap>();

            foreach (Snac x in snacs)
            {
                var flap = new Flap(FlapChannel.SnacData);

                flap.DatagramSequenceNumber = Interlocked.Increment(ref _flapSequenceNumber);
                flap.DataItems.Add(x);

                dataItems.Add(flap);
            }

            return AddItemsToSendBuffer(dataItems);
        }

        #endregion
    }

    //public class TcpConnection
    //{
    //    public BufferBlock<FlapDataPair> SendBuffer { get; set; }

    //    public TcpContextNet45 TcpContext { get; set; }

    //    private TcpConnection()
    //    {
    //        TcpContext = new TcpContextNet45();
    //        SendBuffer = new BufferBlock<FlapDataPair>();

    //        TcpContext.Disconnected += OnTcpContextDisconnected;
    //    }

    //    private void OnTcpContextDisconnected(object sender, DisconnectEventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static TcpConnection Create(IPEndPoint endpoint)
    //    {
    //        var connection = new TcpConnection();

    //        connection.TcpContext.Connect(endpoint);

    //        Task.Run(() => AnalyzeData());
    //        Task.Run(() => SendData());
    //    }


    //    private async Task AnalyzeData()
    //    {
    //        // buffer bytes for analysis. if more bytes are needed to decode
    //        // the received data we wait for another cycle.

    //        var id = Guid.NewGuid().ToString();

    //        try
    //        {

    //            while (IsConnected)
    //            {
    //                // This call will return on its own Thread Pool Thread to
    //                // process the data.
    //                var data = await TcpContext.ReceivedDataBuffer.ReceiveAsync();

    //                // add data to the buffer.
    //                _analyzeBuffer.AddRange(data);

    //                var index = 0;
    //                var iloop = 0;

    //                while (index + 6 <= _analyzeBuffer.Count)
    //                {
    //                    // decode the flap header
    //                    var desc = FlapDescriptor.GetDescriptor(index, _analyzeBuffer);

    //                    if (_analyzeBuffer.Count < index + desc.TotalSize)
    //                    {
    //                        // there is more data needed to deserialize the flap. wait for another cycle...
    //                        Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
    //                            "{3}@{0}: caching {1}, {2} required.", iloop, _analyzeBuffer.Count - index, desc.TotalSize, id);

    //                        break;
    //                    }

    //                    Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
    //                        "{2}@{0}: queuing {1} bytes for analysis", iloop, desc.TotalSize, id);

    //                    ProcessFlap(_analyzeBuffer.GetRange(index, desc.TotalSize));

    //                    index += desc.TotalSize;
    //                    iloop += 1;
    //                }

    //                if (index > 0)
    //                    _analyzeBuffer.RemoveRange(0, index);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }

    //    }


    //}
}