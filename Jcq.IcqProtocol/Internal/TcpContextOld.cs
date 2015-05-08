// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpContext.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Internal;
using DataReceivedEventArgs = System.Diagnostics.DataReceivedEventArgs;

namespace JCsTools.JCQ.IcqInterface
{
//    public class TcpContext : ITcpContext, IDisposable
//    {
//        private readonly string _id = string.Concat("TcpContext:", Guid.NewGuid().ToString());
//        private readonly Object _lock = new Object();
//        private long _bytesReceived;
//        private long _bytesSent;
//        private TcpClient _client;

//        public void Dispose()
//        {
//            if (_client == null) return;

//            _client.Close();
//            _client = null;
//        }

//        public string Id
//        {
//            get { return _id; }
//        }

//        public bool IsConnected { get; private set; }

//        public long BytesReceived
//        {
//            get { return _bytesReceived; }
//        }

//        public long BytesSent
//        {
//            get { return _bytesSent; }
//        }

//        public bool ConnectionCloseExpected { get; private set; }

//        public void SetCloseExpected()
//        {
//            lock (_lock)
//            {
//                ConnectionCloseExpected = true;
//                Debug.WriteLine(string.Format("{0} expecting disconnect.", Environment.TickCount), _id);
//            }
//        }

//        public void SetCloseUnexpected()
//        {
//            lock (_lock)
//            {
//                ConnectionCloseExpected = false;
//                Debug.WriteLine(string.Format("{0} expecting alive.", Environment.TickCount), _id);
//            }
//        }

//        public void Connect(IPEndPoint endPoint)
//        {
//            Debug.WriteLine(string.Format("Connecting to {0}.", endPoint), _id);

//            _client = new TcpClient();
//            _client.Connect(endPoint);

//            IsConnected = true;

//            var stream = _client.GetStream();

//            var info = new ReceiveInfo(stream);

//            stream.BeginRead(info.buffer, 0, ReceiveInfo.BUFFERSIZE, ReadCallback, info);
//        }

//        public void Disconnect()
//        {
//            if (!IsConnected)
//                return;

//            _client.Close();
//            OnDisconnected();
//        }

//        public void SendData(List<byte> data)
//        {
//            if (data == null)
//                throw new ArgumentNullException("data");
//            if (data.Count == 0)
//                throw new ArgumentException("List<Byte> does not contain data.");

//            try
//            {
//                var buffer = data.ToArray();
//                lock (_lock)
//                {
//                    var stream  = _client.GetStream();

//                    stream.Write(buffer, 0, buffer.Length);
//                }

//                Interlocked.Add(ref _bytesSent, buffer.Length);
//            }
//            catch
//            {
//                OnDisconnected();
//                throw;
//            }
//        }

//        public event EventHandler<ConnectedEventArgs> Connected;
//        public event EventHandler<DisconnectEventArgs> Disconnected;
//        public event EventHandler<Internal.DataReceivedEventArgs> DataReceived;

//        private void ReadCallback(IAsyncResult ar)
//        {
//            var info = (ReceiveInfo) ar.AsyncState;
           
//            try
//            {
//                lock (_lock)
//                {
//                    var dataSize = info.Stream.EndRead(ar);
//                    var data = new List<byte>(info.buffer);

//                    data = data.GetRange(0, dataSize);

//                    while (info.Stream.DataAvailable)
//                    {
//                        var buffer = new byte[ReceiveInfo.BUFFERSIZE];

//                        dataSize = info.Stream.Read(buffer, 0, ReceiveInfo.BUFFERSIZE);
//                        data.AddRange(new List<byte>(buffer).GetRange(0, dataSize));
//                    }

//                    info = new ReceiveInfo(info.Stream);

//                    if (data.Count > 0)
//                    {
//                        info.Stream.BeginRead(info.buffer, 0, ReceiveInfo.BUFFERSIZE, ReadCallback, info);

//                        ThreadPool.QueueUserWorkItem(AsyncRaiseDataReceived, data);
//                    }
//                    else
//                    {
//                        Debug.WriteLine("Transfer loop ended with no data.", _id);
//                        OnDisconnected();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Kernel.Exceptions.PublishException(ex);
//                OnDisconnected();
//            }
//        }

//        private void AsyncRaiseDataReceived(object state)
//        {
//            try
//            {
//                var data = (List<byte>) state;

//                Interlocked.Add(ref _bytesReceived, data.Count);

//                OnDataReceived(data);
//            }
//            catch (Exception ex)
//            {
//                Kernel.Exceptions.PublishException(ex);
//                OnDisconnected();
//            }
//        }

//        private void OnDataReceived(IEnumerable<byte> data)
//        {
//            if (DataReceived != null)
//            {
//                DataReceived(this, new Internal.DataReceivedEventArgs(data));
//            }
//        }

//        private void OnConnected(IPEndPoint endpoint)
//        {
//            if (Connected != null)
//            {
//                Connected(this, new ConnectedEventArgs(endpoint));
//            }
//        }

//        private void OnDisconnected()
//        {
//            if (!IsConnected)
//                return;

//            IsConnected = false;
//            var expected = ConnectionCloseExpected;

//            Debug.WriteLine(
//                expected
//                    ? string.Format("{0} Connection closed (expected).", Environment.TickCount)
//                    : string.Format("{0} Connection closed (unexpected).", Environment.TickCount), _id);

//            if (Disconnected != null)
//            {
//                Disconnected(this, new DisconnectEventArgs(expected));
//            }
//        }
//    }
}