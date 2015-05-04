// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacDescriptor.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class SnacDescriptor
    {
        public int ServiceId { get; set; }
        public int SubtypeId { get; set; }
        public int Flags { get; set; }
        public long RequestId { get; set; }

        public static string GetKey(SnacDescriptor descriptor)
        {
            return string.Format("{0:X2},{1:X2}", descriptor.SubtypeId == 1 ? 1 : descriptor.ServiceId,
                descriptor.SubtypeId);
        }

        public virtual void Deserialize(List<byte> data)
        {
            ServiceId = ByteConverter.ToUInt16(data.GetRange(0, 2));
            SubtypeId = ByteConverter.ToUInt16(data.GetRange(2, 2));
            Flags = ByteConverter.ToUInt16(data.GetRange(4, 2));
            RequestId = ByteConverter.ToUInt32(data.GetRange(6, 4));
        }

        public static SnacDescriptor GetDescriptor(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);
            var desc = new SnacDescriptor();
            desc.Deserialize(data);
            return desc;
        }
    }
}