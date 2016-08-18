// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaResponse.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.DataTypes
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

        public int TotalSize => DataSize;

        public bool HasData { get; private set; }

        public virtual int Deserialize(List<byte> data)
        {
            int index = 0;

            DataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2);
            index += 2;

            ClientUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4));
            index += 4;

            ResponseType = (MetaResponseType) ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            ResponseSequenceNumber = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2);
            index += 2;

            HasData = true;
            return index;
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