// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvAuthorizationCookie.cs" company="Jan-Cornelius Molnar">
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
    public class TlvAuthorizationCookie : Tlv
    {
        private readonly List<byte> _AuthorizationCookie = new List<byte>();

        public TlvAuthorizationCookie() : base(0x6)
        {
        }

        public List<byte> AuthorizationCookie
        {
            get { return _AuthorizationCookie; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            _AuthorizationCookie.AddRange(data.GetRange(index, DataSize));
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(_AuthorizationCookie);

            return data;
        }

        public override int CalculateDataSize()
        {
            return _AuthorizationCookie.Count;
        }
    }
}