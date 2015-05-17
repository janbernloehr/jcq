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
        private readonly List<SSIBuddyRecord> _BuddyRecords = new List<SSIBuddyRecord>();
        private readonly List<SSIDenyRecord> _DenyRecords = new List<SSIDenyRecord>();
        private readonly List<SSIGroupRecord> _GroupRecords = new List<SSIGroupRecord>();
        private readonly List<SSIIgnoreListRecord> _IgnoreListRecords = new List<SSIIgnoreListRecord>();
        private readonly SSIPermitDenySettings _PermitDenySettings = new SSIPermitDenySettings();
        private readonly List<SSIPermitRecord> _PermitRecords = new List<SSIPermitRecord>();
        private readonly SSIRosterImportTime _RosterImportTime = new SSIRosterImportTime();

        public Snac1306() : base(0x13, 0x6)
        {
        }

        public DateTime LastChange { get; set; }
        public int ItemCount { get; set; }

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

        public SSIPermitDenySettings PermitDenySettings
        {
            get { return _PermitDenySettings; }
        }

        public SSIRosterImportTime RosterImportTime
        {
            get { return _RosterImportTime; }
        }

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

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            // Version info (byte)
            index += 1;

            var itemIndex = 0;
            var itemCount = ByteConverter.ToUInt16(data.GetRange(index, 2));

            index += 2;

            while (itemIndex < itemCount)
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
                    case SSIItemType.PermitDenySettings:
                        _PermitDenySettings.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case SSIItemType.RosterImportTime:
                        _RosterImportTime.Deserialize(data.GetRange(index, desc.TotalSize));
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
        }
    }

    public class TlvBuddyIcon : Tlv
    {
        private readonly List<byte> _IconHash = new List<byte>();

        public TlvBuddyIcon() : base(0xd5)
        {
        }

        public List<byte> IconHash
        {
            get { return _IconHash; }
        }

        public override int CalculateDataSize()
        {
            return 2 + IconHash.Count;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            int length = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            //_IconHash.AddRange(data.GetRange(index, length))
        }

        public override List<byte> Serialize()
        {
            List<byte> data;

            data = base.Serialize();

            // MD5 Hash Size
            data.AddRange(ByteConverter.GetBytes((ushort) IconHash.Count));

            // MD5 Hash
            data.AddRange(IconHash);

            return data;
        }
    }
}