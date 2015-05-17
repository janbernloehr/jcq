// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac040A.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.DataTypes
{
    public class Snac040A : Snac
    {
        private readonly List<MissedMessageInfo> _MissedMessageInfos = new List<MissedMessageInfo>();

        public Snac040A() : base(0x4, 0xa)
        {
        }

        public List<MissedMessageInfo> MissedMessageInfos
        {
            get { return _MissedMessageInfos; }
        }

        public override int CalculateDataSize()
        {
            return _MissedMessageInfos.Sum(x => x.CalculateTotalSize());
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index;

            index = SizeFixPart;

            while (index < data.Count)
            {
                MissedMessageInfo info;

                info = new MissedMessageInfo();
                index += info.Deserialize(index, data);

                _MissedMessageInfos.Add(info);
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}