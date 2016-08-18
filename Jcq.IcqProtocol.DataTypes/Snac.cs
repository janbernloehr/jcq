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

        public int Deserialize(List<byte> data)
        {
            return Deserialize(SnacDescriptor.GetDescriptor(0, data), data);
        }

        public virtual int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            ServiceId = descriptor.ServiceId;
            SubtypeId = descriptor.SubtypeId;
            Flags = descriptor.Flags;
            RequestId = descriptor.RequestId;

            HasData = true;
            return SizeFixPart;
        }

        public virtual List<byte> Serialize()
        {
            DataSize = CalculateDataSize();

            var data = new List<byte>(SizeFixPart + DataSize);

            if (RequestId == 0)
                RequestId = Interlocked.Increment(ref _snacRequestId);

            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(ServiceId)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(SubtypeId)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(Flags)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt32(RequestId)));

            return data;
        }

        public static Tuple<int, int> GetKey(Snac snac)
        {
            return new Tuple<int, int>(snac.ServiceId, snac.SubtypeId);
        }
    }
}