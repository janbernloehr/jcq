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
  public class Snac0206 : Snac
  {
    public Snac0206() : base(0x2, 0x6)
    {
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

    private TlvDCInfo _DCInfo = new TlvDCInfo();
    public TlvDCInfo DCInfo {
      get { return _DCInfo; }
    }

    private TlvExternalIpAddress _ExternalAddress = new TlvExternalIpAddress();
    public TlvExternalIpAddress ExternalAddress {
      get { return _ExternalAddress; }
    }

    private TlvClientIdleTime _ClientIdleTime = new TlvClientIdleTime();
    public TlvClientIdleTime ClientIdleTime {
      get { return _ClientIdleTime; }
    }

    private TlvSignonTime _SignOnTime = new TlvSignonTime();
    public TlvSignonTime SignOnTime {
      get { return _SignOnTime; }
    }

    private TlvMemberSince _MemberSince = new TlvMemberSince();
    public TlvMemberSince MemberSince {
      get { return _MemberSince; }
    }

    private TlvEncodingType _EncodingType = new TlvEncodingType();
    public TlvEncodingType EncodingType {
      get { return _EncodingType; }
    }

    private TlvClientProfileString _ClientProfileString = new TlvClientProfileString();
    public TlvClientProfileString ClientProfileString {
      get { return _ClientProfileString; }
    }

    private TlvAwayMessageEncoding _AwayMessageEncoding = new TlvAwayMessageEncoding();
    public TlvAwayMessageEncoding AwayMessageEncoding {
      get { return _AwayMessageEncoding; }
    }

    private TlvAwayMessageString _AwayMessage = new TlvAwayMessageString();
    public TlvAwayMessageString AwayMessage {
      get { return _AwayMessage; }
    }

    private TlvCapabilities _UserCapabilities = new TlvCapabilities();
    public TlvCapabilities UserCapabilities {
      get { return _UserCapabilities; }
    }


    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      int innerTlvCount;
      int innerTlvIndex;

      _Uin = ByteConverter.ToStringFromByteIndex(index, data);
      index += 1 + _Uin.Length;

      _WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

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
          case 0xc:
            _DCInfo.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xa:
            _ExternalAddress.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xf:
            _ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x3:
            _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x5:
            _MemberSince.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
        innerTlvIndex += 1;
      }

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x1:
            _EncodingType.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x2:
            _ClientProfileString.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x3:
            _AwayMessageEncoding.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x4:
            _AwayMessage.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x5:
            _UserCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }
  }

  public class TlvEncodingType : Tlv
  {
    public TlvEncodingType() : base(0x1)
    {
    }

    private string _EncodingType;
    public string EncodingType {
      get { return _EncodingType; }
      set { _EncodingType = value; }
    }


    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _EncodingType = ByteConverter.ToStringFromUInt16Index(index, data);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_EncodingType.Length));
      data.AddRange(ByteConverter.GetBytes(_EncodingType));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2 + _EncodingType.Length;
    }
  }

  public class TlvClientProfileString : Tlv
  {
    public TlvClientProfileString() : base(0x2)
    {
    }

    private string _ClientProfile;
    public string ClientProfile {
      get { return _ClientProfile; }
      set { _ClientProfile = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientProfile = ByteConverter.ToStringFromUInt16Index(index, data);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientProfile.Length));
      data.AddRange(ByteConverter.GetBytes(_ClientProfile));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2 + _ClientProfile.Length;
    }
  }

  public class TlvAwayMessageEncoding : Tlv
  {
    public TlvAwayMessageEncoding() : base(0x3)
    {
    }

    private string _AwayMessageEncoding;
    public string AwayMessageEncoding {
      get { return _AwayMessageEncoding; }
      set { _AwayMessageEncoding = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _AwayMessageEncoding = ByteConverter.ToStringFromUInt16Index(index, data);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_AwayMessageEncoding.Length));
      data.AddRange(ByteConverter.GetBytes(_AwayMessageEncoding));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2 + _AwayMessageEncoding.Length;
    }
  }

  public class TlvAwayMessageString : Tlv
  {
    public TlvAwayMessageString() : base(0x4)
    {
    }

    private string _AwayMessage;
    public string AwayMessage {
      get { return _AwayMessage; }
      set { _AwayMessage = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _AwayMessage = ByteConverter.ToStringFromUInt16Index(index, data);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_AwayMessage.Length));
      data.AddRange(ByteConverter.GetBytes(_AwayMessage));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2 + _AwayMessage.Length;
    }
  }

  //TODO: Complete Implementation of Location Services to achieve 100% AIM Support
}

