// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaSearchByTlvRequest.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class MetaSearchByTlvRequest : MetaInformationRequest
    {
        private readonly List<Tlv> _searchTlvs = new List<Tlv>();

        public MetaSearchByTlvRequest(MetaRequestSubType subtype) : base(subtype)
        {
        }

        public List<Tlv> SearchTlvs
        {
            get { return _searchTlvs; }
        }

        public override int CalculateDataSize()
        {
            return 2 + _searchTlvs.Sum(x => x.CalculateTotalSize());
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var x in _searchTlvs)
            {
                data.AddRange(x.Serialize());
            }

            return data;
        }
    }
}