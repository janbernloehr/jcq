// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIBuddyRecord.cs" company="Jan-Cornelius Molnar">
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
    public class SSIBuddyRecord : SSIRecord
    {
        private readonly TlvBuddyCommentField _Comment = new TlvBuddyCommentField();
        private readonly TlvLocalEmailAddress _LocalEmailAddress = new TlvLocalEmailAddress();
        private readonly TlvLocalScreenName _LocalScreenName = new TlvLocalScreenName();
        private readonly TlvLocalSmsNumber _LocalSmsNumber = new TlvLocalSmsNumber();
        private readonly TlvPersonalBuddyAlerts _PersonalAlerts = new TlvPersonalBuddyAlerts();

        public SSIBuddyRecord() : base(SSIItemType.BuddyRecord)
        {
        }

        public bool AwaitingAuthorization { get; set; }

        public TlvLocalScreenName LocalScreenName
        {
            get { return _LocalScreenName; }
        }

        public TlvLocalEmailAddress LocalEmailAddress
        {
            get { return _LocalEmailAddress; }
        }

        public TlvLocalSmsNumber LocalSmsNumber
        {
            get { return _LocalSmsNumber; }
        }

        public TlvBuddyCommentField Comment
        {
            get { return _Comment; }
        }

        public TlvPersonalBuddyAlerts PersonalAlerts
        {
            get { return _PersonalAlerts; }
        }

        public override int CalculateDataSize()
        {
            return 0;
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
                    case 0x66:
                        AwaitingAuthorization = true;
                        break;
                    case 0x131:
                        _LocalScreenName.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x137:
                        _LocalEmailAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x13a:
                        _LocalSmsNumber.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x13c:
                        _Comment.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x13d:
                        _PersonalAlerts.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }
        }
    }
}