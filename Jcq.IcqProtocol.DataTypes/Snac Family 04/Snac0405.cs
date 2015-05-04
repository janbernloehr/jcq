// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0405.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0405 : Snac
    {
        public Snac0405() : base(0x4, 0x5)
        {
        }

        public int Channel { get; set; }
        public long MessageFlags { get; set; }
        public int MaxMessageSnacSize { get; set; }
        public int MaxSenderWarningLevel { get; set; }
        public int MaxReceiverWarningLevel { get; set; }
        public int MinimumMessageInterval { get; set; }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            Channel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MessageFlags = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            MaxMessageSnacSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MaxSenderWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MaxReceiverWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MinimumMessageInterval = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            //unknown param
            index += 2;

            SetTotalSize(index);
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}