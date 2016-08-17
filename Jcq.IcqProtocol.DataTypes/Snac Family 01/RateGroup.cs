// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RateGroup.cs" company="Jan-Cornelius Molnar">
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
    public class RateGroup : ISerializable
    {
        public int GroupId { get; set; }

        public List<FamilySubtypePair> ServiceFamilyIdSubTypeIdPairs { get; } = new List<FamilySubtypePair>();

        public int TotalSize
        {
            get { return 4 + DataSize; }
        }

        public int DataSize { get; private set; }

        public virtual void Deserialize(List<byte> data)
        {
            int pairCount;
            int pairIndex = 0;
            int index;

            GroupId = ByteConverter.ToUInt16(data.GetRange(0, 2));
            pairCount = ByteConverter.ToUInt16(data.GetRange(2, 2));

            index = 4;

            while (pairIndex < pairCount)
            {
                int familyId = ByteConverter.ToUInt16(data.GetRange(index, 2));
                int subtypeId = ByteConverter.ToUInt16(data.GetRange(index + 2, 2));

                ServiceFamilyIdSubTypeIdPairs.Add(new FamilySubtypePair(familyId, subtypeId));

                pairIndex += 1;
                index += 4;
            }

            DataSize = index - 4;
            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public bool HasData { get; private set; }

        public virtual int CalculateDataSize()
        {
            return ServiceFamilyIdSubTypeIdPairs.Count*4;
        }

        public int CalculateTotalSize()
        {
            return 4 + CalculateDataSize();
        }
    }
}