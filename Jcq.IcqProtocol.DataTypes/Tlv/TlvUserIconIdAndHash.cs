// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvUserIconIdAndHash.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class TlvUserIconIdAndHash : Tlv
    {
        private readonly List<byte> _IconMD5Hash = new List<byte>();

        public TlvUserIconIdAndHash() : base(0x1d)
        {
        }

        public int IconId { get; set; }
        public byte IconFlags { get; set; }
        public byte IconHashLenght { get; set; }

        public List<byte> IconMD5Hash
        {
            get { return _IconMD5Hash; }
        }

        public override int CalculateDataSize()
        {
            return 2 + 1 + 1 + _IconMD5Hash.Count;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            IconId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            IconFlags = data[index];
            index += 1;

            IconHashLenght = data[index];
            index += 1;

            _IconMD5Hash.AddRange(data.GetRange(index, IconHashLenght));
            index += _IconMD5Hash.Count;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) IconId));
            data.Add(IconFlags);
            data.Add(IconHashLenght);
            data.AddRange(_IconMD5Hash);

            return data;
        }
    }
}