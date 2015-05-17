// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0402.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac0402 : Snac
    {
        public Snac0402() : base(0x4, 0x2)
        {
        }

        public int Channel { get; set; }
        public int MessageFlags { get; set; }
        public int MaxMessageSnacSize { get; set; }
        public int MaxSenderWarningLevel { get; set; }
        public int MaxReceiverWarningLevel { get; set; }
        public int MinimumMessageInterval { get; set; }

        public override int CalculateDataSize()
        {
            return 2 + 4 + 2 + 2 + 2 + 2 + 2;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) Channel));
            data.AddRange(ByteConverter.GetBytes((uint) MessageFlags));
            data.AddRange(ByteConverter.GetBytes((ushort) MaxMessageSnacSize));
            data.AddRange(ByteConverter.GetBytes((ushort) MaxSenderWarningLevel));
            data.AddRange(ByteConverter.GetBytes((ushort) MaxReceiverWarningLevel));
            data.AddRange(ByteConverter.GetBytes((ushort) MinimumMessageInterval));
            data.AddRange(ByteConverter.GetBytes(0));

            return data;
        }
    }
}