// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaResponseDescriptor.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class MetaResponseDescriptor
    {
        public int TotalSize { get; private set; }
        public MetaResponseType ResponseType { get; set; }
        public MetaResponseSubType ResponseSubType { get; set; }

        public static MetaResponseDescriptor GetDescriptor(int offset, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);
            var desc = new MetaResponseDescriptor();
            desc.Deserialize(data);
            return desc;
        }

        private void Deserialize(List<byte> data)
        {
            var index = 0;

            TotalSize = 2 + ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            index += 4;

            ResponseType = (MetaResponseType) ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            index += 2;

            if (ResponseType == MetaResponseType.MetaInformationResponse)
            {
                index += 2;

                ResponseSubType = (MetaResponseSubType) ByteConverter.ToUInt16LE(data.GetRange(index, 2));
            }
        }
    }
}