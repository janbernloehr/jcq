// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0406.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0406 : Snac
    {
        public Snac0406() : base(0x4, 0x6)
        {
        }

        public long CookieID { get; set; }
        public MessageChannel Channel { get; set; }
        public string ScreenName { get; set; }

        public TlvMessageData MessageData { get; } = new TlvMessageData();

        public bool RequestAnAckFromServer { get; set; }
        public bool StoreMessageIfRecipientIsOffline { get; set; }

        public override int CalculateDataSize()
        {
            return 8 + 2 + 1 + ScreenName.Length + MessageData.CalculateTotalSize() + (RequestAnAckFromServer
                ? 4
                : 0) + (StoreMessageIfRecipientIsOffline ? 4 : 0);
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            if (Channel != MessageChannel.Channel1PlainText)
                throw new NotImplementedException();

            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ulong) CookieID));
            data.AddRange(ByteConverter.GetBytes((ushort) Channel));
            data.Add((byte) ScreenName.Length);
            data.AddRange(ByteConverter.GetBytes(ScreenName));

            data.AddRange(MessageData.Serialize());

            if (RequestAnAckFromServer)
            {
                data.AddRange(new TlvRequestAckFromServer().Serialize());
            }

            if (StoreMessageIfRecipientIsOffline)
            {
                data.AddRange(new TlvStoreMessageIfRecipientOffline().Serialize());
            }

            return data;
        }
    }
}