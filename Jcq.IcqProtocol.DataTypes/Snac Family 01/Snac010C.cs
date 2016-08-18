// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac010C.cs" company="Jan-Cornelius Molnar">
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
    public class Snac010C : Snac
    {
        public Snac010C() : base(0x1, 0xc)
        {
        }

        public List<int> RateGroupIds { get; } = new List<int>();

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (int x in RateGroupIds)
            {
                data.AddRange(ByteConverter.GetBytes((ushort) x));
            }

            return data;
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            base.Deserialize(descriptor, data);

            int index = SizeFixPart;

            while (index + 2 <= data.Count)
            {
                RateGroupIds.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

                index += 2;
            }

            TotalSize = index;
            return index;
        }

        public override int CalculateDataSize()
        {
            return RateGroupIds.Count*2;
        }
    }
}