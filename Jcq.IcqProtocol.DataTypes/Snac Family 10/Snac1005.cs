// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1005.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1005 : Snac
    {
        private readonly List<byte> _IconData = new List<byte>();
        private readonly List<byte> _IconHash = new List<byte>();

        public Snac1005() : base(0x10, 0x5)
        {
        }

        public string Uin { get; set; }

        public List<byte> IconHash
        {
            get { return _IconHash; }
        }

        public List<byte> IconData
        {
            get { return _IconData; }
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            // xx   byte   uin length 
            // xx ..   ascii   uin string 

            var index = SizeFixPart;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            // 00 01   word   icon id (not sure) 
            // 01   byte   icon flags (bitmask, purpose unknown) 

            index += 3;

            // 10   byte   md5 hash size (16) 
            // xx ..   array   requested icon md5 hash 

            var length = data[index];
            index += 1;

            _IconHash.AddRange(data.GetRange(index, length));
            index += length;

            // xx xx   word   length of the icon 

            int iconLength = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            // xx ..   array   icon data (jfif - jpeg file interchange format) 

            _IconData.AddRange(data.GetRange(index, iconLength));
            index += iconLength;

            SetTotalSize(index);
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}