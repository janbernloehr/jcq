// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1314.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1314 : Snac
    {
        public Snac1314() : base(0x13, 0x14)
        {
        }

        public string Uin { get; set; }
        public string Message { get; set; }

        public override int CalculateDataSize()
        {
            return 1 + Uin.Length + 2 + Message.Length + 2;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.Add((byte) Uin.Length);
            data.AddRange(ByteConverter.GetBytes(Uin));

            data.AddRange(ByteConverter.GetBytes((ushort) Message.Length));
            data.AddRange(ByteConverter.GetBytes(Message));

            data.AddRange(ByteConverter.GetBytes(0));

            return data;
        }
    }
}