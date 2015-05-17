// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIRecord.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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