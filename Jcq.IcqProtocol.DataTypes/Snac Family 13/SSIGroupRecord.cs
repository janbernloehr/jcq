// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIGroupRecord.cs" company="Jan-Cornelius Molnar">
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
    public class SSIGroupRecord : SSIRecord
    {
        private readonly TlvSSIInnerItems _InnerItems = new TlvSSIInnerItems();

        public SSIGroupRecord() : base(SSIItemType.GroupRecord)
        {
        }

        public bool IsMasterGroup { get; set; }

        public TlvSSIInnerItems InnerItems
        {
            get { return _InnerItems; }
        }

        public override int CalculateDataSize()
        {
            return _InnerItems.TotalSize;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            if (GroupId == 0)
            {
                IsMasterGroup = true;
            }

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0xc8:
                        _InnerItems.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(_InnerItems.Serialize());

            return data;
        }
    }
}