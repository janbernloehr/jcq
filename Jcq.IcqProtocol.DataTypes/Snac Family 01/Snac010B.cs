// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac010B.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac010B : Snac
    {
        private readonly List<int> _RateGroupIds = new List<int>();

        public Snac010B() : base(0x1, 0xb)
        {
        }

        public List<int> RateGroupIds
        {
            get { return _RateGroupIds; }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var x in _RateGroupIds)
            {
                data.AddRange(ByteConverter.GetBytes((ushort) x));
            }

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index + 2 <= data.Count)
            {
                _RateGroupIds.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

                index += 2;
            }

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            return RateGroupIds.Count*2;
        }
    }
}