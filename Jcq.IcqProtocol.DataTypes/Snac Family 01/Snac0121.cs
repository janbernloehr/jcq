// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0121.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0121 : Snac
    {
        public Snac0121() : base(0x1, 0x21)
        {
        }

        public ExtendedStatusNotification Notification { get; private set; }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            if (data.Count > index + 2 && (data[index] == 0 & data[index + 1] == 6))
            {
                // Icq sends information about the service version

                index += 2;

                TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

                index += desc.TotalSize;
            }

            ExtendedStatusNotificationType type;

            type = (ExtendedStatusNotificationType) ByteConverter.ToUInt16(data.GetRange(index, 2));

            switch (type)
            {
                case ExtendedStatusNotificationType.UploadIconRequest:
                    Notification = new UploadIconNotification();
                    break;
                case ExtendedStatusNotificationType.iChatAvialable:
                    Notification = new ChatAvailableNotification();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            Notification.Deserialize(data.GetRange(index, data.Count - index));

            index += Notification.TotalSize;

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }
    }
}