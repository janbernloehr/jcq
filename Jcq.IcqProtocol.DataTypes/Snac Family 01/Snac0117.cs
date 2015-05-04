// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0117.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0117 : Snac
    {
        private readonly List<FamilyVersionPair> _FamilyNameVersionPairs = new List<FamilyVersionPair>();

        public Snac0117() : base(0x1, 0x17)
        {
        }

        public List<FamilyVersionPair> FamilyNameVersionPairs
        {
            get { return _FamilyNameVersionPairs; }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var pair in _FamilyNameVersionPairs)
            {
                data.AddRange(ByteConverter.GetBytes((ushort) pair.FamilyNumber));
                data.AddRange(ByteConverter.GetBytes((ushort) pair.FamilyVersion));
            }

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return _FamilyNameVersionPairs.Count*4;
        }
    }
}