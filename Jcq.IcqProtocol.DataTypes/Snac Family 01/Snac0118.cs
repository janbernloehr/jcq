// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0118.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0118 : Snac
    {
        private readonly List<FamilyVersionPair> _FamilyNameVersionPairs = new List<FamilyVersionPair>();

        public Snac0118() : base(0x1, 0x18)
        {
        }

        public List<FamilyVersionPair> FamilyNameVersionPairs
        {
            get { return _FamilyNameVersionPairs; }
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index + 4 < data.Count)
            {
                int number;
                int version;

                number = ByteConverter.ToUInt16(data.GetRange(index, 2));
                index += 2;
                version = ByteConverter.ToUInt16(data.GetRange(index, 2));
                index += 2;

                _FamilyNameVersionPairs.Add(new FamilyVersionPair(number, version));
            }

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            return _FamilyNameVersionPairs.Count*4;
        }
    }
}