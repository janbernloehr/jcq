// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0402.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0402 : Snac
    {
        public Snac0402() : base(0x4, 0x2)
        {
        }

        public int Channel { get; set; }
        public int MessageFlags { get; set; }
        public int MaxMessageSnacSize { get; set; }
        public int MaxSenderWarningLevel { get; set; }
        public int MaxReceiverWarningLevel { get; set; }
        public int MinimumMessageInterval { get; set; }

        public override int CalculateDataSize()
        {
            return 2 + 4 + 2 + 2 + 2 + 2 + 2;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) Channel));
            data.AddRange(ByteConverter.GetBytes((uint) MessageFlags));
            data.AddRange(ByteConverter.GetBytes((ushort) MaxMessageSnacSize));
            data.AddRange(ByteConverter.GetBytes((ushort) MaxSenderWarningLevel));
            data.AddRange(ByteConverter.GetBytes((ushort) MaxReceiverWarningLevel));
            data.AddRange(ByteConverter.GetBytes((ushort) MinimumMessageInterval));
            data.AddRange(ByteConverter.GetBytes(0));

            return data;
        }
    }
}