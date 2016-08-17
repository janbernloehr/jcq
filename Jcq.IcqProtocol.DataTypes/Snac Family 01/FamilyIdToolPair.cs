// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamilyIdToolPair.cs" company="Jan-Cornelius Molnar">
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
    public class FamilyIdToolPair : ISerializable
    {
        public const int SizeFixPart = 8;

        public FamilyIdToolPair()
        {
        }

        public FamilyIdToolPair(int serviceId, int version, int toolId, int toolVersion)
        {
            FamilyNumber = serviceId;
            FamilyVersion = version;
            ToolId = toolId;
            ToolVersion = toolVersion;
        }

        public int FamilyNumber { get; set; }
        public int FamilyVersion { get; set; }
        public int ToolId { get; set; }
        public int ToolVersion { get; set; }

        public int DataSize
        {
            get { return 0; }
        }

        public int TotalSize
        {
            get { return SizeFixPart; }
        }

        public virtual int CalculateDataSize()
        {
            return 0;
        }

        public int CalculateTotalSize()
        {
            return SizeFixPart;
        }

        public virtual void Deserialize(List<byte> data)
        {
            FamilyNumber = ByteConverter.ToUInt16(data.GetRange(0, 2));
            FamilyVersion = ByteConverter.ToUInt16(data.GetRange(2, 2));
            ToolId = ByteConverter.ToUInt16(data.GetRange(4, 2));
            ToolVersion = ByteConverter.ToUInt16(data.GetRange(6, 2));

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            List<byte> data;

            data = new List<byte>();

            data.AddRange(ByteConverter.GetBytes((ushort) FamilyNumber));
            data.AddRange(ByteConverter.GetBytes((ushort) FamilyVersion));
            data.AddRange(ByteConverter.GetBytes((ushort) ToolId));
            data.AddRange(ByteConverter.GetBytes((ushort) ToolVersion));

            return data;
        }

        public bool HasData { get; private set; }
    }
}