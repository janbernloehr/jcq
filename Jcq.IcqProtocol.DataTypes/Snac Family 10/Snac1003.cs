// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1003.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1003 : Snac
    {
        private readonly List<byte> _IconHash = new List<byte>(16);

        public Snac1003() : base(0x10, 0x3)
        {
        }

        public List<byte> IconHash
        {
            get { return _IconHash; }
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            //00 00\t \tword\t \tunknown field(s)
            //01 01\t \tword\t \tunknown field(s)
            //10\t \tbyte\t \tsize of the icon md5 checksum
            //xx ..\t \tarray\t \ticon md5 checksum

            var index = SizeFixPart;

            byte hashLength;

            index += 2;
            index += 2;

            hashLength = data[index];
            index += 1;

            _IconHash.AddRange(data.GetRange(index, hashLength));
            index += hashLength;

            TotalSize = index;
        }
    }
}