// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0204.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0204 : Snac
    {
        private readonly TlvCapabilities _Capabilities = new TlvCapabilities();
        private readonly TlvMimeType _MimeType = new TlvMimeType();

        public Snac0204() : base(0x2, 0x4)
        {
        }

        public TlvMimeType MimeType
        {
            get { return _MimeType; }
        }

        public TlvCapabilities Capabilities
        {
            get { return _Capabilities; }
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            if (_MimeType.CalculateDataSize() > 0)
            {
                data.AddRange(_MimeType.Serialize());
            }

            data.AddRange(_Capabilities.Serialize());

            return data;
        }

        public override int CalculateDataSize()
        {
            if (_MimeType.CalculateDataSize() == 0)
            {
                return _Capabilities.CalculateTotalSize();
            }
            return _MimeType.CalculateTotalSize() + _Capabilities.CalculateTotalSize();
        }
    }
}