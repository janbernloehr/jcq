// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissedMessageInfo.cs" company="Jan-Cornelius Molnar">
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
    public class MissedMessageInfo : ISerializable
    {
        private readonly TlvOnlineTime _OnlineTime = new TlvOnlineTime();
        private readonly TlvSignonTime _SignOnTime = new TlvSignonTime();
        private readonly TlvUserClass _UserClass = new TlvUserClass();
        private readonly TlvUserStatus _UserStatus = new TlvUserStatus();
        public MessageChannel Channel { get; set; }
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

        public TlvOnlineTime OnlineTime
        {
            get { return _OnlineTime; }
        }

        public TlvSignonTime SignOnTime
        {
            get { return _SignOnTime; }
        }

        public MissedMessageReason MissedReason { get; set; }
        public int MissedMessageCount { get; set; }

        public int TotalSize
        {
            get { return DataSize; }
        }

        public int DataSize { get; private set; }

        public virtual int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public int CalculateTotalSize()
        {
            throw new NotImplementedException();
        }

        public bool HasData { get; private set; }

        public void Deserialize(List<byte> data)
        {
            var index = 0;

            Channel = (MessageChannel) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

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
                    case 0x6:
                        _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        _OnlineTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                innerTlvIndex += 1;
                index += desc.TotalSize;
            }

            MissedMessageCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MissedReason = (MissedMessageReason) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            HasData = true;

            DataSize = index;
        }

        public virtual List<byte> Serialize()
        {
            throw new NotImplementedException();
            //Dim data As List(Of Byte)
            //data = New List(Of Byte)

            //data.AddRange(ByteConverter.GetBytes(CUShort(_Channel)))
            //data.Add(_Uin.Length)
            //data.AddRange(ByteConverter.GetBytes(_Uin))
            //data.AddRange(ByteConverter.GetBytes(CUShort(_WarningLevel)))
            //data.AddRange(ByteConverter.GetBytes(CUShort(_WarningLevel)))
        }

        public int Deserialize(int offset, List<byte> data)
        {
            Deserialize(data.GetRange(offset, data.Count - offset));

            return DataSize;
        }
    }
}