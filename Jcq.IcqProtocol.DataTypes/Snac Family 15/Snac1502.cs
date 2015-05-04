// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1502.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1502 : Snac
    {
        private readonly TlvMetaRequestData _MetaData = new TlvMetaRequestData();

        public Snac1502() : base(0x15, 0x2)
        {
        }

        public TlvMetaRequestData MetaData
        {
            get { return _MetaData; }
        }

        public override int CalculateDataSize()
        {
            return _MetaData.CalculateTotalSize();
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();
            data.AddRange(_MetaData.Serialize());
            return data;
        }

        public override string ToString()
        {
            if (MetaData.MetaRequest is MetaShortUserInformationRequest)
            {
                return string.Format("Search: {0}", ((MetaShortUserInformationRequest) MetaData.MetaRequest).SearchUin);
            }
            return base.ToString();
        }
    }
}