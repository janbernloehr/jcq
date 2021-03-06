// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapDescriptor.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.DataTypes
{
    public class FlapDescriptor
    {
        public FlapChannel Channel { get; set; }
        public int DatagramSequenceNumber { get; set; }
        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return 6 + DataSize; }
        }

        public static FlapDescriptor GetDescriptor(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);

            var desc = new FlapDescriptor();
            desc.Deserialize(data);

            return desc;
        }

        private void Deserialize(List<byte> data)
        {
            int index = 0;

            if (data[index] != 0x2a)
            {
                string info = null;

                for (int i = 0; i <= Flap.SizeFixPart - 1; i++)
                {
                    info += string.Format("{0:X} ", data[i]);
                }

                throw new InvalidOperationException(string.Format("No flap at this position: {0}", info));
            }

            index += 1;

            Channel = (FlapChannel) data[index];
            index += 1;

            DatagramSequenceNumber = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;
        }
    }
}