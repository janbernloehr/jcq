// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac040B.cs" company="Jan-Cornelius Molnar">
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
    public class Snac040B : Snac
    {
        private readonly List<byte> _RequiredCapabilities = new List<byte>();

        public Snac040B() : base(0x4, 0xb)
        {
        }

        public long CookieID { get; set; }
        public MessageChannel Channel { get; set; }
        public string ScreenName { get; set; }
        public int AckReasonCode { get; set; }

        public List<byte> RequiredCapabilities
        {
            get { return _RequiredCapabilities; }
        }

        public string MessageText { get; set; }

        public override int CalculateDataSize()
        {
            return 8 + 2 + 1 + ScreenName.Length + 2 + 2 + 2 + _RequiredCapabilities.Count + 2 + 2 + 2 + 2 +
                   MessageText.Length;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ulong) CookieID));
            data.AddRange(ByteConverter.GetBytes((ushort) Channel));
            data.Add((byte) ScreenName.Length);
            data.AddRange(ByteConverter.GetBytes(ScreenName));

            data.AddRange(ByteConverter.GetBytes((ushort) AckReasonCode));

            // fragment identifier
            data.Add(0x5);
            // fragment version
            data.Add(0x1);

            data.AddRange(ByteConverter.GetBytes((ushort) _RequiredCapabilities.Count));
            data.AddRange(_RequiredCapabilities);

            // fragment identifier
            data.Add(0x1);
            // fragment version
            data.Add(0x1);

            data.AddRange(ByteConverter.GetBytes((ushort) (MessageText.Length + 2 + 2)));
            data.AddRange(new byte[]
            {
                0x0,
                0x0,
                0xff,
                0xff
            });

            data.AddRange(ByteConverter.GetBytes(MessageText));

            return data;
        }
    }
}