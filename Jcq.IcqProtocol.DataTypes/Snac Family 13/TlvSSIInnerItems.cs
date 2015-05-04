// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvSSIInnerItems.cs" company="Jan-Cornelius Molnar">
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
    public class TlvSSIInnerItems : Tlv
    {
        private readonly List<int> _InnerItems = new List<int>();

        public TlvSSIInnerItems() : base(0xc8)
        {
        }

        public List<int> InnerItems
        {
            get { return _InnerItems; }
        }

        public override int CalculateDataSize()
        {
            return _InnerItems.Count*2;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                _InnerItems.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

                index += 2;
            }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var x in _InnerItems)
            {
                data.AddRange(ByteConverter.GetBytes((ushort) x));
            }

            return data;
        }
    }
}