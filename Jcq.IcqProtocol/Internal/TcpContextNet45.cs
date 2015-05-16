// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpContextNet45.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Internal;

namespace JCsTools.JCQ.IcqInterface
{
    public class TcpContextNet45 : ITcpContext, IDisposable
    {
        private static string mylog = "";
        private readonly string _id;
        private readonly BufferBlock<List<byte>> _receivedDataBuffer = new BufferBlock<List<byte>>();
        private readonly object _sendLock = new object();
        private TcpClient _client;
        private NetworkStream _stream;

        public TcpContextNet45()
        {
            _id = Guid.NewGuid().ToString();

            mylog += string.Format("{0}: {1}", DateTime.Now.ToLongTimeString(), _id);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public long BytesReceived { get; private set; }
        public long BytesSent { get; private set; }

        public string Id
        {
            get { return _id; }
        }

        public bool ConnectionCloseExpected { get; private set; }

        public void Connect(IPEndPoint endPoint)
        {
            _client = new TcpClient();
            _client.Connect(endPoint);

            _stream = _client.GetStream();

            OnConnected(endPoint);

            // Start Read Thread
            Task.Run(() => ReadTask());
        }

        public void Disconnect()
        {
            if (_client == null)
                return;

            ConnectionState = TcpConnectionState.Closed;
            _client.Close();
            OnDisconnected(ConnectionCloseExpected);
            _receivedDataBuffer.Complete();
        }

        public void SendData(List<byte> data)
        {
            //if (!_client.Connected | ConnectionState != TcpConnectionState.Connected)
            //    throw new InvalidOperationException("TcpClient has to be in a connected state to send data.");

            if (ConnectionState != TcpConnectionState.Connected)
                throw new InvalidOperationException("TcpClient has to be in a connected state to send data.");


            try
            {
                lock (_sendLock) // There should be only one thread sending on this network stream concurrently.
                {
                    _stream.Write(data.ToArray(), 0, data.Count);
                    BytesSent += data.Count;
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);

                if (ConnectionState == TcpConnectionState.Connected)
                {
                    ConnectionState = TcpConnectionState.Faulted;
                    OnDisconnected(ConnectionCloseExpected);
                    _receivedDataBuffer.Complete();
                }

                throw;
            }
        }

        public void SetCloseExpected()
        {
            ConnectionCloseExpected = true;
        }

        public void SetCloseUnexpected()
        {
            ConnectionCloseExpected = false;
        }

        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<DisconnectEventArgs> Disconnected;
        public TcpConnectionState ConnectionState { get; private set; }

        public BufferBlock<List<byte>> ReceivedDataBuffer
        {
            get { return _receivedDataBuffer; }
        }

        //public event EventHandler<DataReceivedEventArgs> DataReceived;

        private async Task ReadTask()
        {
            try
            {
                while (_client.Connected & ConnectionState == TcpConnectionState.Connected)
                {
                    var buffer = new byte[256];

                    var bytesRead = await _stream.ReadAsync(buffer, 0, 256);

                    if (bytesRead == 0)
                    {
                        if (ConnectionState == TcpConnectionState.Connected)
                        {
                            ConnectionState = TcpConnectionState.Closed;
                            OnDisconnected(ConnectionCloseExpected);
                            _receivedDataBuffer.Complete();
                        }

                        Debug.WriteLine("ReadTask ended since no data was read.", _id);
                        break;
                    }

                    var result = new List<byte>(buffer.Take(bytesRead));

                    while (_stream.DataAvailable)
                    {
                        bytesRead = _stream.Read(buffer, 0, 256);

                        result.AddRange(buffer.Take(bytesRead));
                    }

                    Debug.WriteLine(BitConverter.ToString(result.ToArray()));

                    BytesReceived += result.Count;

                    _receivedDataBuffer.Post(result);

                    //OnDataReceived(result);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);

                if (ConnectionState == TcpConnectionState.Connected)
                {
                    ConnectionState = TcpConnectionState.Faulted;
                    OnDisconnected(ConnectionCloseExpected);
                    _receivedDataBuffer.Complete();
                }

                //TODO: Close client
            }
        }

        //private void OnDataReceived(List<byte> data)
        //{
        //    if (DataReceived != null)
        //        DataReceived(this, new DataReceivedEventArgs(data));
        //}

        private void OnConnected(IPEndPoint endpoint)
        {
            if (Connected != null)
                Connected(this, new ConnectedEventArgs(endpoint));
        }

        private void OnDisconnected(bool expected)
        {
            if (Disconnected != null)
                Disconnected(this, new DisconnectEventArgs(expected));
        }
    }
}