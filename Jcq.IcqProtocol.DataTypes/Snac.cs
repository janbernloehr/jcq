// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac.cs" company="Jan-Cornelius Molnar">
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
using System.Threading;

namespace Jcq.IcqProtocol.DataTypes
{
    public abstract class Snac : ISerializable
    {
        public const int SizeFixPart = 10;
        private static long _snacRequestId;

        protected Snac(int serviceId, int subtypeId)
        {
            ServiceId = serviceId;
            SubtypeId = subtypeId;
        }

        public int ServiceId { get; set; }
        public int SubtypeId { get; set; }
        public int Flags { get; set; }
        public long RequestId { get; set; }

        public int TotalSize
        {
            get { return SizeFixPart + DataSize; }
            protected set { DataSize = value - SizeFixPart; }
        }

        public int DataSize { get; private set; }
        public bool HasData { get; private set; }
        public abstract int CalculateDataSize();

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public virtual void Deserialize(List<byte> data)
        {
            ServiceId = ByteConverter.ToUInt16(data.GetRange(0, 2));
            SubtypeId = ByteConverter.ToUInt16(data.GetRange(2, 2));
            Flags = ByteConverter.ToUInt16(data.GetRange(4, 2));
            RequestId = ByteConverter.ToUInt16(data.GetRange(6, 4));

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            DataSize = CalculateDataSize();

            var data = new List<byte>(SizeFixPart + DataSize);

            RequestId = Interlocked.Increment(ref _snacRequestId);

            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(ServiceId)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(SubtypeId)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(Flags)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt32(RequestId)));

            return data;
        }

        public static string GetKey(Snac snac)
        {
            return string.Format("{0:X2},{1:X2}", snac.SubtypeId == 1 ? 1 : snac.ServiceId, snac.SubtypeId);
        }
    }
}