// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac011e.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac011e : Snac
    {
        private readonly TlvDCInfo _DCInfo = new TlvDCInfo();
        private readonly TlvUserStatus _UserStatus = new TlvUserStatus();

        public Snac011e() : base(0x1, 0x1e)
        {
        }

        public TlvUserStatus UserStatus
        {
            get { return _UserStatus; }
        }

        public TlvDCInfo DCInfo
        {
            get { return _DCInfo; }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(_UserStatus.Serialize());

            //TODO: Snac011e.Serialize: Check whether DCInfo is supplied.
            data.AddRange(_DCInfo.Serialize());

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return _UserStatus.CalculateTotalSize() + _DCInfo.CalculateTotalSize();
        }
    }
}