// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITcpContext.cs" company="Jan-Cornelius Molnar">
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