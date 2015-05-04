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
  public class Snac1306 : Snac
  {
    public Snac1306() : base(0x13, 0x6)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    private System.DateTime _LastChange;
    public System.DateTime LastChange {
      get { return _LastChange; }
      set { _LastChange = value; }
    }


    private int _ItemCount;
    public int ItemCount {
      get { return _ItemCount; }
      set { _ItemCount = value; }
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

    private SSIPermitDenySettings _PermitDenySettings = new SSIPermitDenySettings();
    public SSIPermitDenySettings PermitDenySettings {
      get { return _PermitDenySettings; }
    }

    private SSIRosterImportTime _RosterImportTime = new SSIRosterImportTime();
    public SSIRosterImportTime RosterImportTime {
      get { return _RosterImportTime; }
    }

    private SSIBuddyIcon _BuddyIcon;
    public SSIBuddyIcon BuddyIcon {
      get { return _BuddyIcon; }
    }

    private int _MaxItemId;
    public int MaxItemId {
      get { return _MaxItemId; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      // Version info (byte)
      index += 1;

      int itemIndex;

      itemCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (itemIndex < itemCount) {
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
          case SSIItemType.PermitDenySettings:
            _PermitDenySettings.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case SSIItemType.RosterImportTime:
            _RosterImportTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case SSIItemType.OwnIconAvatarInfo:
            _BuddyIcon = new SSIBuddyIcon();
            _BuddyIcon.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          default:
            Core.Kernel.Logger.Log("Snac1306", TraceEventType.Error, "Unsupported SSI item type: {0}", desc.ItemType);
            break;
        }

        index += desc.TotalSize;
        itemIndex += 1;

        _MaxItemId = Math.Max(desc.ItemId, MaxItemId);
      }

      _LastChange = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
      index += 4;

      this.SetTotalSize(index);
    }
  }

  public class SSIItemDescriptor
  {
    private string _ItemName;
    public string ItemName {
      get { return _ItemName; }
    }

    private int _GroupId;
    public int GroupId {
      get { return _GroupId; }
    }

    private int _ItemId;
    public int ItemId {
      get { return _ItemId; }
    }

    private SSIItemType _ItemType;
    public SSIItemType ItemType {
      get { return _ItemType; }
    }

    private int _AdditionalDataIndex;
    public int AdditionalDataIndex {
      get { return _AdditionalDataIndex; }
    }

    public int TotalSize {
      get { return _DataSize + 2 + 2 + 2 + 2 + 2 + _ItemName.Length; }
    }

    private int _DataSize;
    public int DataSize {
      get { return _DataSize; }
    }

    public static SSIItemDescriptor GetDescriptor(int offset, List<byte> data)
    {
      SSIItemDescriptor descriptor = new SSIItemDescriptor();
      descriptor.Deserialize(offset, data);
      return descriptor;
    }

    private void Deserialize(int offset, List<byte> bytes)
    {
      List<byte> data = bytes.GetRange(offset, bytes.Count - offset);
      int index;

      _ItemName = ByteConverter.ToStringFromUInt16Index(index, data);
      index += _ItemName.Length + 2;

      _GroupId = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _ItemId = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _ItemType = (SSIItemType)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _AdditionalDataIndex = offset + index;
    }
  }

  public enum SSIItemType
  {
    BuddyRecord = 0x0,
    GroupRecord = 0x1,
    PermitRecord = 0x2,
    DenyRecord = 0x3,
    PermitDenySettings = 0x4,
    PresenceInfo = 0x5,
    IgnoreListRecord = 0xe,
    LastUpdateDate = 0xf,
    NonICQContact = 0x10,
    RosterImportTime = 0x13,
    OwnIconAvatarInfo = 0x14
  }

  public abstract class SSIRecord : ISerializable
  {
    public SSIRecord(SSIItemType type)
    {
      _ItemType = type;
    }

    private string _ItemName;
    public string ItemName {
      get { return _ItemName; }
      set {
        if (value == "143979279")
          Debugger.Break();
        _ItemName = value;
      }
    }

    private int _GroupId;
    public int GroupId {
      get { return _GroupId; }
      set { _GroupId = value; }
    }

    private int _ItemId;
    public int ItemId {
      get { return _ItemId; }
      set { _ItemId = value; }
    }

    private SSIItemType _ItemType;
    public SSIItemType ItemType {
      get { return _ItemType; }
      set { _ItemType = value; }
    }

    private int _DataSize;
    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public int ISerializable.TotalSize {
      get { return SizeFixPart + DataSize; }
    }

    private bool _HasData;
    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    protected int SizeFixPart {
      get { return 2 + _ItemName.Length + 2 + 2 + 2 + 2; }
    }

    public abstract int ISerializable.CalculateDataSize();

    public int ISerializable.CalculateTotalSize()
    {
      return SizeFixPart + CalculateDataSize();
    }


    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int index;

      _ItemName = ByteConverter.ToStringFromUInt16Index(index, data);
      index += _ItemName.Length + 2;

      _GroupId = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _ItemId = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _ItemType = (SSIItemType)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;
    }

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data = new List<byte>();
      int dataSize;

      dataSize = CalculateDataSize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ItemName.Length));
      data.AddRange(ByteConverter.GetBytes(_ItemName));
      data.AddRange(ByteConverter.GetBytes((ushort)_GroupId));
      data.AddRange(ByteConverter.GetBytes((ushort)_ItemId));
      data.AddRange(ByteConverter.GetBytes((ushort)_ItemType));
      data.AddRange(ByteConverter.GetBytes((ushort)dataSize));

      return data;
    }

  }

  public class SSIBuddyRecord : SSIRecord
  {
    public SSIBuddyRecord() : base(SSIItemType.BuddyRecord)
    {
    }

    public override int CalculateDataSize()
    {
      return 0;
    }

    private bool _AwaitingAuthorization;
    public bool AwaitingAuthorization {
      get { return _AwaitingAuthorization; }
      set { _AwaitingAuthorization = value; }
    }

    private TlvLocalScreenName _LocalScreenName = new TlvLocalScreenName();
    public TlvLocalScreenName LocalScreenName {
      get { return _LocalScreenName; }
    }

    private TlvLocalEmailAddress _LocalEmailAddress = new TlvLocalEmailAddress();
    public TlvLocalEmailAddress LocalEmailAddress {
      get { return _LocalEmailAddress; }
    }

    private TlvLocalSmsNumber _LocalSmsNumber = new TlvLocalSmsNumber();
    public TlvLocalSmsNumber LocalSmsNumber {
      get { return _LocalSmsNumber; }
    }

    private TlvBuddyCommentField _Comment = new TlvBuddyCommentField();
    public TlvBuddyCommentField Comment {
      get { return _Comment; }
    }

    private TlvPersonalBuddyAlerts _PersonalAlerts = new TlvPersonalBuddyAlerts();
    public TlvPersonalBuddyAlerts PersonalAlerts {
      get { return _PersonalAlerts; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      return base.Serialize;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = this.SizeFixPart;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x66:
            _AwaitingAuthorization = true;
            break;
          case 0x131:
            _LocalScreenName.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x137:
            _LocalEmailAddress.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x13a:
            _LocalSmsNumber.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x13c:
            _Comment.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x13d:
            _PersonalAlerts.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }
    }
  }

  public enum PrivacySetting : byte
  {
    AllowAllUsersToSeeMe = 1,
    BlockAllUsersFromSeeingMe = 2,
    AllowOnlyUsersInPermitList = 3,
    BlockOnlyUsersInDenyList = 4,
    AllowOnlyUsersInBuddyList = 5
  }

  public class TlvPrivacySetting : Tlv
  {
    public TlvPrivacySetting() : base(0xca)
    {
    }

    private PrivacySetting _PrivacySetting;
    public PrivacySetting PrivacySetting {
      get { return _PrivacySetting; }
      set { _PrivacySetting = value; }
    }

    public override int CalculateDataSize()
    {
      return 1;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _PrivacySetting = (PrivacySetting)data(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();
      data.Add((byte)_PrivacySetting);
      return data;
    }
  }

  public class TlvAllowOtherToSee : Tlv
  {
    public TlvAllowOtherToSee() : base(0xcc)
    {
    }

    private AllowOtherToSeeSetting _AllowOtherToSeeSetting;
    public AllowOtherToSeeSetting AllowOtherToSeeSetting {
      get { return _AllowOtherToSeeSetting; }
      set { _AllowOtherToSeeSetting = value; }
    }

    public override int CalculateDataSize()
    {
      return 4;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _AllowOtherToSeeSetting = (AllowOtherToSeeSetting)ByteConverter.ToUInt32(data.GetRange(index, 4));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();
      data.AddRange(ByteConverter.GetBytes((uint)_AllowOtherToSeeSetting));
      return data;
    }
  }

  [Flags()]
  public enum AllowOtherToSeeSetting : uint
  {
    DoNotAllowOthersToSeeThatIAmUsingAWirelessDevice = 0x2,
    AllowOthersToSeeMyIdleTime = 0x400,
    AllowOthersToSeeThatIAmTypingAResponse = 0x400000
  }

  public class TlvBuddyListImportTime : Tlv
  {
    public TlvBuddyListImportTime() : base(0xd4)
    {
    }

    private System.DateTime _ImportTime;
    public System.DateTime ImportTime {
      get { return _ImportTime; }
      set { _ImportTime = value; }
    }

    public override int CalculateDataSize()
    {
      return 4;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ImportTime = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();
      data.AddRange(ByteConverter.GetBytesForUInt32Date(_ImportTime));
      return data;
    }
  }

  public class TlvLocalScreenName : Tlv
  {
    public TlvLocalScreenName() : base(0x131)
    {
    }

    private string _LocalScreenName;
    public string LocalScreenName {
      get { return _LocalScreenName; }
      set { _LocalScreenName = value; }
    }

    public override int CalculateDataSize()
    {
      if (string.IsNullOrEmpty(_LocalScreenName))
        return 0;

      return _LocalScreenName.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      if (!string.IsNullOrEmpty(_LocalScreenName)) {
        data.AddRange(ByteConverter.GetBytes(_LocalScreenName));
      }

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _LocalScreenName = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvLocalEmailAddress : Tlv
  {
    public TlvLocalEmailAddress() : base(0x137)
    {
    }

    private string _LocalEmailAddress;
    public string LocalEmailAddress {
      get { return _LocalEmailAddress; }
      set { _LocalEmailAddress = value; }
    }

    public override int CalculateDataSize()
    {
      return _LocalEmailAddress.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_LocalEmailAddress));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _LocalEmailAddress = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvLocalSmsNumber : Tlv
  {
    public TlvLocalSmsNumber() : base(0x13a)
    {
    }

    private string _LocalSmsNumber;
    public string LocalSmsNumber {
      get { return _LocalSmsNumber; }
      set { _LocalSmsNumber = value; }
    }

    public override int CalculateDataSize()
    {
      return _LocalSmsNumber.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_LocalSmsNumber));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _LocalSmsNumber = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvBuddyCommentField : Tlv
  {
    public TlvBuddyCommentField() : base(0x13c)
    {
    }

    private string _Comment;
    public string Comment {
      get { return _Comment; }
      set { _Comment = value; }
    }

    public override int CalculateDataSize()
    {
      return _Comment.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_Comment));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _Comment = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvPersonalBuddyAlerts : Tlv
  {
    public TlvPersonalBuddyAlerts() : base(0x13d)
    {
    }

    private BuddyAlertType _AlertType;
    public BuddyAlertType AlertType {
      get { return _AlertType; }
      set { _AlertType = value; }
    }


    private BuddyAlert _Alert;
    public BuddyAlert Alert {
      get { return _Alert; }
      set { _Alert = value; }
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();
      data.Add(_AlertType);
      data.Add(_Alert);
      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _AlertType = (BuddyAlertType)data(index);
      _Alert = (BuddyAlert)data(index + 1);
    }
  }

  public enum BuddyAlertType : byte
  {
    PopUpWindow = 0x1,
    PlaySound = 0x2
  }

  public enum BuddyAlert : byte
  {
    WhenContactComesOnline = 0x1,
    WhenContactBecomesUnidle = 0x2,
    WhenContactReturnsFromAway = 0x4
  }

  public class SSIGroupRecord : SSIRecord
  {
    public SSIGroupRecord() : base(SSIItemType.GroupRecord)
    {
    }

    public override int CalculateDataSize()
    {
      return _InnerItems.TotalSize;
    }

    private bool _IsMasterGroup;
    public bool IsMasterGroup {
      get { return _IsMasterGroup; }
      set { _IsMasterGroup = value; }
    }

    private TlvSSIInnerItems _InnerItems = new TlvSSIInnerItems();
    public TlvSSIInnerItems InnerItems {
      get { return _InnerItems; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = this.SizeFixPart;

      if (GroupId == 0) {
        _IsMasterGroup = true;
      }

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0xc8:
            _InnerItems.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(_InnerItems.Serialize);

      return data;
    }
  }

  public class SSIRosterImportTime : SSIRecord
  {
    public SSIRosterImportTime() : base(SSIItemType.RosterImportTime)
    {
    }

    public override int CalculateDataSize()
    {
      return _ImportTime.TotalSize;
    }

    private TlvBuddyListImportTime _ImportTime = new TlvBuddyListImportTime();
    public TlvBuddyListImportTime ImportTime {
      get { return _ImportTime; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = this.SizeFixPart;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0xd4:
            _ImportTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public class SSIPermitDenySettings : SSIRecord
  {
    public SSIPermitDenySettings() : base(SSIItemType.PermitDenySettings)
    {
    }

    public override int CalculateDataSize()
    {
      return _PrivacySetting.TotalSize + _AllowOthersToSee.TotalSize;
    }

    private TlvPrivacySetting _PrivacySetting = new TlvPrivacySetting();
    public TlvPrivacySetting PrivacySetting {
      get { return _PrivacySetting; }
    }

    private TlvAllowOtherToSee _AllowOthersToSee = new TlvAllowOtherToSee();
    public TlvAllowOtherToSee AllowOthersToSee {
      get { return _AllowOthersToSee; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = this.SizeFixPart;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0xca:
            _PrivacySetting.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xcc:
            _AllowOthersToSee.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public class SSIPermitRecord : SSIRecord
  {
    public SSIPermitRecord() : base(SSIItemType.PermitRecord)
    {
    }

    public override int CalculateDataSize()
    {
      return 0;
    }
  }

  public class SSIDenyRecord : SSIRecord
  {
    public SSIDenyRecord() : base(SSIItemType.DenyRecord)
    {
    }

    public override int CalculateDataSize()
    {
      return 0;
    }
  }

  public class SSIIgnoreListRecord : SSIRecord
  {
    public SSIIgnoreListRecord() : base(SSIItemType.IgnoreListRecord)
    {
    }

    public override int CalculateDataSize()
    {
      return 0;
    }
  }

  public class TlvSSIInnerItems : Tlv
  {
    public TlvSSIInnerItems() : base(0xc8)
    {
    }

    public override int CalculateDataSize()
    {
      return _InnerItems.Count * 2;
    }

    private List<int> _InnerItems = new List<int>();
    public List<int> InnerItems {
      get { return _InnerItems; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      while (index < data.Count) {
        _InnerItems.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

        index += 2;
      }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      foreach (int x in _InnerItems) {
        data.AddRange(ByteConverter.GetBytes((ushort)x));
      }

      return data;
    }
  }

  public class SSIBuddyIcon : SSIRecord
  {
    public SSIBuddyIcon() : base(SSIItemType.OwnIconAvatarInfo)
    {
      ItemName = "1";
      //ItemId = &H4DC8
    }

    public override int CalculateDataSize()
    {
      return LocalScreenName.CalculateTotalSize + BuddyIcon.CalculateTotalSize;
    }

    private TlvBuddyIcon _BuddyIcon = new TlvBuddyIcon();
    public TlvBuddyIcon BuddyIcon {
      get { return _BuddyIcon; }
    }

    private TlvLocalScreenName _LocalScreenName = new TlvLocalScreenName();
    public TlvLocalScreenName LocalScreenName {
      get { return _LocalScreenName; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = this.SizeFixPart;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0xd5:
            _BuddyIcon.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data;

      data = base.Serialize();

      data.AddRange(LocalScreenName.Serialize);

      data.AddRange(BuddyIcon.Serialize);

      return data;
    }
  }

  public class TlvBuddyIcon : Tlv
  {
    public TlvBuddyIcon() : base(0xd5)
    {
    }

    public override int CalculateDataSize()
    {
      return 2 + IconHash.Count;
    }

    private List<byte> _IconHash = new List<byte>();
    public List<byte> IconHash {
      get { return _IconHash; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      int length = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      //_IconHash.AddRange(data.GetRange(index, length))
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data;

      data = base.Serialize;

      // MD5 Hash Size
      data.AddRange(ByteConverter.GetBytes((ushort)IconHash.Count));

      // MD5 Hash
      data.AddRange(IconHash);

      return data;
    }
  }
}

