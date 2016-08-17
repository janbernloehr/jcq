// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0407.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0407 : Snac
    {
        public Snac0407() : base(0x4, 0x7)
        {
        }

        public decimal CookieID { get; set; }
        public MessageChannel Channel { get; set; }
        public string ScreenName { get; set; }
        public int SenderWarningLevel { get; private set; }
        public bool AutoResponseFlag { get; private set; }

        public TlvUserClass UserClass { get; } = new TlvUserClass();

        public TlvUserStatus UserStatus { get; } = new TlvUserStatus();

        public TlvClientIdleTime ClientIdleTime { get; } = new TlvClientIdleTime();

        public TlvAccountCreationTime AccountCreationTime { get; } = new TlvAccountCreationTime();

        public TlvMessageData MessageData { get; } = new TlvMessageData();

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

            int index = SizeFixPart;

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
            int innerTlvIndex = 0;

            innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (innerTlvIndex < innerTlvCount)
            {
                desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0xf:
                        ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        AccountCreationTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x4:
                        AutoResponseFlag = true;
                        break;
                }

                index += desc.TotalSize;
                innerTlvIndex += 1;
            }

            desc = TlvDescriptor.GetDescriptor(index, data);

            MessageData.Deserialize(data.GetRange(index, desc.TotalSize));
            index += MessageData.TotalSize;

            while (index + 4 <= data.Count)
            {
                desc = TlvDescriptor.GetDescriptor(index, data);

                index += desc.TotalSize;
            }

            TotalSize = index;
        }
    }
}