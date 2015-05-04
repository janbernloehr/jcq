// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvDescriptor.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class TlvDescriptor
    {
        private TlvDescriptor(int typeId, int dataSize)
        {
            TypeId = typeId;
            DataSize = dataSize;
        }

        public int TypeId { get; private set; }
        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return DataSize + 4; }
        }

        public static TlvDescriptor GetDescriptor(int offset, List<byte> data)
        {
            int typeId;
            int dataSize;

            if (offset + 4 > data.Count)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                throw new ArgumentException(
                    string.Format("Offset and length were out of bounds for the present data: {0}/{1}", offset + 4,
                        data.Count));
            }

            typeId = ByteConverter.ToUInt16(data.GetRange(offset, 2));
            dataSize = ByteConverter.ToUInt16(data.GetRange(offset + 2, 2));

            return new TlvDescriptor(typeId, dataSize);
        }
    }
}