// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseConnector.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Jcq.IcqProtocol.Internal;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface.Internal
{
    public class BaseConnector : ContextService, IIcqDataTranferService
    {
        private readonly List<byte> _analyzeBuffer;
        private readonly object _analyzeLock;
        private readonly List<FlapDataPair> _sendBuffer;
        private readonly object _sendLock;
        private readonly Timer _sendTimer;
        private readonly int _sendTimerDue;
        private readonly Dictionary<string, List<Delegate>> _snacHandlers;
        private int _flapSequenceNumber;
        private bool _sendTimerRunning;

        public BaseConnector(IContext context) : base(context)
        {
            FlapReceived += OnFlapReceived;
            TcpContext = new TcpContext();

            _analyzeLock = new object();
            _analyzeBuffer = new List<byte>();

            _snacHandlers = new Dictionary<string, List<Delegate>>();

            _sendLock = new object();
            _sendBuffer = new List<FlapDataPair>();
            _sendTimer = new Timer(OnSendTimerTick, null, Timeout.Infinite, Timeout.Infinite);
            _sendTimerDue = 100;
        }

        public bool IsConnected
        {
            get { return TcpContext != null && TcpContext.IsConnected; }
        }

        public TcpContext TcpContext { get; private set; }

        public void RegisterSnacHandler<T>(int serviceId, int subtypeId, Action<T> handler) where T : Snac
        {
            string key = null;
            List<Delegate> handlerList = null;

            key = string.Format("{0:X2},{1:X2}", serviceId, subtypeId);

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
            var hostName = ConfigurationManager.AppSettings["OscarServer"];
            var hostPort = int.Parse(ConfigurationManager.AppSettings["OscarPort"]);

            var hentry = Dns.GetHostEntry(hostName);
            var endpoint = new IPEndPoint(hentry.AddressList[0], hostPort);

            InnerConnect(endpoint);
        }

        protected virtual void InnerConnect(IPEndPoint endpoint)
        {
            InnerDisconnect();

            TcpContext = new TcpContext();

            TcpContext.Connect(endpoint);

            TcpContext.DataReceived += OnTcpContextDataReceived;
            TcpContext.Disconnected += OnTcpContextDisconnected;

            OnInternalConnected(EventArgs.Empty);
        }

        protected virtual void InnerDisconnect()
        {
            if (TcpContext != null)
                TcpContext.Disconnect();
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
            string[] serverAddressParts = null;
            var serverIp = default(IPAddress);
            var serverPort = 0;

            if (address == null)
                throw new ArgumentNullException("address");

            serverAddressParts = address.Split(':');

            serverIp = IPAddress.Parse(serverAddressParts[0]);
            serverPort = int.Parse(serverAddressParts[1]);

            return new IPEndPoint(serverIp, serverPort);
        }

        private void OnTcpContextDisconnected(object sender, DisconnectEventArgs e)
        {
            OnInternalDisconnected(e);
        }

        private void CallSnacHandlers(Snac snac)
        {
            var key = Snac.GetKey(snac);
            List<Delegate> handlers = null;

            try
            {
                Debug.WriteLine(string.Format(">> Snac {0}", key), "BaseConnector");

                if (_snacHandlers.TryGetValue(key, out handlers))
                {
                    foreach (var x in handlers)
                    {
                        x.DynamicInvoke(snac);
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #region " Data Receiving "

        public event EventHandler<FlapTransportEventArgs> FlapReceived;

        private void OnTcpContextDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            try
            {
                lock (_analyzeLock)
                {
                    if (_analyzeBuffer.Count > 0)
                    {
                        Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                            "data received. already cached: {0}, new: {1}, ticket: {2}", _analyzeBuffer.Count,
                            data.Count, e.Ticket);
                        data.InsertRange(0, _analyzeBuffer);
                        _analyzeBuffer.Clear();
                    }

                    var index = 0;
                    var iloop = 0;

                    while (index + 6 <= data.Count)
                    {
                        var desc = FlapDescriptor.GetDescriptor(index, data);

                        if (data.Count < index + desc.TotalSize)
                        {
                            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                                "{0}: caching {1}, {2} more required.", iloop, data.Count - index, desc.TotalSize);
                            _analyzeBuffer.AddRange(data.GetRange(index, data.Count - index));
                        }
                        else
                        {
                            Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose,
                                "{0}: queuing {1} bytes for analyzation", iloop, data.Count - index, desc.TotalSize);
                            ThreadPool.QueueUserWorkItem(AsyncPreAnalyze, data.GetRange(index, desc.TotalSize));
                        }

                        index += desc.TotalSize;
                        iloop += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AsyncPreAnalyze(object state)
        {
            var data = default(List<byte>);
            var f = default(Flap);

            try
            {
                data = (List<byte>) state;

                f = new Flap();
                f.Deserialize(data);
                if (FlapReceived != null)
                {
                    FlapReceived(this, new FlapTransportEventArgs(f));
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private string ToHex(IEnumerable<byte> data)
        {
            var sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.AppendFormat("{0:X2} ", b);
            }
            return sb.ToString();
        }

        private void OnFlapReceived(object sender, FlapTransportEventArgs e)
        {
            var flap = e.Flap;

            try
            {
                if (flap.Channel == FlapChannel.SnacData)
                {
                    foreach (Snac x in flap.DataItems)
                    {
                        CallSnacHandlers(x);

                        Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, "Processed: {0}", Snac.GetKey(x));
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #endregion

        #region " Data Sending "

        public event EventHandler<FlapTransportEventArgs> FlapSent;

        private void OnSendTimerTick(object state)
        {
            FlapDataPair[] dataToSend = null;

            try
            {
                lock (_sendLock)
                {
                    Kernel.Logger.Log("BaseConnector", TraceEventType.Verbose, "Send timer tick {0} items in buffer",
                        _sendBuffer.Count);

                    if (_sendBuffer.Count == 0)
                        return;

                    dataToSend = _sendBuffer.ToArray();
_sendBuffer.Clear();

                    _sendTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _sendTimerRunning = false;
                }

                foreach (var dataItem in dataToSend)
                {
                    TcpContext.SendData(dataItem.Data);

                    if (FlapSent != null)
                    {
                        FlapSent(this, new FlapTransportEventArgs(dataItem.Flap));
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AddItemToSendBuffer(Flap flap)
        {
            lock (_sendLock)
            {
                _sendBuffer.Add(new FlapDataPair(flap));

                if (!_sendTimerRunning)
                {
                    _sendTimer.Change(_sendTimerDue, Timeout.Infinite);
                    _sendTimerRunning = true;
                }
            }
        }

        private void AddItemsToSendBuffer(IEnumerable<Flap> flaps)
        {
            lock (_sendLock)
            {
                _sendBuffer.AddRange(from x in flaps select new FlapDataPair(x));

                if (!_sendTimerRunning)
                {
                    _sendTimer.Change(_sendTimerDue, Timeout.Infinite);
                    _sendTimerRunning = true;
                }
            }
        }

        public void Send(Flap flap)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Invalid try to send data. TcpContext is not connected.");

            //_flapSequenceNumber += 1

            flap.DatagramSequenceNumber = Interlocked.Increment(ref _flapSequenceNumber);

            //_TcpContext.SendData(flap.Serialize)

            //RaiseEvent FlapSent(Me, New FlapTransportEventArgs(flap))

            AddItemToSendBuffer(flap);
        }

        public void Send(params Snac[] snacs)
        {
            SendList(snacs);
        }

        public void SendList(IEnumerable<Snac> snacs)
        {
            var flap = default(Flap);
            //Dim data As List(Of Byte) = New List(Of Byte)
            var dataItems = default(List<Flap>);

            if (!IsConnected)
                throw new InvalidOperationException("Invalid try to send data. TcpContext is not connected.");

            dataItems = new List<Flap>();

            foreach (var x in snacs)
            {
                flap = new Flap(FlapChannel.SnacData);

                //_flapSequenceNumber += 1

                flap.DatagramSequenceNumber = Interlocked.Increment(ref _flapSequenceNumber);
                flap.DataItems.Add(x);

                //data.AddRange(flap.Serialize)
                dataItems.Add(flap);

                //RaiseEvent FlapSent(Me, New FlapTransportEventArgs(flap))
            }

            //_TcpContext.SendData(data)

            AddItemsToSendBuffer(dataItems);
        }

        #endregion
    }
}