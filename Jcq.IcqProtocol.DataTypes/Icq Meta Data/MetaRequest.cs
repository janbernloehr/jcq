// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaRequest.cs" company="Jan-Cornelius Molnar">
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
    public abstract class MetaRequest : ISerializable
    {
        public const int SizeFixPart = 10;
        private static int _reqestSequenceNumber;

        protected MetaRequest(MetaRequestType requestType)
        {
            RequestType = requestType;
        }

        public long ClientUin { get; set; }
        public MetaRequestType RequestType { get; set; }
        public int RequestSequenceNumber { get; set; }
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

        public virtual List<byte> Serialize()
        {
            var data = new List<byte>();

            data.AddRange(ByteConverter.GetBytesLE((ushort) (CalculateDataSize() + SizeFixPart - 2)));
            data.AddRange(ByteConverter.GetBytesLE((uint) ClientUin));
            data.AddRange(ByteConverter.GetBytesLE((ushort) RequestType));
            data.AddRange(ByteConverter.GetBytesLE((ushort) RequestSequenceNumber));

            return data;
        }

        public virtual void Deserialize(List<byte> data)
        {
            //Dim index As Integer

            //_DataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2)
            //index += 2

            //_ClientUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4))
            //index += 4

            //_RequestType = DirectCast(CInt(ByteConverter.ToUInt16LE(data.GetRange(index, 2))), MetaRequestType)
            //index += 2

            //_RequestSequenceNumber = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2)
            //index += 2

            //_HasData = True
            throw new NotImplementedException();
        }

        public static int GetNextSequenceNumber()
        {
            _reqestSequenceNumber += 1;
            return _reqestSequenceNumber;
        }

        protected void SetDataSize(int index)
        {
            DataSize = index - SizeFixPart;
        }
    }
}