// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamilyIdToolPair.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
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