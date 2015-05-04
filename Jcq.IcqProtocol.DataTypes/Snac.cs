// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Collections.Generic;
using System.Threading;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public abstract class Snac : ISerializable
    {
        public const int SizeFixPart = 10;
        private static long snacRequestId;

        protected Snac(int serviceId, int subtypeId)
        {
            ServiceId = serviceId;
            SubtypeId = subtypeId;
        }

        public int ServiceId { get; set; }
        public int SubtypeId { get; set; }
        public int Flags { get; set; }
        public long RequestId { get; set; }

        public int SnacDataSize
        {
            get { return DataSize; }
        }

        public int SnacTotalSize
        {
            get { return SizeFixPart + SnacDataSize; }
        }

        public int TotalSize
        {
            get { return SizeFixPart + SnacDataSize; }
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
            List<byte> data;

            DataSize = CalculateDataSize();

            data = new List<byte>(SizeFixPart + DataSize);

            RequestId = Interlocked.Increment(ref snacRequestId);

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

        protected void SetTotalSize(int value)
        {
            DataSize = value - SizeFixPart;
        }
    }
}