// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac1306.cs" company="Jan-Cornelius Molnar">
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
    public class Snac1306 : Snac
    {
        public Snac1306() : base(0x13, 0x6)
        {
        }

        public DateTime LastChange { get; set; }
        public int ItemCount { get; set; }

        public List<SSIBuddyRecord> BuddyRecords { get; } = new List<SSIBuddyRecord>();

        public List<SSIGroupRecord> GroupRecords { get; } = new List<SSIGroupRecord>();

        public List<SSIPermitRecord> PermitRecords { get; } = new List<SSIPermitRecord>();

        public List<SSIIgnoreListRecord> IgnoreListRecords { get; } = new List<SSIIgnoreListRecord>();

        public List<SSIDenyRecord> DenyRecords { get; } = new List<SSIDenyRecord>();

        public SSIPermitDenySettings PermitDenySettings { get; } = new SSIPermitDenySettings();

        public SSIRosterImportTime RosterImportTime { get; } = new SSIRosterImportTime();

        public SSIBuddyIcon BuddyIcon { get; private set; }
        public int MaxItemId { get; private set; }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int Deserialize(SnacDescriptor descriptor, List<byte> data)
        {
            base.Deserialize(descriptor, data);

            int index = SizeFixPart;

            // Version info (byte)
            index += 1;

            int itemIndex = 0;
            ushort itemCount = ByteConverter.ToUInt16(data.GetRange(index, 2));

            index += 2;

            while (itemIndex < itemCount)
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
                    case SSIItemType.PermitDenySettings:
                        PermitDenySettings.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case SSIItemType.RosterImportTime:
                        RosterImportTime.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case SSIItemType.OwnIconAvatarInfo:
                        BuddyIcon = new SSIBuddyIcon();
                        BuddyIcon.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    default:
                        Kernel.Logger.Log("Snac1306", TraceEventType.Error, "Unsupported SSI item type: {0}",
                            desc.ItemType);
                        break;
                }

                index += desc.TotalSize;
                itemIndex += 1;

                MaxItemId = Math.Max(desc.ItemId, MaxItemId);
            }

            LastChange = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
            index += 4;

            TotalSize = index;
            return index;
        }
    }
}