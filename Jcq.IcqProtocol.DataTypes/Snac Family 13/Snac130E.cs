// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac130E.cs" company="Jan-Cornelius Molnar">
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
    public class Snac130E : Snac
    {
        private List<SSIActionResultCode> _ActionResultCodes = new List<SSIActionResultCode>();

        public Snac130E() : base(0x13, 0xe)
        {
        }

        public List<SSIActionResultCode> ActionResultCodes
        {
            get { return _ActionResultCodes; }
            set { _ActionResultCodes = value; }
        }

        public override int CalculateDataSize()
        {
            return _ActionResultCodes.Count*2;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            if (data.Count > index + 2)
            {
                if (data[index] == 0 & data[index + 1] == 6)
                {
                    index += 2;
                }
            }

            if (data.Count > index + 2)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);
                index += desc.TotalSize;
            }

            while (index < data.Count)
            {
                SSIActionResultCode code;

                code = (SSIActionResultCode) ByteConverter.ToUInt16(data.GetRange(index, 2));
                _ActionResultCodes.Add(code);

                index += 2;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}