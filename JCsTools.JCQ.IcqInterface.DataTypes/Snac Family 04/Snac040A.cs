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
  public class Snac040A : Snac
  {
    public Snac040A() : base(0x4, 0xa)
    {
    }

    public override int CalculateDataSize()
    {
      int size;
      foreach (MissedMessageInfo x in _MissedMessageInfos) {
        size += x.CalculateTotalSize;
      }
      return size;
    }

    private List<MissedMessageInfo> _MissedMessageInfos = new List<MissedMessageInfo>();
    public List<MissedMessageInfo> MissedMessageInfos {
      get { return _MissedMessageInfos; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index;

      index = Snac.SizeFixPart;

      while (index < data.Count) {
        MissedMessageInfo info;

        info = new MissedMessageInfo();
        index += info.Deserialize(index, data);

        _MissedMessageInfos.Add(info);
      }

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public class MissedMessageInfo : ISerializable
  {
    private MessageChannel _Channel;
    public MessageChannel Channel {
      get { return _Channel; }
      set { _Channel = value; }
    }

    private string _Uin;
    public string Uin {
      get { return _Uin; }
      set { _Uin = value; }
    }

    private int _WarningLevel;
    public int WarningLevel {
      get { return _WarningLevel; }
      set { _WarningLevel = value; }
    }

    private TlvUserClass _UserClass = new TlvUserClass();
    public TlvUserClass UserClass {
      get { return _UserClass; }
    }

    private TlvUserStatus _UserStatus = new TlvUserStatus();
    public TlvUserStatus UserStatus {
      get { return _UserStatus; }
    }

    private TlvOnlineTime _OnlineTime = new TlvOnlineTime();
    public TlvOnlineTime OnlineTime {
      get { return _OnlineTime; }
    }

    private TlvSignonTime _SignOnTime = new TlvSignonTime();
    public TlvSignonTime SignOnTime {
      get { return _SignOnTime; }
    }

    private MissedMessageReason _MissedReason;
    public MissedMessageReason MissedReason {
      get { return _MissedReason; }
      set { _MissedReason = value; }
    }

    private int _MissedMessageCount;
    public int MissedMessageCount {
      get { return _MissedMessageCount; }
      set { _MissedMessageCount = value; }
    }

    public int ISerializable.TotalSize {
      get { return DataSize; }
    }

    private int _DataSize;

    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public int ISerializable.CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public int ISerializable.CalculateTotalSize()
    {
      throw new NotImplementedException();
    }

    private bool _HasData;

    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public int Deserialize(int offset, System.Collections.Generic.List<byte> data)
    {
      Deserialize(data.GetRange(offset, data.Count - offset));

      return DataSize;
    }

    public void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int index;

      _Channel = (MessageChannel)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _Uin = ByteConverter.ToStringFromByteIndex(index, data);
      index += 1 + _Uin.Length;

      _WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      int innerTlvCount;
      int innerTlvIndex;

      innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (innerTlvIndex < innerTlvCount) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x1:
            _UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x6:
            _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xf:
            _OnlineTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x3:
            _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        innerTlvIndex += 1;
        index += desc.TotalSize;
      }

      _MissedMessageCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MissedReason = (MissedMessageReason)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _HasData = true;

      _DataSize = index;
    }

    public System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      throw new NotImplementedException();
      //Dim data As List(Of Byte)
      //data = New List(Of Byte)

      //data.AddRange(ByteConverter.GetBytes(CUShort(_Channel)))
      //data.Add(_Uin.Length)
      //data.AddRange(ByteConverter.GetBytes(_Uin))
      //data.AddRange(ByteConverter.GetBytes(CUShort(_WarningLevel)))
      //data.AddRange(ByteConverter.GetBytes(CUShort(_WarningLevel)))
    }

  }

  public enum MissedMessageReason
  {
    MessageInvalid = 0x0,
    MessageTooLarge = 0x1,
    MessageRateExceeded = 0x2,
    SenderToEvil = 0x3,
    YouAreToEvil = 0x4
  }
}

