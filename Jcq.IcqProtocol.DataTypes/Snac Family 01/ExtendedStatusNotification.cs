// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedStatusNotification.cs" company="Jan-Cornelius Molnar">
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