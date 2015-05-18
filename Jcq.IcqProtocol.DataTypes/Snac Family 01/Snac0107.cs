// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0107.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0107 : Snac
    {
        private readonly List<RateClass> _rateClasses = new List<RateClass>();
        private readonly List<RateGroup> _rateGroups = new List<RateGroup>();

        public Snac0107() : base(0x1, 0x7)
        {
        }

        public List<RateClass> RateClasses
        {
            get { return _rateClasses; }
        }

        public List<RateGroup> RateGroups
        {
            get { return _rateGroups; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            int classCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            var classIndex = 0;

            while (classIndex < classCount)
            {
                var cls = new RateClass();
                cls.Deserialize(data.GetRange(index, data.Count - index));

                _rateClasses.Add(cls);

                index += cls.TotalSize;
                classIndex += 1;
            }

            while (index + 4 <= data.Count)
            {
                var rateGroup = new RateGroup();
                rateGroup.Deserialize(data.GetRange(index, data.Count - index));

                _rateGroups.Add(rateGroup);

                index += rateGroup.TotalSize;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + _rateClasses.Sum(x => x.CalculateTotalSize()) + _rateGroups.Sum(x => x.CalculateTotalSize());
        }
    }
}