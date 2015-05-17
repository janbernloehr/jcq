// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfflineMessageResponse.cs" company="Jan-Cornelius Molnar">
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
using System.Text;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class OfflineMessageResponse : MetaResponse
    {
        public OfflineMessageResponse() : base(MetaResponseType.OfflineMessageResponse)
        {
        }

        public long SenderUin { get; set; }
        public DateTime DateSent { get; set; }
        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
        public MessageFlag MessageFlags { get; set; }

        public override int CalculateDataSize()
        {
            //Return 2 + 4 + 2 + 2 + 4 + 6 + 1 + 1 + 2 + _MessageText.Length
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            SenderUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4));
            index += 4;

            int year = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            int month = data[index];
            index += 1;

            int day = data[index];
            index += 1;

            int hour = data[index];
            index += 1;

            int minute = data[index];
            index += 1;

            DateSent = new DateTime(year, month, day, hour, minute, 0);

            MessageType = (MessageType) data[index];
            index += 1;

            MessageFlags = (MessageFlag) data[index];
            index += 1;

            MessageText = ByteConverter.ToStringFromUInt16LEIndex(index, data);
            if (MessageText.EndsWith(Encoding.UTF8.GetString(new byte[] {0})))
                MessageText = MessageText.Substring(0, MessageText.Length - 1);
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}