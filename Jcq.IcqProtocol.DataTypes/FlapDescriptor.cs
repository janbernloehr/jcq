// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapDescriptor.cs" company="Jan-Cornelius Molnar">
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
    public class FlapDescriptor
    {
        public FlapChannel Channel { get; set; }
        public int DatagramSequenceNumber { get; set; }
        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return 6 + DataSize; }
        }

        public static FlapDescriptor GetDescriptor(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);

            var desc = new FlapDescriptor();
            desc.Deserialize(data);

            return desc;
        }

        private void Deserialize(List<byte> data)
        {
            var index = 0;

            if (data[index] != 0x2a)
            {
                string info = null;

                for (var i = 0; i <= Flap.SizeFixPart - 1; i++)
                {
                    info += string.Format("{0:X} ", data[i]);
                }

                throw new InvalidOperationException(string.Format("No flap at this position: {0}", info));
            }

            index += 1;

            Channel = (FlapChannel) data[index];
            index += 1;

            DatagramSequenceNumber = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;
        }
    }
}