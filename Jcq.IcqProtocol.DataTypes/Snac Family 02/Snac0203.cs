// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0203.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0203 : Snac
    {
        private readonly TlvMaxCapabilities _MaxCapabilities = new TlvMaxCapabilities();
        private readonly TlvProfileMaxLength _ProfileMaxLenght = new TlvProfileMaxLength();

        public Snac0203() : base(0x2, 0x3)
        {
        }

        public TlvProfileMaxLength TlvProfileMaxLength
        {
            get { return _ProfileMaxLenght; }
        }

        public TlvMaxCapabilities MaxCapabilities
        {
            get { return _MaxCapabilities; }
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        _ProfileMaxLenght.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x2:
                        _MaxCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }
    }
}