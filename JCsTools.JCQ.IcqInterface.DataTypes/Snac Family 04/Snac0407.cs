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
  public class Snac0407 : Snac
  {
    public Snac0407() : base(0x4, 0x7)
    {
    }

    public override int CalculateDataSize()
    {

    }

    private decimal _CookieID;
    public decimal CookieID {
      get { return _CookieID; }
      set { _CookieID = value; }
    }

    private MessageChannel _Channel;
    public MessageChannel Channel {
      get { return _Channel; }
      set { _Channel = value; }
    }

    private string _ScreenName;
    public string ScreenName {
      get { return _ScreenName; }
      set { _ScreenName = value; }
    }

    private int _SenderWarningLevel;
    public int SenderWarningLevel {
      get { return _SenderWarningLevel; }
    }


    private bool _AutoResponseFlag;
    public bool AutoResponseFlag {
      get { return _AutoResponseFlag; }
    }

    private TlvUserClass _UserClass = new TlvUserClass();
    public TlvUserClass UserClass {
      get { return _UserClass; }
    }

    private TlvUserStatus _UserStatus = new TlvUserStatus();
    public TlvUserStatus UserStatus {
      get { return _UserStatus; }
    }

    private TlvClientIdleTime _ClientIdleTime = new TlvClientIdleTime();
    public TlvClientIdleTime ClientIdleTime {
      get { return _ClientIdleTime; }
    }

    private TlvAccountCreationTime _AccountCreationTime = new TlvAccountCreationTime();
    public TlvAccountCreationTime AccountCreationTime {
      get { return _AccountCreationTime; }
    }

    private TlvMessageData _MessageData = new TlvMessageData();
    public TlvMessageData MessageData {
      get { return _MessageData; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      TlvDescriptor desc;

      _CookieID = ByteConverter.ToUInt64(data.GetRange(index, 8));
      index += 8;

      _Channel = (MessageChannel)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _ScreenName = ByteConverter.ToStringFromByteIndex(index, data);
      index += 1 + _ScreenName.Length;

      _SenderWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      int innerTlvCount;
      int innerTlvIndex;

      innerTlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (innerTlvIndex < innerTlvCount) {
        desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x1:
            _UserClass.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x6:
            _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xf:
            _ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x3:
            _AccountCreationTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x4:
            _AutoResponseFlag = true;
            break;
        }

        index += desc.TotalSize;
        innerTlvIndex += 1;
      }

      desc = TlvDescriptor.GetDescriptor(index, data);

      _MessageData.Deserialize(data.GetRange(index, desc.TotalSize));
      index += _MessageData.TotalSize;

      while (index + 4 <= data.Count) {
        desc = TlvDescriptor.GetDescriptor(index, data);

        index += desc.TotalSize;
      }

      this.SetTotalSize(index);
    }
  }

  public class TlvAccountCreationTime : Tlv
  {
    public TlvAccountCreationTime() : base(0x5)
    {
    }

    private DateTime _AccountCreationTime;
    public DateTime AccountCreationTime {
      get { return _AccountCreationTime; }
      set { _AccountCreationTime = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytesForUInt32Date(_AccountCreationTime));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _AccountCreationTime = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
    }

    public override int CalculateDataSize()
    {
      return 4;
    }
  }
}

