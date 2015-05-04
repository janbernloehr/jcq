// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIItemDescriptor.cs" company="Jan-Cornelius Molnar">
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
    public class SSIItemDescriptor
    {
        public string ItemName { get; private set; }
        public int GroupId { get; private set; }
        public int ItemId { get; private set; }
        public SSIItemType ItemType { get; private set; }
        public int AdditionalDataIndex { get; private set; }

        public int TotalSize
        {
            get { return DataSize + 2 + 2 + 2 + 2 + 2 + ItemName.Length; }
        }

        public int DataSize { get; private set; }

        public static SSIItemDescriptor GetDescriptor(int offset, List<byte> data)
        {
            var descriptor = new SSIItemDescriptor();
            descriptor.Deserialize(offset, data);
            return descriptor;
        }

        private void Deserialize(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);
            var index = 0;

            ItemName = ByteConverter.ToStringFromUInt16Index(index, data);
            index += ItemName.Length + 2;

            GroupId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            ItemId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            ItemType = (SSIItemType) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            AdditionalDataIndex = offset + index;
        }
    }
}