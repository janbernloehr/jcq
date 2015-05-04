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
    public class TcpContext : ITcpContext, IDisposable
    {
        public delegate void ConnectedEventHandler(object sender, ConnectedEventArgs e);

        public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

        public delegate void DisconnectedEventHandler(object sender, DisconnectEventArgs e);

        private readonly string _Id = string.Concat("TcpContext:", Guid.NewGuid().ToString());
        private readonly Object _lock = new Object();
        private long _bytesReceived;
        private long _bytesSent;
        private TcpClient _Client;

        public void Dispose()
        {
            if (_Client == null) return;

            _Client.Close();
            _Client = null;
        }

        public string Id
        {
            get { return _Id; }
        }

        public bool IsConnected { get; private set; }

        public long BytesReceived
        {
            get { return _bytesReceived; }
        }

        public long BytesSent
        {
            get { return _bytesSent; }
        }

        public bool ConnectionCloseExpected { get; private set; }

        public void SetCloseExpected()
        {
            lock (_lock)
            {
                ConnectionCloseExpected = true;
                Debug.WriteLine(string.Format("{0} expecting disconnect.", Environment.TickCount), _Id);
            }
        }

        public void SetCloseUnexpected()
        {
            lock (_lock)
            {
                ConnectionCloseExpected = false;
                Debug.WriteLine(string.Format("{0} expecting alive.", Environment.TickCount), _Id);
            }
        }

        public void Connect(IPEndPoint endPoint)
        {
            var stream = default(NetworkStream);
            var info = default(ReceiveInfo);

            Debug.WriteLine(string.Format("Connecting to {0}.", endPoint), _Id);

            _Client = new TcpClient();
            _Client.Connect(endPoint);

            IsConnected = true;

            stream = _Client.GetStream();

            info = new ReceiveInfo(stream);

            stream.BeginRead(info.buffer, 0, ReceiveInfo.BUFFERSIZE, ReadCallback, info);
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            _Client.Close();
            OnDisconnected();
        }

        public void SendData(List<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Count == 0)
                throw new ArgumentException("List<Byte> does not contain data.");

            byte[] buffer = null;

            try
            {
                buffer = data.ToArray();
                lock (_lock)
                {
                    var stream = default(NetworkStream);

                    stream = _Client.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }

                Interlocked.Add(ref _bytesSent, buffer.Length);
            }
            catch
            {
                OnDisconnected();
                throw;
            }
        }

        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<DisconnectEventArgs> Disconnected;
        public event EventHandler<Internal.DataReceivedEventArgs> DataReceived;

        private void ReadCallback(IAsyncResult ar)
        {
            var info = (ReceiveInfo) ar.AsyncState;
            var data = default(List<byte>);

            try
            {
                lock (_lock)
                {
                    var dataSize = 0;

                    dataSize = info.Stream.EndRead(ar);
                    data = new List<byte>(info.buffer);
                    data = data.GetRange(0, dataSize);

                    while (info.Stream.DataAvailable)
                    {
                        var buffer = new byte[ReceiveInfo.BUFFERSIZE];

                        dataSize = info.Stream.Read(buffer, 0, ReceiveInfo.BUFFERSIZE);
                        data.AddRange(new List<byte>(buffer).GetRange(0, dataSize));
                    }

                    info = new ReceiveInfo(info.Stream);

                    if (data.Count > 0)
                    {
                        info.Stream.BeginRead(info.buffer, 0, ReceiveInfo.BUFFERSIZE, ReadCallback, info);

                        ThreadPool.QueueUserWorkItem(AsyncRaiseDataReceived, data);
                    }
                    else
                    {
                        Debug.WriteLine("Transfer loop ended with no data.", _Id);
                        OnDisconnected();
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
                OnDisconnected();
            }
        }

        private void AsyncRaiseDataReceived(object state)
        {
            var data = default(List<byte>);

            try
            {
                data = (List<byte>) state;

                Interlocked.Add(ref _bytesReceived, data.Count);

                OnDataReceived(data);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
                OnDisconnected();
            }
        }

        private void OnDataReceived(IEnumerable<byte> data)
        {
            if (DataReceived != null)
            {
                DataReceived(this, new Internal.DataReceivedEventArgs(data));
            }
        }

        private void OnConnected(IPEndPoint endpoint)
        {
            if (Connected != null)
            {
                Connected(this, new ConnectedEventArgs(endpoint));
            }
        }

        private void OnDisconnected()
        {
            var expected = false;

            if (!IsConnected)
                return;

            IsConnected = false;
            expected = ConnectionCloseExpected;

            if (expected)
            {
                Debug.WriteLine(string.Format("{0} Connection closed (expected).", Environment.TickCount), _Id);
            }
            else
            {
                Debug.WriteLine(string.Format("{0} Connection closed (unexpected).", Environment.TickCount), _Id);
            }

            if (Disconnected != null)
            {
                Disconnected(this, new DisconnectEventArgs(expected));
            }
        }
    }
}