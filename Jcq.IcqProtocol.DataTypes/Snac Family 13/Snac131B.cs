// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac131B.cs" company="Jan-Cornelius Molnar">
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
    public class Snac131B : Snac
    {
        public Snac131B() : base(0x13, 0x1b)
        {
        }

        public string Uin { get; set; }
        public string Message { get; set; }
        public bool AuthorizationAccepted { get; set; }

        public override int CalculateDataSize()
        {
            return 1 + Uin.Length + 1 + 2 + Message.Length + 2;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            if (data[index] == 1)
            {
                AuthorizationAccepted = true;
            }

            index += 1;

            Message = ByteConverter.ToStringFromUInt16Index(index, data);
            index += 2 + Message.Length;

            index += 2;

            SetTotalSize(index);
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}