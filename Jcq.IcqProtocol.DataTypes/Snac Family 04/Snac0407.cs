// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0407.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0407 : Snac
    {
        private readonly TlvAccountCreationTime _AccountCreationTime = new TlvAccountCreationTime();
        private readonly TlvClientIdleTime _ClientIdleTime = new TlvClientIdleTime();
        private readonly TlvMessageData _MessageData = new TlvMessageData();
        private readonly TlvUserClass _UserClass = new TlvUserClass();
        private readonly TlvUserStatus _UserStatus = new TlvUserStatus();

        public Snac0407() : base(0x4, 0x7)
        {
        }

        public decimal CookieID { get; set; }
        public MessageChannel Channel { get; set; }
        public string ScreenName { get; set; }
        public int SenderWarningLevel { get; private set; }
        public bool AutoResponseFlag { get; private set; }

        public TlvUserClass UserClass
        {
            get { return _UserClass; }
        }

        public TlvUserStatus UserStatus
        {
            get { return _UserStatus; }
        }

        public TlvClientIdleTime ClientIdleTime
        {
            get { return _ClientIdleTime; }
        }

        public TlvAccountCreationTime AccountCreationTime
        {
            get { return _AccountCreationTime; }
        }

        public TlvMessageData MessageData
        {
            get { return _MessageData; }
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

            TlvDescriptor desc;

            CookieID = ByteConverter.ToUInt64(data.GetRange(index, 8));
            index += 8;

            Channel = (MessageChannel) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            ScreenName = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + ScreenName.Length;

            SenderWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            int innerTlvCount;
            var innerTlvIndex = 0;

            innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (innerTlvIndex < innerTlvCount)
            {
                desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        _UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        _ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        _AccountCreationTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x4:
                        AutoResponseFlag = true;
                        break;
                }

                index += desc.TotalSize;
                innerTlvIndex += 1;
            }

            desc = TlvDescriptor.GetDescriptor(index, data);

            _MessageData.Deserialize(data.GetRange(index, desc.TotalSize));
            index += _MessageData.TotalSize;

            while (index + 4 <= data.Count)
            {
                desc = TlvDescriptor.GetDescriptor(index, data);

                index += desc.TotalSize;
            }

            SetTotalSize(index);
        }
    }
}