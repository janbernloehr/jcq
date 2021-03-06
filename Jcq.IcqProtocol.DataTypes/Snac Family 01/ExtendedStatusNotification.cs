// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedStatusNotification.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace Jcq.IcqProtocol.DataTypes
{
    public abstract class ExtendedStatusNotification : ISerializable
    {
        public const int SizeFixPart = 2;
        public ExtendedStatusNotificationType Type { get; set; }
        public abstract int CalculateDataSize();

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public int DataSize { get; private set; }

        public int TotalSize
        {
            get { return SizeFixPart + DataSize; }
        }

        public bool HasData { get; private set; }

        public virtual void Deserialize(List<byte> data)
        {
            Type = (ExtendedStatusNotificationType) ByteConverter.ToUInt16(data.GetRange(0, 2));

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            List<byte> data;

            data = new List<byte>();

            data.AddRange(ByteConverter.GetBytes((uint) Type));

            return data;
        }

        protected void SetDataSize(int value)
        {
            DataSize = value;
        }
    }
}