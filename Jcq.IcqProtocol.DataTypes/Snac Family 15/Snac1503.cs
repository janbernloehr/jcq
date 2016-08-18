// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1503.cs" company="Jan-Cornelius Molnar">
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
using Jcq.Core;

namespace Jcq.IcqProtocol.DataTypes
{
    public class Snac1503 : Snac
    {
        public Snac1503() : base(0x15, 0x3)
        {
        }

        public TlvMetaResponseData MetaData { get; } = new TlvMetaResponseData();

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            base.Deserialize(descriptor, data);

            int index = SizeFixPart;

            while (index < data.Count)
            {
                TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

                Kernel.Logger.Log("Snac1503", TraceEventType.Information,
                    $"tlv {desc.TypeId:X2} found at index {index}; data size: {desc.DataSize} total lenght: {data.Count}");

                switch (desc.TypeId)
                {
                    case 0x1:
                        MetaData.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
            return index;
        }
    }
}