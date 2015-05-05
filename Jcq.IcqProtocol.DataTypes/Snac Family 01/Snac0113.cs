// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0113.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0113 : Snac
    {
        private readonly TlvMessageOfTheDay _MessageOfTheDay = new TlvMessageOfTheDay();
        private int _tlvsSize;

        public Snac0113() : base(0x1, 0x13)
        {
        }

        public MessageOfTheDayType MessageOfTheDayType { get; set; }

        public TlvMessageOfTheDay MessageOfTheDay
        {
            get { return _MessageOfTheDay; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            MessageOfTheDayType = (MessageOfTheDayType) (byte) ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (index + 4 <= data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0xb:
                        _MessageOfTheDay.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                _tlvsSize += desc.TotalSize;
                index += desc.TotalSize;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + _tlvsSize;
        }
    }
}