// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0103.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0103 : Snac
    {
        private readonly List<int> _ServerSupportedFamilyIds = new List<int>();

        public Snac0103() : base(0x1, 0x3)
        {
        }

        public List<int> ServerSupportedFamilyIds
        {
            get { return _ServerSupportedFamilyIds; }
        }

        public override int CalculateDataSize()
        {
            return _ServerSupportedFamilyIds.Count*2;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                _ServerSupportedFamilyIds.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

                index += 2;
            }

            SetTotalSize(index);
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var familyId in _ServerSupportedFamilyIds)
            {
                data.AddRange(ByteConverter.GetBytes((ushort) familyId));
            }

            return data;
        }
    }
}