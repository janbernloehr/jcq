// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0107.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac0107 : Snac
    {
        private readonly List<RateClass> _RateClasses = new List<RateClass>();
        private readonly List<RateGroup> _RateGroups = new List<RateGroup>();

        public Snac0107() : base(0x1, 0x7)
        {
        }

        public List<RateClass> RateClasses
        {
            get { return _RateClasses; }
        }

        public List<RateGroup> RateGroups
        {
            get { return _RateGroups; }
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
                RateClass cls;
                cls = new RateClass();
                cls.Deserialize(data.GetRange(index, data.Count - index));

                _RateClasses.Add(cls);

                index += cls.TotalSize;
                classIndex += 1;
            }

            while (index + 4 <= data.Count)
            {
                RateGroup @group;
                @group = new RateGroup();
                @group.Deserialize(data.GetRange(index, data.Count - index));

                _RateGroups.Add(@group);

                index += @group.TotalSize;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + _RateClasses.Sum(x => x.CalculateTotalSize()) + _RateGroups.Sum(x => x.CalculateTotalSize());
        }
    }
}