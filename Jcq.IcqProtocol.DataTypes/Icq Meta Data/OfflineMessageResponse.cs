// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfflineMessageResponse.cs" company="Jan-Cornelius Molnar">
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

            int year;
            int month;
            int day;
            int hour;
            int minute;

            year = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            month = data[index];
            index += 1;

            day = data[index];
            index += 1;

            hour = data[index];
            index += 1;

            minute = data[index];
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