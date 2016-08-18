// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1004.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1004 : Snac
    {
        public Snac1004() : base(0x10, 0x4)
        {
        }

        public string Uin { get; set; }

        public List<byte> IconHash { get; } = new List<byte>();

        public override int CalculateDataSize()
        {
            return 1 + Uin.Length + 1 + 2 + 1 + 1 + IconHash.Count;
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.Add((byte) Uin.Length);
            data.AddRange(ByteConverter.GetBytes(Uin));
            data.Add(0x1);
            data.Add(0x0);
            data.Add(0x1);
            data.Add(0x1);
            data.Add((byte) IconHash.Count);
            data.AddRange(IconHash);

            //xx   byte   uin length 
            // xx ..   ascii   uin string 
            // 01   byte   unknown (command ?) 
            // 00 01   word   icon id (not sure) 
            // 01   byte   icon flags (bitmask, purpose unknown) 
            // 10   byte   md5 hash size (16) 
            // xx ..   array   requested icon md5 hash 


            return data;
        }
    }
}