// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="Jan-Cornelius Molnar">
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
    public class UserInfo : ISerializable
    {
        private readonly TlvDCInfo _DCInfo = new TlvDCInfo();
        private readonly TlvExternalIpAddress _ExternalAddress = new TlvExternalIpAddress();
        private readonly TlvMemberSince _MemberSince = new TlvMemberSince();
        private readonly TlvOnlineTime _OnlineTime = new TlvOnlineTime();
        private readonly TlvSignonTime _SignOnTime = new TlvSignonTime();
        private readonly TlvCapabilities _UserCapabilities = new TlvCapabilities();
        private readonly TlvUserClass _UserClass = new TlvUserClass();
        private readonly TlvUserIconIdAndHash _UserIconIdAndHash = new TlvUserIconIdAndHash();
        private readonly TlvUserStatus _UserStatus = new TlvUserStatus();
        public string Uin { get; set; }
        public int WarningLevel { get; set; }

        public TlvUserClass UserClass
        {
            get { return _UserClass; }
        }

        public TlvUserStatus UserStatus
        {
            get { return _UserStatus; }
        }

        public TlvDCInfo DCInfo
        {
            get { return _DCInfo; }
        }

        public TlvExternalIpAddress ExternalAddress
        {
            get { return _ExternalAddress; }
        }

        public TlvSignonTime SignOnTime
        {
            get { return _SignOnTime; }
        }

        public TlvMemberSince MemberSince
        {
            get { return _MemberSince; }
        }

        public TlvCapabilities UserCapabilities
        {
            get { return _UserCapabilities; }
        }

        public TlvOnlineTime OnlineTime
        {
            get { return _OnlineTime; }
        }

        public TlvUserIconIdAndHash UserIconIdAndHash
        {
            get { return _UserIconIdAndHash; }
        }

        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return DataSize; }
        }

        public bool HasData { get; private set; }

        public virtual int CalculateDataSize()
        {
            return _UserClass.CalculateDataSize() + _DCInfo.CalculateDataSize() + _ExternalAddress.CalculateDataSize() +
                   _UserStatus.CalculateDataSize() + _UserCapabilities.CalculateDataSize() +
                   _OnlineTime.CalculateDataSize() +
                   _SignOnTime.CalculateDataSize() + _MemberSince.CalculateDataSize() +
                   _UserIconIdAndHash.CalculateDataSize();
        }

        public int CalculateTotalSize()
        {
            return CalculateDataSize();
        }

        public virtual void Deserialize(List<byte> data)
        {
            var index = 0;

            if (data[index] == 0x0 & data[index + 1] == 0x6)
                index += 8;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            int innerTlvCount;
            var innerTlvIndex = 0;

            innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (innerTlvIndex < innerTlvCount)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        _UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xc:
                        _DCInfo.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xa:
                        _ExternalAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xd:
                        _UserCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        _OnlineTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        _MemberSince.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x1d:
                        _UserIconIdAndHash.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
                innerTlvIndex += 1;
            }

            DataSize = index;

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            List<byte> data;

            data = new List<byte>();

            data.AddRange(_UserClass.Serialize());
            data.AddRange(_DCInfo.Serialize());
            data.AddRange(_ExternalAddress.Serialize());
            data.AddRange(_UserStatus.Serialize());
            data.AddRange(_UserCapabilities.Serialize());
            data.AddRange(_OnlineTime.Serialize());
            data.AddRange(_SignOnTime.Serialize());
            data.AddRange(_MemberSince.Serialize());
            data.AddRange(_UserIconIdAndHash.Serialize());

            return data;
        }

        public int Deserialize(int offset, List<byte> data)
        {
            Deserialize(data.GetRange(offset, data.Count - offset));

            return DataSize;
        }
    }
}