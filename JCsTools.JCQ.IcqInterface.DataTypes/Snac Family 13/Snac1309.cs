//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public class Snac1309 : Snac
  {
    public Snac1309() : base(0x13, 0x9)
    {
    }

    public override int CalculateDataSize()
    {
      int size;
      foreach (SSIRecord x in _BuddyRecords) {
        size += x.CalculateTotalSize;
      }
      foreach (SSIRecord x in _GroupRecords) {
        size += x.CalculateTotalSize;
      }
      foreach (SSIRecord x in _PermitRecords) {
        size += x.CalculateTotalSize;
      }
      foreach (SSIRecord x in _DenyRecords) {
        size += x.CalculateTotalSize;
      }
      foreach (SSIRecord x in _IgnoreListRecords) {
        size += x.CalculateTotalSize;
      }
      if (BuddyIcon != null) {
        size += BuddyIcon.CalculateTotalSize;
      }
      return size;
    }

    private List<SSIBuddyRecord> _BuddyRecords = new List<SSIBuddyRecord>();
    public List<SSIBuddyRecord> BuddyRecords {
      get { return _BuddyRecords; }
    }

    private List<SSIGroupRecord> _GroupRecords = new List<SSIGroupRecord>();
    public List<SSIGroupRecord> GroupRecords {
      get { return _GroupRecords; }
    }

    private List<SSIPermitRecord> _PermitRecords = new List<SSIPermitRecord>();
    public List<SSIPermitRecord> PermitRecords {
      get { return _PermitRecords; }
    }

    private List<SSIIgnoreListRecord> _IgnoreListRecords = new List<SSIIgnoreListRecord>();
    public List<SSIIgnoreListRecord> IgnoreListRecords {
      get { return _IgnoreListRecords; }
    }

    private List<SSIDenyRecord> _DenyRecords = new List<SSIDenyRecord>();
    public List<SSIDenyRecord> DenyRecords {
      get { return _DenyRecords; }
    }

    private SSIBuddyIcon _BuddyIcon;
    public SSIBuddyIcon BuddyIcon {
      get { return _BuddyIcon; }
      set { _BuddyIcon = value; }
    }

    private int _MaxItemId;
    public int MaxItemId {
      get { return _MaxItemId; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      while (index < data.Count) {
        SSIItemDescriptor desc = SSIItemDescriptor.GetDescriptor(index, data);

        switch (desc.ItemType) {
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
            Core.Kernel.Logger.Log("Snac1309", TraceEventType.Error, "Unsupported SSI item type: {0}", desc.ItemType);
            break;
        }

        index += desc.TotalSize;

        _MaxItemId = Math.Max(desc.ItemId, MaxItemId);
      }

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      foreach (SSIRecord x in _BuddyRecords) {
        data.AddRange(x.Serialize);
      }
      foreach (SSIRecord x in _GroupRecords) {
        data.AddRange(x.Serialize);
      }
      foreach (SSIRecord x in _PermitRecords) {
        data.AddRange(x.Serialize);
      }
      foreach (SSIRecord x in _DenyRecords) {
        data.AddRange(x.Serialize);
      }
      foreach (SSIRecord x in _IgnoreListRecords) {
        data.AddRange(x.Serialize);
      }
      if (BuddyIcon != null) {
        data.AddRange(BuddyIcon.Serialize);
      }

      return data;
    }
  }
}

