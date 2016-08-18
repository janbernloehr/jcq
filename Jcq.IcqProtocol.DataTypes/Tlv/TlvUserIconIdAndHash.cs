// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvUserIconIdAndHash.cs" company="Jan-Cornelius Molnar">
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
    public class TlvUserIconIdAndHash : Tlv
    {
        public TlvUserIconIdAndHash() : base(0x1d)
        {
        }

        public int IconId { get; set; }
        public byte IconFlags { get; set; }
        public byte IconHashLenght { get; set; }

        public List<byte> IconMD5Hash { get; } = new List<byte>();

        public override int CalculateDataSize()
        {
            return 2 + 1 + 1 + IconMD5Hash.Count;
        }

        public override int Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            IconId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            IconFlags = data[index];
            index += 1;

            IconHashLenght = data[index];
            index += 1;

            IconMD5Hash.AddRange(data.GetRange(index, IconHashLenght));
            index += IconMD5Hash.Count;

            return index;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) IconId));
            data.Add(IconFlags);
            data.Add(IconHashLenght);
            data.AddRange(IconMD5Hash);

            return data;
        }
    }
}