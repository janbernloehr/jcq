// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaRequest.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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