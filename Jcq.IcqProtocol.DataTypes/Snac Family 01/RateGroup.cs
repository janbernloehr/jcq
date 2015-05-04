// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RateGroup.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class RateGroup : ISerializable
    {
        private readonly List<FamilySubtypePair> _ServiceFamilyIdSubTypeIdPairs = new List<FamilySubtypePair>();
        public int GroupId { get; set; }

        public List<FamilySubtypePair> ServiceFamilyIdSubTypeIdPairs
        {
            get { return _ServiceFamilyIdSubTypeIdPairs; }
        }

        public int TotalSize
        {
            get { return 4 + DataSize; }
        }

        public int DataSize { get; private set; }

        public virtual void Deserialize(List<byte> data)
        {
            int pairCount;
            var pairIndex = 0;
            int index;

            GroupId = ByteConverter.ToUInt16(data.GetRange(0, 2));
            pairCount = ByteConverter.ToUInt16(data.GetRange(2, 2));

            index = 4;

            while (pairIndex < pairCount)
            {
                int familyId = ByteConverter.ToUInt16(data.GetRange(index, 2));
                int subtypeId = ByteConverter.ToUInt16(data.GetRange(index + 2, 2));

                _ServiceFamilyIdSubTypeIdPairs.Add(new FamilySubtypePair(familyId, subtypeId));

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
            return _ServiceFamilyIdSubTypeIdPairs.Count*4;
        }

        public int CalculateTotalSize()
        {
            return 4 + CalculateDataSize();
        }
    }
}