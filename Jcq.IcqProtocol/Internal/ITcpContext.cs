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

namespace JCsTools.JCQ.IcqInterface.Internal
{
    public interface ITcpContext
    {
        long BytesReceived { get; }
        long BytesSent { get; }
        string Id { get; }
        bool IsConnected { get; }
        bool ConnectionCloseExpected { get; }
        void Connect(IPEndPoint endPoint);
        void Disconnect();
        void SendData(List<byte> data);
        void SetCloseExpected();
        void SetCloseUnexpected();
        event EventHandler<ConnectedEventArgs> Connected;
        event EventHandler<DisconnectEventArgs> Disconnected;
        event EventHandler<DataReceivedEventArgs> DataReceived;
    }
}