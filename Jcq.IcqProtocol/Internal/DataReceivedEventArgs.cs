// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataReceivedEventArgs.cs" company="Jan-Cornelius Molnar">
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
using System.Threading;

namespace JCsTools.JCQ.IcqInterface.Internal
{
    public class DataReceivedEventArgs : EventArgs
    {
        private static int _ticketCounter;
        private readonly List<byte> _data;
        private readonly int _ticket;

        public DataReceivedEventArgs(List<byte> data)
        {
            _data = data;

            _ticket = Interlocked.Increment(ref _ticketCounter);
        }

        public List<byte> Data
        {
            get { return _data; }
        }

        public int Ticket
        {
            get { return _ticket; }
        }
    }
}