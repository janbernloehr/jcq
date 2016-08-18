// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0206.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0206 : Snac
    {
        public Snac0206() : base(0x2, 0x6)
        {
        }

        public string Uin { get; set; }
        public int WarningLevel { get; set; }

        public TlvUserClass UserClass { get; } = new TlvUserClass();

        public TlvUserStatus UserStatus { get; } = new TlvUserStatus();

        public TlvDCInfo DCInfo { get; } = new TlvDCInfo();

        public TlvExternalIpAddress ExternalAddress { get; } = new TlvExternalIpAddress();

        public TlvClientIdleTime ClientIdleTime { get; } = new TlvClientIdleTime();

        public TlvSignonTime SignOnTime { get; } = new TlvSignonTime();

        public TlvMemberSince MemberSince { get; } = new TlvMemberSince();

        public TlvEncodingType EncodingType { get; } = new TlvEncodingType();

        public TlvClientProfileString ClientProfileString { get; } = new TlvClientProfileString();

        public TlvAwayMessageEncoding AwayMessageEncoding { get; } = new TlvAwayMessageEncoding();

        public TlvAwayMessageString AwayMessage { get; } = new TlvAwayMessageString();

        public TlvCapabilities UserCapabilities { get; } = new TlvCapabilities();

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            base.Deserialize(descriptor, data);

            int index = SizeFixPart;

            int innerTlvCount;
            int innerTlvIndex = 0;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (innerTlvIndex < innerTlvCount)
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
                    case 0xc:
                        DCInfo.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xa:
                        ExternalAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        MemberSince.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
                innerTlvIndex += 1;
            }

            while (index < data.Count)
            {
                TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        EncodingType.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x2:
                        ClientProfileString.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        AwayMessageEncoding.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x4:
                        AwayMessage.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        UserCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
            return index;
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }
    }

    //TODO: Complete Implementation of Location Services to achieve 100% AIM Support
}