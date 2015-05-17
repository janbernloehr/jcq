// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIItemDescriptor.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.DataTypes
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