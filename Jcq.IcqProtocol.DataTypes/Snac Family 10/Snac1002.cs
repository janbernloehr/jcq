// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1002.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1002 : Snac
    {
        private readonly List<byte> _IconData = new List<byte>();

        public Snac1002() : base(0x10, 0x2)
        {
        }

        public int ReferenceNumber { get; set; }

        public List<byte> IconData
        {
            get { return _IconData; }
        }

        // xx xx\t \tword\t \treference number
        // xx xx\t \tword\t \ticon length
        // xx ..\t \tarray\t \ticon data (jpg, gif, bmp, etc...)

        public override int CalculateDataSize()
        {
            return 2 + 2 + IconData.Count;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) ReferenceNumber));
            data.AddRange(ByteConverter.GetBytes((ushort) IconData.Count));
            data.AddRange(IconData);

            return data;
        }
    }
}