// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace Jcq.IcqProtocol.DataTypes
{
    public class UserInfo : ISerializable
    {
        public string Uin { get; set; }
        public int WarningLevel { get; set; }

        public TlvUserClass UserClass { get; } = new TlvUserClass();

        public TlvUserStatus UserStatus { get; } = new TlvUserStatus();

        public TlvDCInfo DCInfo { get; } = new TlvDCInfo();

        public TlvExternalIpAddress ExternalAddress { get; } = new TlvExternalIpAddress();

        public TlvSignonTime SignOnTime { get; } = new TlvSignonTime();

        public TlvMemberSince MemberSince { get; } = new TlvMemberSince();

        public TlvCapabilities UserCapabilities { get; } = new TlvCapabilities();

        public TlvOnlineTime OnlineTime { get; } = new TlvOnlineTime();

        public TlvUserIconIdAndHash UserIconIdAndHash { get; } = new TlvUserIconIdAndHash();

        public int DataSize { get; private set; }

        public int TotalSize => DataSize;

        public bool HasData { get; private set; }

        public virtual int CalculateDataSize()
        {
            return UserClass.CalculateDataSize() + DCInfo.CalculateDataSize() + ExternalAddress.CalculateDataSize() +
                   UserStatus.CalculateDataSize() + UserCapabilities.CalculateDataSize() +
                   OnlineTime.CalculateDataSize() +
                   SignOnTime.CalculateDataSize() + MemberSince.CalculateDataSize() +
                   UserIconIdAndHash.CalculateDataSize();
        }

        public int CalculateTotalSize()
        {
            return CalculateDataSize();
        }

        public virtual int Deserialize(List<byte> data)
        {
            int index = 0;

            if (data[index] == 0x0 & data[index + 1] == 0x6)
                index += 8;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            int innerTlvIndex = 0;

            int innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (innerTlvIndex < innerTlvCount)
            {
                TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xc:
                        DCInfo.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xa:
                        ExternalAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xd:
                        UserCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        OnlineTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        MemberSince.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x1d:
                        UserIconIdAndHash.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
                innerTlvIndex += 1;
            }

            DataSize = index;

            HasData = true;

            return index;
        }

        public virtual List<byte> Serialize()
        {
            var data = new List<byte>(CalculateTotalSize());

            data.AddRange(UserClass.Serialize());
            data.AddRange(DCInfo.Serialize());
            data.AddRange(ExternalAddress.Serialize());
            data.AddRange(UserStatus.Serialize());
            data.AddRange(UserCapabilities.Serialize());
            data.AddRange(OnlineTime.Serialize());
            data.AddRange(SignOnTime.Serialize());
            data.AddRange(MemberSince.Serialize());
            data.AddRange(UserIconIdAndHash.Serialize());

            return data;
        }

        public int Deserialize(int offset, List<byte> data)
        {
            Deserialize(data.GetRange(offset, data.Count - offset));

            return DataSize;
        }
    }
}