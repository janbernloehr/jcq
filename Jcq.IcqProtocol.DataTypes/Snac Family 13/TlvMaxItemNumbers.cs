// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvMaxItemNumbers.cs" company="Jan-Cornelius Molnar">
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
    public class TlvMaxItemNumbers : Tlv
    {
        public TlvMaxItemNumbers() : base(0x4)
        {
        }

        public int MaxContacts { get; set; }
        public int MaxGroups { get; set; }
        public int MaxVisibleContacts { get; set; }
        public int MaxInvisibleContacts { get; set; }
        public int MaxVisibleInvisibleBitmasks { get; set; }
        public int MaxPresenseInfoFields { get; set; }
        public int MaxIgnoreListEntries { get; set; }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            MaxContacts = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MaxGroups = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MaxVisibleContacts = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MaxInvisibleContacts = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MaxPresenseInfoFields = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            index += 2*8;

            MaxIgnoreListEntries = ByteConverter.ToUInt16(data.GetRange(index, 2));
        }
    }
}