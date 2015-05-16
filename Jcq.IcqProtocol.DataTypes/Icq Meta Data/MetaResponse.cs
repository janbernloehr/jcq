// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaResponse.cs" company="Jan-Cornelius Molnar">
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
    public abstract class MetaResponse : ISerializable
    {
        public const int SizeFixPart = 10;

        protected MetaResponse(MetaResponseType responseType)
        {
            ResponseType = responseType;
        }

        public long ClientUin { get; set; }
        public MetaResponseType ResponseType { get; set; }
        public int ResponseSequenceNumber { get; set; }
        public abstract int CalculateDataSize();

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return DataSize; }
        }

        public bool HasData { get; private set; }

        public virtual void Deserialize(List<byte> data)
        {
            var index = 0;

            DataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2);
            index += 2;

            ClientUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4));
            index += 4;

            ResponseType = (MetaResponseType) ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            ResponseSequenceNumber = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2);
            index += 2;

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            throw new NotImplementedException();
            //Dim data As List(Of Byte) = New List(Of Byte)

            //data.AddRange(ByteConverter.GetBytesLE(CUShort(Me.CalculateDataSize + (SizeFixPart - 2))))
            //data.AddRange(ByteConverter.GetBytesLE(CUInt(_ClientUin)))
            //data.AddRange(ByteConverter.GetBytesLE(CUShort(_ResponseType)))
            //data.AddRange(ByteConverter.GetBytesLE(CUShort(_ResponseSequenceNumber)))

            //Return data
        }

        protected void SetDataSize(int index)
        {
            DataSize = index - SizeFixPart;
        }
    }
}