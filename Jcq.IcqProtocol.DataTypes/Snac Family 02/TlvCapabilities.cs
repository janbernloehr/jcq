// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvCapabilities.cs" company="Jan-Cornelius Molnar">
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
    public class TlvCapabilities : Tlv
    {
        public TlvCapabilities() : base(0x5)
        {
        }

        public List<Guid> Capabilites { get; } = new List<Guid>();

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (Guid cap in Capabilites)
            {
                data.AddRange(ByteConverter.GetBytes(cap));
            }

            return data;
        }

        public override int Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            while (index + 16 <= data.Count)
            {
                Capabilites.Add(ByteConverter.ToGuid(data.GetRange(index, 16)));

                index += 16;
            }

            return index;
        }

        public override int CalculateDataSize()
        {
            return Capabilites.Count*16;
        }
    }
}