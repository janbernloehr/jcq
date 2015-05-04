// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvUserStatus.cs" company="Jan-Cornelius Molnar">
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
    public class TlvUserStatus : Tlv
    {
        public TlvUserStatus() : base(0x6)
        {
        }

        public UserFlag NewProperty { get; set; }
        public UserStatus UserStatus { get; set; }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) NewProperty));
            data.AddRange(ByteConverter.GetBytes((ushort) UserStatus));

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            NewProperty = (UserFlag) ByteConverter.ToUInt16(data.GetRange(index, 2));
            UserStatus = (UserStatus) ByteConverter.ToUInt16(data.GetRange(index + 2, 2));
        }

        public override int CalculateDataSize()
        {
            return 4;
        }
    }

    [Flags]
    public enum UserFlag
    {
        StatusShowIpFlag = 0x2,
        UserBirthdayFlag = 0x8,
        UserActiveWebfrontFlag = 0x20,
        DirectConnectionNotSupported = 0x100,
        DirectConnectionUponAuthorization = 0x1000,
        OnlyWithContactUsers = 0x2000
    }

    [Flags]
    public enum UserStatus
    {
        Online = 0x0,
        Away = 0x1,
        DoNotDisturb = 0x2,
        NotAvailable = 0x4,
        Occupied = 0x10,
        FreeForChat = 0x20,
        Invisible = 0x100,
        Offline
    }
}