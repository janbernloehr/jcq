// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0105.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0105 : Snac
    {
        private readonly TlvAuthorizationCookie _AuthorizationCookie = new TlvAuthorizationCookie();
        private readonly TlvServerAddress _ServerAddress = new TlvServerAddress();
        private readonly TlvServiceFamilyId _ServiceFamily = new TlvServiceFamilyId();

        public Snac0105() : base(0x1, 0x5)
        {
        }

        public TlvServiceFamilyId ServiceFamily
        {
            get { return _ServiceFamily; }
        }

        public TlvServerAddress ServerAddress
        {
            get { return _ServerAddress; }
        }

        public TlvAuthorizationCookie AuthorizationCookie
        {
            get { return _AuthorizationCookie; }
        }

        public override int CalculateDataSize()
        {
            return _ServiceFamily.CalculateTotalSize() + _ServerAddress.CalculateTotalSize() +
                   _AuthorizationCookie.CalculateTotalSize();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            index += 8;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0xd:
                        _ServiceFamily.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        _ServerAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        _AuthorizationCookie.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            SetTotalSize(index);
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(_ServiceFamily.Serialize());
            data.AddRange(_ServerAddress.Serialize());
            data.AddRange(_AuthorizationCookie.Serialize());

            return data;
        }
    }
}