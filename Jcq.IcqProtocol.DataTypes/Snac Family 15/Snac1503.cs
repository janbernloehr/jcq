// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1503.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using JCsTools.Core;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac1503 : Snac
    {
        private readonly TlvMetaResponseData _MetaData = new TlvMetaResponseData();

        public Snac1503() : base(0x15, 0x3)
        {
        }

        public TlvMetaResponseData MetaData
        {
            get { return _MetaData; }
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

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                Kernel.Logger.Log("Snac1503", TraceEventType.Information,
                    "tlv {0:X2} found at index {1}; data size: {2} total lenght: {3}", desc.TypeId, index, desc.DataSize,
                    data.Count);

                switch (desc.TypeId)
                {
                    case 0x1:
                        _MetaData.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            SetTotalSize(index);
        }
    }
}