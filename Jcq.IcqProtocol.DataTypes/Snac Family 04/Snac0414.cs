// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0414.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac0414 : Snac
    {
        public Snac0414() : base(0x4, 0x14)
        {
        }

        public long CookieID { get; set; }
        public MessageChannel Channel { get; set; }
        public string ScreenName { get; set; }
        public NotificationType NotificationType { get; set; }

        public override int CalculateDataSize()
        {
            return 8 + 2 + 1 + ScreenName.Length + 2;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            CookieID = (long) ByteConverter.ToUInt64(data.GetRange(index, 8));
            index += 8;

            Channel = (MessageChannel) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            ScreenName = ByteConverter.ToStringFromByteIndex(index, data);
            index += 1 + ScreenName.Length;

            NotificationType = (NotificationType) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((ulong) CookieID));
            data.AddRange(ByteConverter.GetBytes((ushort) Channel));
            data.Add((byte) ScreenName.Length);
            data.AddRange(ByteConverter.GetBytes(ScreenName));

            data.AddRange(ByteConverter.GetBytes((ushort) NotificationType));

            return data;
        }
    }
}