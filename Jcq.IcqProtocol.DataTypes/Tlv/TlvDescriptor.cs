// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvDescriptor.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class TlvDescriptor
    {
        private TlvDescriptor(int typeId, int dataSize)
        {
            TypeId = typeId;
            DataSize = dataSize;
        }

        public int TypeId { get; private set; }
        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return DataSize + 4; }
        }

        public static TlvDescriptor GetDescriptor(int offset, List<byte> data)
        {
            int typeId;
            int dataSize;

            if (offset + 4 > data.Count)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                throw new ArgumentException(
                    string.Format("Offset and length were out of bounds for the present data: {0}/{1}", offset + 4,
                        data.Count));
            }

            typeId = ByteConverter.ToUInt16(data.GetRange(offset, 2));
            dataSize = ByteConverter.ToUInt16(data.GetRange(offset + 2, 2));

            return new TlvDescriptor(typeId, dataSize);
        }
    }
}