// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0204.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0204 : Snac
    {
        private readonly TlvCapabilities _Capabilities = new TlvCapabilities();
        private readonly TlvMimeType _MimeType = new TlvMimeType();

        public Snac0204() : base(0x2, 0x4)
        {
        }

        public TlvMimeType MimeType
        {
            get { return _MimeType; }
        }

        public TlvCapabilities Capabilities
        {
            get { return _Capabilities; }
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            if (_MimeType.CalculateDataSize() > 0)
            {
                data.AddRange(_MimeType.Serialize());
            }

            data.AddRange(_Capabilities.Serialize());

            return data;
        }

        public override int CalculateDataSize()
        {
            if (_MimeType.CalculateDataSize() == 0)
            {
                return _Capabilities.CalculateTotalSize();
            }
            return _MimeType.CalculateTotalSize() + _Capabilities.CalculateTotalSize();
        }
    }
}