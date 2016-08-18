// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac130A.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using Jcq.Core;

namespace Jcq.IcqProtocol.DataTypes
{
    public class Snac130A : Snac
    {
        public Snac130A() : base(0x13, 0xa)
        {
        }

        public List<SSIBuddyRecord> BuddyRecords { get; } = new List<SSIBuddyRecord>();

        public List<SSIGroupRecord> GroupRecords { get; } = new List<SSIGroupRecord>();

        public List<SSIPermitRecord> PermitRecords { get; } = new List<SSIPermitRecord>();

        public List<SSIIgnoreListRecord> IgnoreListRecords { get; } = new List<SSIIgnoreListRecord>();

        public List<SSIDenyRecord> DenyRecords { get; } = new List<SSIDenyRecord>();

        public SSIBuddyIcon BuddyIcon { get; set; }
        public int MaxItemId { get; private set; }

        public override int CalculateDataSize()
        {
            int size = 0;

            size += BuddyRecords.Sum(x => x.CalculateTotalSize());
            size += GroupRecords.Sum(x => x.CalculateTotalSize());
            size += PermitRecords.Sum(x => x.CalculateTotalSize());
            size += DenyRecords.Sum(x => x.CalculateTotalSize());
            size += IgnoreListRecords.Sum(x => x.CalculateTotalSize());

            if (BuddyIcon != null)
            {
                size += BuddyIcon.CalculateTotalSize();
            }

            return size;
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            base.Deserialize(descriptor, data);

            int index = SizeFixPart;

            while (index < data.Count)
            {
                SSIItemDescriptor desc = SSIItemDescriptor.GetDescriptor(index, data);

                switch (desc.ItemType)
                {
                    case SSIItemType.BuddyRecord:
                        var buddy = new SSIBuddyRecord();
                        buddy.Deserialize(data.GetRange(index, desc.TotalSize));
                        BuddyRecords.Add(buddy);
                        break;
                    case SSIItemType.GroupRecord:
                        var @group = new SSIGroupRecord();
                        group.Deserialize(data.GetRange(index, desc.TotalSize));
                        GroupRecords.Add(group);
                        break;
                    case SSIItemType.PermitRecord:
                        var permit = new SSIPermitRecord();
                        permit.Deserialize(data.GetRange(index, desc.TotalSize));
                        PermitRecords.Add(permit);
                        break;
                    case SSIItemType.DenyRecord:
                        var deny = new SSIDenyRecord();
                        deny.Deserialize(data.GetRange(index, desc.TotalSize));
                        DenyRecords.Add(deny);
                        break;
                    case SSIItemType.IgnoreListRecord:
                        var ignore = new SSIIgnoreListRecord();
                        ignore.Deserialize(data.GetRange(index, desc.TotalSize));
                        IgnoreListRecords.Add(ignore);
                        break;
                    default:
                        Kernel.Logger.Log("Snac130A", TraceEventType.Error,
                            $"Unsupported SSI item type: {desc.ItemType}");
                        break;
                }

                index += desc.TotalSize;

                MaxItemId = Math.Max(desc.ItemId, MaxItemId);
            }

            TotalSize = index;
            return index;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (SSIBuddyRecord x in BuddyRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (SSIGroupRecord x in GroupRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (SSIPermitRecord x in PermitRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (SSIDenyRecord x in DenyRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (SSIIgnoreListRecord x in IgnoreListRecords)
            {
                data.AddRange(x.Serialize());
            }
            if (BuddyIcon != null)
            {
                data.AddRange(BuddyIcon.Serialize());
            }

            return data;
        }
    }
}