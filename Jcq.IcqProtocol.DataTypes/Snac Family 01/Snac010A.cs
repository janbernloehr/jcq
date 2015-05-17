// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac010A.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac010A : Snac
    {
        private readonly List<RateClass> _rateClasses = new List<RateClass>();

        public Snac010A() : base(0x1, 0xa)
        {
        }

        public MessageCode MessageCode { get; set; }

        public List<RateClass> RateClasses
        {
            get { return _rateClasses; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;
            //TlvDescriptor desc;

            //index += 2

            //desc = TlvDescriptor.GetDescriptor(index, data)
            //index += desc.TotalSize

            //desc = TlvDescriptor.GetDescriptor(index, data)
            //index += desc.TotalSize

            //_MessageCode = DirectCast(Convert.ToInt32(ByteConverter.ToUInt16(data.GetRange(index, 2))), MessageCode)
            //index += 2
            //_RateClass.Deserialize(data.GetRange(index, data.Count - index))
            //index += _RateClass.TotalSize

            //Dim verbose As String = String.Format("{0}, {1}", MessageCode.ToString, _RateClass.ToString)

            //Core.Kernel.Logger.Log("ClientRate", TraceEventType.Verbose, verbose)


            // We do not know the precise structure of the first part...
            int length = ByteConverter.ToUInt16(data.GetRange(index, 2));

            index += length + 2;

            MessageCode = (MessageCode) Convert.ToInt32(ByteConverter.ToUInt16(data.GetRange(index, 2)));

            index += 2;

            while (index < data.Count - 1)
            {
                var rc = new RateClass();

                rc.Deserialize(data.GetRange(index, data.Count - index));

                index += rc.TotalSize;

                _rateClasses.Add(rc);
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + _rateClasses.Sum(r => r.TotalSize);
        }
    }
}