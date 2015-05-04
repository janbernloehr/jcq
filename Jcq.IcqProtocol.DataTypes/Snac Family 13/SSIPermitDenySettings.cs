// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIPermitDenySettings.cs" company="Jan-Cornelius Molnar">
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
    public class SSIPermitDenySettings : SSIRecord
    {
        private readonly TlvAllowOtherToSee _AllowOthersToSee = new TlvAllowOtherToSee();
        private readonly TlvPrivacySetting _PrivacySetting = new TlvPrivacySetting();

        public SSIPermitDenySettings() : base(SSIItemType.PermitDenySettings)
        {
        }

        public TlvPrivacySetting PrivacySetting
        {
            get { return _PrivacySetting; }
        }

        public TlvAllowOtherToSee AllowOthersToSee
        {
            get { return _AllowOthersToSee; }
        }

        public override int CalculateDataSize()
        {
            return _PrivacySetting.TotalSize + _AllowOthersToSee.TotalSize;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0xca:
                        _PrivacySetting.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xcc:
                        _AllowOthersToSee.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}