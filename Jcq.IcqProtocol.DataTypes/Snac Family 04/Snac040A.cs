// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac040A.cs" company="Jan-Cornelius Molnar">
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