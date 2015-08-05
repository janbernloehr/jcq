// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacDescriptor.cs" company="Jan-Cornelius Molnar">
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
    public class SnacDescriptor
    {
        public int ServiceId { get; set; }
        public int SubtypeId { get; set; }
        public int Flags { get; set; }
        public long RequestId { get; set; }

        public static string GetKey(SnacDescriptor descriptor)
        {
            //return string.Format("{0:X2},{1:X2}", descriptor.SubtypeId == 1 ? 1 : descriptor.ServiceId,
            //    descriptor.SubtypeId);
            return string.Format("{0:X2},{1:X2}",  descriptor.ServiceId,
                descriptor.SubtypeId);
        }

        public virtual void Deserialize(List<byte> data)
        {
            ServiceId = ByteConverter.ToUInt16(data.GetRange(0, 2));
            SubtypeId = ByteConverter.ToUInt16(data.GetRange(2, 2));
            Flags = ByteConverter.ToUInt16(data.GetRange(4, 2));
            RequestId = ByteConverter.ToUInt32(data.GetRange(6, 4));
        }

        public static SnacDescriptor GetDescriptor(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);
            var desc = new SnacDescriptor();
            desc.Deserialize(data);
            return desc;
        }
    }
}