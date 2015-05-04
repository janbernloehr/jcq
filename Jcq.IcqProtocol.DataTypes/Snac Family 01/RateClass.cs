// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RateClass.cs" company="Jan-Cornelius Molnar">
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
    public class RateClass : ISerializable
    {
        public long ClassId { get; set; }
        public long WindowSize { get; set; }
        public long ClearLevel { get; set; }
        public long AlertLevel { get; set; }
        public long LimitLevel { get; set; }
        public long DisconnectLevel { get; set; }
        public long CurrentLevel { get; set; }
        public long MaxLevel { get; set; }
        public long LastTime { get; set; }
        public byte CurrentState { get; set; }

        public int TotalSize
        {
            get { return DataSize; }
        }

        public int DataSize { get; private set; }

        public virtual int CalculateDataSize()
        {
            return 35;
        }

        public int CalculateTotalSize()
        {
            return CalculateDataSize();
        }

        public virtual void Deserialize(List<byte> data)
        {
            var index = 0;

            ClassId = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            WindowSize = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            ClearLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            AlertLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            LimitLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            DisconnectLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            CurrentLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            MaxLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            LastTime = ByteConverter.ToUInt32(data.GetRange(index, 4));
            index += 4;

            CurrentState = data[index];
            index += 1;

            DataSize = index;
            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public bool HasData { get; private set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Window size: {0}\\nClear level: {1}\\nAlert level: {2}\\n" +
                    "Limit level: {3}\\nDisconnect level: {4}\\nCurrent level: {5}\\n" +
                    "Max level: {6}\\nLast time: {7}", WindowSize, ClearLevel, AlertLevel, LimitLevel, DisconnectLevel,
                    CurrentLevel, MaxLevel, LastTime).Replace("\\n", Environment.NewLine);
        }
    }
}