// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvBuddyIcon.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2016 Jan-Cornelius Molnar
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

#region

using System.Collections.Generic;

#endregion

namespace Jcq.IcqProtocol.DataTypes
{
    public class TlvBuddyIcon : Tlv
    {
        public TlvBuddyIcon() : base(0xd5)
        {
        }

        public List<byte> IconHash { get; } = new List<byte>();

        public override int CalculateDataSize()
        {
            return 2 + IconHash.Count;
        }

        public override int Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            int length = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            //_IconHash.AddRange(data.GetRange(index, length))
            return index;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            // MD5 Hash Size
            data.AddRange(ByteConverter.GetBytes((ushort) IconHash.Count));

            // MD5 Hash
            data.AddRange(IconHash);

            return data;
        }
    }
}