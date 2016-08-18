// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1005.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1005 : Snac
    {
        public Snac1005() : base(0x10, 0x5)
        {
        }

        public string Uin { get; set; }

        public List<byte> IconHash { get; } = new List<byte>();

        public List<byte> IconData { get; } = new List<byte>();

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            base.Deserialize(descriptor, data);

            // xx   byte   uin length 
            // xx ..   ascii   uin string 

            int index = SizeFixPart;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            // 00 01   word   icon id (not sure) 
            // 01   byte   icon flags (bitmask, purpose unknown) 

            index += 3;

            // 10   byte   md5 hash size (16) 
            // xx ..   array   requested icon md5 hash 

            byte length = data[index];
            index += 1;

            IconHash.AddRange(data.GetRange(index, length));
            index += length;

            // xx xx   word   length of the icon 

            int iconLength = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            // xx ..   array   icon data (jfif - jpeg file interchange format) 

            IconData.AddRange(data.GetRange(index, iconLength));
            index += iconLength;

            TotalSize = index;
            return index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}