// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvCapabilities.cs" company="Jan-Cornelius Molnar">
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
    public class TlvCapabilities : Tlv
    {
        private readonly List<Guid> _Capabilites = new List<Guid>();

        public TlvCapabilities() : base(0x5)
        {
        }

        public List<Guid> Capabilites
        {
            get { return _Capabilites; }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var cap in _Capabilites)
            {
                data.AddRange(ByteConverter.GetBytes(cap));
            }

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index + 16 <= data.Count)
            {
                _Capabilites.Add(ByteConverter.ToGuid(data.GetRange(index, 16)));

                index += 16;
            }
        }

        public override int CalculateDataSize()
        {
            return _Capabilites.Count*16;
        }
    }
}