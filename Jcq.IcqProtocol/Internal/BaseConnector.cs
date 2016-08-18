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
        private readonly Dictionary<Tuple<int,int>, List<Delegate>> _snacHandlers;
        private int _flapSequenceNumber;
        private BufferBlock<FlapDataPair> _sendBuffer;

        public BaseConnector(IContext context)
            : base(context)
        {
            _snacHandlers = new Dictionary<Tuple<int, int>, List<Delegate>>();
        }

        public bool IsConnected => TcpContext != null && TcpContext.ConnectionState == TcpConnectionState.Connected;

        public ITcpContext TcpContext { get; private set; }

        public void RegisterSnacHandler<T>(int serviceId, int subtypeId, Action<T> handler) where T : Snac
        {
            List<Delegate> handlerList;

            var key = new Tuple<int,int>(serviceId, subtypeId);

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

            TcpContext.Disconnected += OnTcpContextDisconnected;

            OnInternalConnected(EventArgs.Empty);

            _sendBuffer = new BufferBlock<FlapDataPair>();

            Task.Run(() => AnalyzeData(TcpContext));
            Task.Run(() => SendData(TcpContext, _sendBuffer));

            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, $"{TcpContext.Id} connected to {endpoint}");
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
            InternalConnected?.Invoke(this, e);
        }

        protected virtual void OnInternalDisconnected(DisconnectEventArgs e)
        {
            InternalDisconnected?.Invoke(this, e);
        }

        protected IPEndPoint ConvertServerAddressToEndPoint(string address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

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
            var key = Snac.GetKey(snac);

            try
            {
                Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, $"Processed: {key}");
                Debug.WriteLine($"<< Snac {key.Item1:X2},{key.Item2:X2}", "BaseConnector");

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
                                $"{id}@{context.Id}/{iloop}: caching {_analyzeBuffer.Count - index}, {desc.TotalSize} required.");

                            break;
                        }

                        Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                            $"{id}@{context.Id}/{iloop}: queuing {desc.TotalSize} bytes for analysis");

                        ProcessFlap(desc, _analyzeBuffer.GetRange(index, desc.TotalSize));

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

        private void ProcessFlap(FlapDescriptor desc, List<byte> flapData)
        {
            try
            {
                var flap = new Flap();

                WriteInFlapLog(flapData, desc);

                flap.Deserialize(desc, flapData);

                FlapReceived?.Invoke(this, new FlapTransportEventArgs(flap));

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

        private void WriteFlapLog(string modifier, List<byte> flapData, IFlapDescriptor descriptor)
        {
            try
            {
                string path = string.Format("jcq/dumps/{0}/{3}/{1}_{2}.json", TcpContext.Id, descriptor.DatagramSequenceNumber,
                    descriptor.Channel == FlapChannel.SnacData 
                        ? descriptor.SnacKey.Replace(",", "")
                        : Enum.GetName(typeof(FlapChannel), descriptor.Channel),
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

        private void WriteInFlapLog(List<byte> flapData, IFlapDescriptor flap)
        {
            WriteFlapLog("in", flapData, flap);
        }

        private void WriteOutFlapLog(FlapDataPair pair)
        {
            WriteFlapLog("out", pair.Data, pair.Flap);
        }
        
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
                        $"{id}@{context.Id} SendBuffer received data {_sendBuffer.Count} items in buffer");

                    try
                    {
                        if (dataItem.Flap.Channel == FlapChannel.SnacData)
                        {
                            var s = (Snac) dataItem.Flap.DataItems.First();
                            TimeSpan wait = Context.GetService<IRateLimitsService>().Calculate(s.ServiceId, s.SubtypeId);

                            if (wait > TimeSpan.Zero)
                            {
                                Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                        $"{id}@{context.Id} Waiting {wait} for rate limit");

                                await Task.Delay(wait);
                            }
                        }

                        context.SendData(dataItem.Data);

                        FlapSent?.Invoke(this, new FlapTransportEventArgs(dataItem.Flap));

                        //TODO: For the moment we want it in this way so that this thread can continue to send without being disturbed
                        // by what happens on the other threads
                        Task.Run(() => dataItem.TaskCompletionSource.SetResult(dataItem.Flap.DatagramSequenceNumber));
                    }
                    catch (Exception sendException)
                    {
                        dataItem.TaskCompletionSource.SetException(sendException);
                        throw; // TODO: Evaluate this
                    }

                    await Task.Delay(100); //TODO: Implement proper throtteling
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        public event EventHandler<FlapTransportEventArgs> FlapSent;
        
        private Task<int> AddItemToSendBuffer(Flap flap)
        {
            var pair = new FlapDataPair(flap);

            _sendBuffer.Post(pair);

            WriteOutFlapLog(pair);

            return pair.TaskCompletionSource.Task;
        }

        private Task<int[]> AddItemsToSendBuffer(IEnumerable<Flap> flaps)
        {
            var pairs = flaps.Select(f => new FlapDataPair(f));
            var tasks = new List<Task<int>>();

            foreach (FlapDataPair pair in pairs)
            {
                _sendBuffer.Post(pair);

                WriteOutFlapLog(pair);

                tasks.Add(pair.TaskCompletionSource.Task);
            }

            return Task.WhenAll(tasks);
        }

        public Task<int> Send(Flap flap)
        {
            flap.DatagramSequenceNumber = Interlocked.Increment(ref _flapSequenceNumber);

            return AddItemToSendBuffer(flap);
        }

        public Task<int[]> Send(params Snac[] snacs)
        {
            return SendList(snacs);
        }

        public Task<int[]> SendList(IEnumerable<Snac> snacs)
        {
            var dataItems = new List<Flap>();

            foreach (Snac x in snacs)
            {
                var flap = new Flap(FlapChannel.SnacData)
                {
                    DatagramSequenceNumber = Interlocked.Increment(ref _flapSequenceNumber)
                };

                flap.DataItems.Add(x);

                dataItems.Add(flap);
            }

            return AddItemsToSendBuffer(dataItems);
        }

        #endregion
    }
}