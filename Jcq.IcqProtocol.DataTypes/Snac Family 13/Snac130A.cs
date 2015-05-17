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
        private readonly List<SSIBuddyRecord> _BuddyRecords = new List<SSIBuddyRecord>();
        private readonly List<SSIDenyRecord> _DenyRecords = new List<SSIDenyRecord>();
        private readonly List<SSIGroupRecord> _GroupRecords = new List<SSIGroupRecord>();
        private readonly List<SSIIgnoreListRecord> _IgnoreListRecords = new List<SSIIgnoreListRecord>();
        private readonly List<SSIPermitRecord> _PermitRecords = new List<SSIPermitRecord>();

        public Snac130A() : base(0x13, 0xa)
        {
        }

        public List<SSIBuddyRecord> BuddyRecords
        {
            get { return _BuddyRecords; }
        }

        public List<SSIGroupRecord> GroupRecords
        {
            get { return _GroupRecords; }
        }

        public List<SSIPermitRecord> PermitRecords
        {
            get { return _PermitRecords; }
        }

        public List<SSIIgnoreListRecord> IgnoreListRecords
        {
            get { return _IgnoreListRecords; }
        }

        public List<SSIDenyRecord> DenyRecords
        {
            get { return _DenyRecords; }
        }

        public SSIBuddyIcon BuddyIcon { get; set; }
        public int MaxItemId { get; private set; }

        public override int CalculateDataSize()
        {
            var size = 0;

            size += _BuddyRecords.Sum(x => x.CalculateTotalSize());
            size += _GroupRecords.Sum(x => x.CalculateTotalSize());
            size += _PermitRecords.Sum(x => x.CalculateTotalSize());
            size += _DenyRecords.Sum(x => x.CalculateTotalSize());
            size += _IgnoreListRecords.Sum(x => x.CalculateTotalSize());

            if (BuddyIcon != null)
            {
                size += BuddyIcon.CalculateTotalSize();
            }

            return size;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var desc = SSIItemDescriptor.GetDescriptor(index, data);

                switch (desc.ItemType)
                {
                    case SSIItemType.BuddyRecord:
                        SSIBuddyRecord buddy;
                        buddy = new SSIBuddyRecord();
                        buddy.Deserialize(data.GetRange(index, desc.TotalSize));
                        _BuddyRecords.Add(buddy);
                        break;
                    case SSIItemType.GroupRecord:
                        SSIGroupRecord @group;
                        @group = new SSIGroupRecord();
                        @group.Deserialize(data.GetRange(index, desc.TotalSize));
                        _GroupRecords.Add(@group);
                        break;
                    case SSIItemType.PermitRecord:
                        SSIPermitRecord permit;
                        permit = new SSIPermitRecord();
                        permit.Deserialize(data.GetRange(index, desc.TotalSize));
                        _PermitRecords.Add(permit);
                        break;
                    case SSIItemType.DenyRecord:
                        SSIDenyRecord deny;
                        deny = new SSIDenyRecord();
                        deny.Deserialize(data.GetRange(index, desc.TotalSize));
                        _DenyRecords.Add(deny);
                        break;
                    case SSIItemType.IgnoreListRecord:
                        SSIIgnoreListRecord ignore;
                        ignore = new SSIIgnoreListRecord();
                        ignore.Deserialize(data.GetRange(index, desc.TotalSize));
                        _IgnoreListRecords.Add(ignore);
                        break;
                    default:
                        Kernel.Logger.Log("Snac130A", TraceEventType.Error, "Unsupported SSI item type: {0}",
                            desc.ItemType);
                        break;
                }

                index += desc.TotalSize;

                MaxItemId = Math.Max(desc.ItemId, MaxItemId);
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var x in _BuddyRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (var x in _GroupRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (var x in _PermitRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (var x in _DenyRecords)
            {
                data.AddRange(x.Serialize());
            }
            foreach (var x in _IgnoreListRecords)
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