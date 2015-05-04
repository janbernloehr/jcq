// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac040B.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
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