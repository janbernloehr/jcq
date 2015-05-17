// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITcpContext.cs" company="Jan-Cornelius Molnar">
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
using System.Net;
using System.Threading.Tasks.Dataflow;

namespace JCsTools.JCQ.IcqInterface.Internal
{
    public interface ITcpContext
    {
        /// <summary>
        ///     Gets the number of bytes received.
        /// </summary>
        long BytesReceived { get; }

        /// <summary>
        ///     Gets the number of bytes sent.
        /// </summary>
        long BytesSent { get; }

        /// <summary>
        ///     Gets a unique identifier for this ITcpContext.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the connection state.
        /// </summary>
        TcpConnectionState ConnectionState { get; }

        /// <summary>
        ///     Gets a value indicating whether an imminent closing of the connection is expected.
        /// </summary>
        bool ConnectionCloseExpected { get; }

        //event EventHandler<DataReceivedEventArgs> DataReceived;


        BufferBlock<List<byte>> ReceivedDataBuffer { get; }

        /// <summary>
        ///     Connect to the given endpoint. If the connection is successfull,
        ///     the ReceivedDataBuffer is filled with incoming data and data can be send to the endpoint.
        /// </summary>
        /// <param name="endPoint"></param>
        void Connect(IPEndPoint endPoint);

        /// <summary>
        ///     Disconnect from the endpoint.
        /// </summary>
        void Disconnect();

        void SendData(List<byte> data);
        void SetCloseExpected();
        void SetCloseUnexpected();
        event EventHandler<ConnectedEventArgs> Connected;
        event EventHandler<DisconnectEventArgs> Disconnected;
    }


    public enum TcpConnectionState
    {
        Connected,
        Faulted,
        Closed
    }
}