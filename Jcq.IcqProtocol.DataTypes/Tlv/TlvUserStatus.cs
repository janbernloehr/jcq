// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvUserStatus.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Jcq.IcqProtocol.DataTypes
{
    public class TlvUserStatus : Tlv
    {
        public TlvUserStatus() : base(0x6)
        {
        }

        public UserFlag UserFlag { get; set; }
        public UserStatus UserStatus { get; set; }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ushort) UserFlag));
            data.AddRange(ByteConverter.GetBytes((ushort) UserStatus));

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            UserFlag = (UserFlag) ByteConverter.ToUInt16(data.GetRange(index, 2));
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