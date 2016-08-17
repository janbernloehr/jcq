// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RateClass.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using Jcq.Core;

namespace Jcq.IcqProtocol.DataTypes
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
            int index = 0;

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

            Kernel.Logger.Log("RateLimits", TraceEventType.Information, ToString());
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
                    "{8} Window size: {0}\n{8} Clear level: {1}\n{8} Alert level: {2}\n{8} Limit level: {3}\n{8} Disconnect level: {4}\n{8} Current level: {5}\n{8} Max level: {6}\n{8} Last time: {7}",
                    WindowSize, ClearLevel, AlertLevel, LimitLevel, DisconnectLevel, CurrentLevel, MaxLevel, LastTime,
                    ClassId).Replace("\\n", Environment.NewLine);
        }
    }
}