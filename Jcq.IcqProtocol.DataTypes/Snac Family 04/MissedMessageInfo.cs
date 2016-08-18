// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissedMessageInfo.cs" company="Jan-Cornelius Molnar">
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
    public class MissedMessageInfo : ISerializable
    {
        public MessageChannel Channel { get; set; }
        public string Uin { get; set; }
        public int WarningLevel { get; set; }

        public TlvUserClass UserClass { get; } = new TlvUserClass();

        public TlvUserStatus UserStatus { get; } = new TlvUserStatus();

        public TlvOnlineTime OnlineTime { get; } = new TlvOnlineTime();

        public TlvSignonTime SignOnTime { get; } = new TlvSignonTime();

        public MissedMessageReason MissedReason { get; set; }
        public int MissedMessageCount { get; set; }

        public int TotalSize => DataSize;

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

        public int Deserialize(List<byte> data)
        {
            int index = 0;

            Channel = (MessageChannel) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;
            
            int innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            for (int innerTlvIndex = 0; innerTlvIndex < innerTlvCount; innerTlvIndex++)
            {
                TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        OnlineTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }
            
            MissedMessageCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            MissedReason = (MissedMessageReason) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            HasData = true;

            DataSize = index;

            return index;
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