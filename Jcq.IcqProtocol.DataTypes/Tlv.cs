// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tlv.cs" company="Jan-Cornelius Molnar">
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
    public abstract class Tlv : ISerializable
    {
        public const int SizeFixPart = 4;

        protected Tlv(int typeNumber)
        {
            TypeNumber = typeNumber;
        }

        protected Tlv(int typeNumber, int dataSize) : this(typeNumber)
        {
            DataSize = dataSize;
        }

        public int TypeNumber { get; set; }
        public bool HasData { get; private set; }
        public virtual int DataSize { get; private set; }
        public abstract int CalculateDataSize();

        public int TotalSize => SizeFixPart + DataSize;

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public virtual int Deserialize(List<byte> data)
        {
            TypeNumber = ByteConverter.ToUInt16(data.GetRange(0, 2));
            DataSize = ByteConverter.ToUInt16(data.GetRange(2, 2));

            HasData = true;

            return 4;
        }

        public virtual List<byte> Serialize()
        {
            DataSize = CalculateDataSize();

            var data = new List<byte>(SizeFixPart + DataSize);

            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(TypeNumber)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(DataSize)));

            return data;
        }

        protected void SetDataSize(int value)
        {
            DataSize = value;
            HasData = true;
        }
    }

    //Public Class UnknownTlv
    //    Inherits Tlv


    //    public Overrides Function CalculateDataSize() As Integer
    //        Throw New NotImplementedException
    //    End Function
    //End Class
}