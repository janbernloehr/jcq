// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0101.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0101 : Snac
    {
        private readonly TlvErrorSubCode _SubError = new TlvErrorSubCode();

        public Snac0101() : base(0x1, 0x1)
        {
        }

        public ErrorCode ErrorCode { get; set; }

        public TlvErrorSubCode SubError
        {
            get { return _SubError; }
        }

        public override List<byte> Serialize()
        {
            List<byte> data;

            data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((uint) ErrorCode));
            data.AddRange(_SubError.Serialize());

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            ErrorCode = (ErrorCode) (short) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                if (desc.TypeId == 0x8)
                {
                    _SubError.Deserialize(data.GetRange(index, desc.TotalSize));
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            return 2 + 6;
        }
    }
}