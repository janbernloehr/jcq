// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tlv.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public abstract class Tlv : ISerializable
    {
        public const int SizeFixPart = 4;

        protected Tlv(int typeNumber)
        {
            TypeNumber = typeNumber;
        }

        protected Tlv(int typeNumber, int dataSize) : this(typeNumber)
        {
            DataSize = dataSize;
        }

        public int TypeNumber { get; set; }
        public bool HasData { get; private set; }
        public virtual int DataSize { get; private set; }
        public abstract int CalculateDataSize();

        public int TotalSize
        {
            get { return SizeFixPart + DataSize; }
        }

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public virtual void Deserialize(List<byte> data)
        {
            TypeNumber = ByteConverter.ToUInt16(data.GetRange(0, 2));
            DataSize = ByteConverter.ToUInt16(data.GetRange(2, 2));

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            DataSize = CalculateDataSize();

            var data = new List<byte>(SizeFixPart + DataSize);

            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(TypeNumber)));
            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(DataSize)));

            return data;
        }

        protected void SetDataSize(int value)
        {
            DataSize = value;
            HasData = true;
        }
    }

    //Public Class UnknownTlv
    //    Inherits Tlv


    //    public Overrides Function CalculateDataSize() As Integer
    //        Throw New NotImplementedException
    //    End Function
    //End Class
}