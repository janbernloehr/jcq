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
  public class UserInfo : ISerializable
  {
    private int _DataSize;
    private bool _HasData;

    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public int ISerializable.TotalSize {
      get { return _DataSize; }
    }

    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public int ISerializable.CalculateDataSize()
    {
      return _UserClass.CalculateDataSize + _DCInfo.CalculateDataSize + _ExternalAddress.CalculateDataSize + _UserStatus.CalculateDataSize + _UserCapabilities.CalculateDataSize + _OnlineTime.CalculateDataSize + _SignOnTime.CalculateDataSize + _MemberSince.CalculateDataSize + _UserIconIdAndHash.CalculateDataSize;
    }

    public int ISerializable.CalculateTotalSize()
    {
      return CalculateDataSize();
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

    private TlvSignonTime _SignOnTime = new TlvSignonTime();
    public TlvSignonTime SignOnTime {
      get { return _SignOnTime; }
    }

    private TlvMemberSince _MemberSince = new TlvMemberSince();
    public TlvMemberSince MemberSince {
      get { return _MemberSince; }
    }

    private TlvCapabilities _UserCapabilities = new TlvCapabilities();
    public TlvCapabilities UserCapabilities {
      get { return _UserCapabilities; }
    }

    private TlvOnlineTime _OnlineTime = new TlvOnlineTime();
    public TlvOnlineTime OnlineTime {
      get { return _OnlineTime; }
    }

    private TlvUserIconIdAndHash _UserIconIdAndHash = new TlvUserIconIdAndHash();
    public TlvUserIconIdAndHash UserIconIdAndHash {
      get { return _UserIconIdAndHash; }
    }

    public void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int index = 0;

      if (data(index) == 0x0 & data(index + 1) == 0x6)
        index += 8;

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
          case 0xc:
            _DCInfo.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xa:
            _ExternalAddress.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x6:
            _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xd:
            _UserCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0xf:
            _OnlineTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x3:
            _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x5:
            _MemberSince.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x1d:
            _UserIconIdAndHash.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
        innerTlvIndex += 1;
      }

      _DataSize = index;

      _HasData = true;
    }

    public int Deserialize(int offset, List<byte> data)
    {
      Deserialize(data.GetRange(offset, data.Count - offset));

      return DataSize;
    }

    public System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data;

      data = new List<byte>();

      data.AddRange(_UserClass.Serialize);
      data.AddRange(_DCInfo.Serialize);
      data.AddRange(_ExternalAddress.Serialize);
      data.AddRange(_UserStatus.Serialize);
      data.AddRange(_UserCapabilities.Serialize);
      data.AddRange(_OnlineTime.Serialize);
      data.AddRange(_SignOnTime.Serialize);
      data.AddRange(_MemberSince.Serialize);
      data.AddRange(_UserIconIdAndHash.Serialize);

      return data;
    }

  }
}

