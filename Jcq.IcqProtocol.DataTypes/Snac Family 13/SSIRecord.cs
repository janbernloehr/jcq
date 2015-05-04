// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIRecord.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public abstract class SSIRecord : ISerializable
    {
        private string _ItemName;

        protected SSIRecord(SSIItemType type)
        {
            ItemType = type;
        }

        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (value == "143979279")
                    Debugger.Break();
                _ItemName = value;
            }
        }

        public int GroupId { get; set; }
        public int ItemId { get; set; }
        public SSIItemType ItemType { get; set; }

        protected int SizeFixPart
        {
            get { return 2 + _ItemName.Length + 2 + 2 + 2 + 2; }
        }

        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return SizeFixPart + DataSize; }
        }

        public bool HasData { get; private set; }
        public abstract int CalculateDataSize();

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public virtual void Deserialize(List<byte> data)
        {
            var index = 0;

            _ItemName = ByteConverter.ToStringFromUInt16Index(index, data);
            index += _ItemName.Length + 2;

            GroupId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            ItemId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            ItemType = (SSIItemType) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;
        }

        public virtual List<byte> Serialize()
        {
            var data = new List<byte>();
            int dataSize;

            dataSize = CalculateDataSize();

            data.AddRange(ByteConverter.GetBytes((ushort) _ItemName.Length));
            data.AddRange(ByteConverter.GetBytes(_ItemName));
            data.AddRange(ByteConverter.GetBytes((ushort) GroupId));
            data.AddRange(ByteConverter.GetBytes((ushort) ItemId));
            data.AddRange(ByteConverter.GetBytes((ushort) ItemType));
            data.AddRange(ByteConverter.GetBytes((ushort) dataSize));

            return data;
        }
    }
}