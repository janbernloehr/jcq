// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac011f.cs" company="Jan-Cornelius Molnar">
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
    public class Snac011f : Snac
    {
        public Snac011f() : base(0x1, 0x1f)
        {
        }

        public long RequestedDataOffset { get; set; }
        public long RequestedDataLength { get; set; }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            RequestedDataOffset = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            RequestedDataLength = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 8;
        }
    }
}