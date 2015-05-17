// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac130E.cs" company="Jan-Cornelius Molnar">
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
    public class Snac130E : Snac
    {
        private List<SSIActionResultCode> _ActionResultCodes = new List<SSIActionResultCode>();

        public Snac130E() : base(0x13, 0xe)
        {
        }

        public List<SSIActionResultCode> ActionResultCodes
        {
            get { return _ActionResultCodes; }
            set { _ActionResultCodes = value; }
        }

        public override int CalculateDataSize()
        {
            return _ActionResultCodes.Count*2;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            if (data.Count > index + 2)
            {
                if (data[index] == 0 & data[index + 1] == 6)
                {
                    index += 2;
                }
            }

            if (data.Count > index + 2)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);
                index += desc.TotalSize;
            }

            while (index < data.Count)
            {
                SSIActionResultCode code;

                code = (SSIActionResultCode) ByteConverter.ToUInt16(data.GetRange(index, 2));
                _ActionResultCodes.Add(code);

                index += 2;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}