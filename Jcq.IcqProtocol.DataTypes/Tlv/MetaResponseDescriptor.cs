// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaResponseDescriptor.cs" company="Jan-Cornelius Molnar">
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
    public class MetaResponseDescriptor
    {
        public int TotalSize { get; private set; }
        public MetaResponseType ResponseType { get; set; }
        public MetaResponseSubType ResponseSubType { get; set; }

        public static MetaResponseDescriptor GetDescriptor(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);
            var desc = new MetaResponseDescriptor();
            desc.Deserialize(data);
            return desc;
        }

        private void Deserialize(List<byte> data)
        {
            var index = 0;

            TotalSize = 2 + ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            index += 4;

            ResponseType = (MetaResponseType) ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            if (ResponseType == MetaResponseType.MetaInformationResponse)
            {
                index += 2;

                ResponseSubType = (MetaResponseSubType) ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            }
        }
    }
}