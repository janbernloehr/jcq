// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvMessageData.cs" company="Jan-Cornelius Molnar">
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
    public class TlvMessageData : Tlv
    {
        private readonly List<byte> _requiredCapabilities = new List<byte>();

        public TlvMessageData()
            : base(0x2)
        {
            _requiredCapabilities.AddRange(new Byte[] {1, 6});
        }

        public List<byte> RequiredCapabilities
        {
            get { return _requiredCapabilities; }
        }

        public string MessageText { get; set; }

        public override int CalculateDataSize()
        {
            return 4 + _requiredCapabilities.Count + 4 + 4 + 2*MessageText.Length;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            // fragment identifier
            data.Add(0x5);
            // fragment version
            data.Add(0x1);

            data.AddRange(ByteConverter.GetBytes((ushort) _requiredCapabilities.Count));
            data.AddRange(_requiredCapabilities);

            // fragment identifier
            data.Add(0x1);
            // fragment version
            data.Add(0x1);

            // Unicode (does not work ...)
            var messageBytes = Encoding.BigEndianUnicode.GetBytes(MessageText);
            data.AddRange(ByteConverter.GetBytes((ushort) (4 + messageBytes.Length)));

            data.AddRange(new byte[]
            {
                0x0,
                0x2,
                0x0,
                0x0
            });

            data.AddRange(messageBytes);

            //var messageBytes = ByteConverter.GetBytes(MessageText);
            //data.AddRange(ByteConverter.GetBytes((ushort)(4 + messageBytes.Length)));
            //data.AddRange(new byte[]
            //{
            //    0x0,
            //    0x0,
            //    0xff,
            //    0xff
            //});

            //data.AddRange(messageBytes);

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            index += 2;

            int length = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            _requiredCapabilities.AddRange(data.GetRange(index, length));
            index += length;

            index += 2;

            length = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            index += 4;

            //MessageText = ByteConverter.ToString(data.GetRange(index, length - 4));
            MessageText = Encoding.BigEndianUnicode.GetString(data.GetRange(index, length - 4).ToArray());

            index += length - 4;
        }
    }
}