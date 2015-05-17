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
        private readonly TlvAwayMessageString _AwayMessage = new TlvAwayMessageString();
        private readonly TlvAwayMessageEncoding _AwayMessageEncoding = new TlvAwayMessageEncoding();
        private readonly TlvClientIdleTime _ClientIdleTime = new TlvClientIdleTime();
        private readonly TlvClientProfileString _ClientProfileString = new TlvClientProfileString();
        private readonly TlvDCInfo _DCInfo = new TlvDCInfo();
        private readonly TlvEncodingType _EncodingType = new TlvEncodingType();
        private readonly TlvExternalIpAddress _ExternalAddress = new TlvExternalIpAddress();
        private readonly TlvMemberSince _MemberSince = new TlvMemberSince();
        private readonly TlvSignonTime _SignOnTime = new TlvSignonTime();
        private readonly TlvCapabilities _UserCapabilities = new TlvCapabilities();
        private readonly TlvUserClass _UserClass = new TlvUserClass();
        private readonly TlvUserStatus _UserStatus = new TlvUserStatus();

        public Snac0206() : base(0x2, 0x6)
        {
        }

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

        public TlvClientIdleTime ClientIdleTime
        {
            get { return _ClientIdleTime; }
        }

        public TlvSignonTime SignOnTime
        {
            get { return _SignOnTime; }
        }

        public TlvMemberSince MemberSince
        {
            get { return _MemberSince; }
        }

        public TlvEncodingType EncodingType
        {
            get { return _EncodingType; }
        }

        public TlvClientProfileString ClientProfileString
        {
            get { return _ClientProfileString; }
        }

        public TlvAwayMessageEncoding AwayMessageEncoding
        {
            get { return _AwayMessageEncoding; }
        }

        public TlvAwayMessageString AwayMessage
        {
            get { return _AwayMessage; }
        }

        public TlvCapabilities UserCapabilities
        {
            get { return _UserCapabilities; }
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            int innerTlvCount;
            var innerTlvIndex = 0;

            Uin = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + Uin.Length;

            WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

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
                    case 0xc:
                        _DCInfo.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xa:
                        _ExternalAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        _ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        _MemberSince.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
                innerTlvIndex += 1;
            }

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        _EncodingType.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x2:
                        _ClientProfileString.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        _AwayMessageEncoding.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x4:
                        _AwayMessage.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x5:
                        _UserCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }
    }

    //TODO: Complete Implementation of Location Services to achieve 100% AIM Support
}