// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0909.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0909 : Snac
    {
        private readonly TlvErrorDescriptionUrl _ErrorDescriptionUrl = new TlvErrorDescriptionUrl();
        private readonly TlvErrorSubCode _ErrorSubCode = new TlvErrorSubCode();

        public Snac0909() : base(0x9, 0x9)
        {
        }

        public int ErrorCode { get; set; }

        public TlvErrorSubCode ErrorSubCode
        {
            get { return _ErrorSubCode; }
        }

        public TlvErrorDescriptionUrl ErrorDescriptionUrl
        {
            get { return _ErrorDescriptionUrl; }
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

            var index = SizeFixPart;

            ErrorCode = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x8:
                        _ErrorSubCode.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x4:
                        _ErrorDescriptionUrl.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
        }
    }
}