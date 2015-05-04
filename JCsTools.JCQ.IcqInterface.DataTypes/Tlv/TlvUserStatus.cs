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
  public class TlvUserStatus : Tlv
  {
    public TlvUserStatus() : base(0x6)
    {
    }

    private UserFlag _UserFlag;
    public UserFlag NewProperty {
      get { return _UserFlag; }
      set { _UserFlag = value; }
    }

    private UserStatus _UserStatus;
    public UserStatus UserStatus {
      get { return _UserStatus; }
      set { _UserStatus = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_UserFlag));
      data.AddRange(ByteConverter.GetBytes((ushort)_UserStatus));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _UserFlag = (UserFlag)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      _UserStatus = (UserStatus)(int)ByteConverter.ToUInt16(data.GetRange(index + 2, 2));
    }

    public override int CalculateDataSize()
    {
      return 4;
    }
  }

  [Flags()]
  public enum UserFlag
  {
    StatusShowIpFlag = 0x2,
    UserBirthdayFlag = 0x8,
    UserActiveWebfrontFlag = 0x20,
    DirectConnectionNotSupported = 0x100,
    DirectConnectionUponAuthorization = 0x1000,
    OnlyWithContactUsers = 0x2000
  }

  [Flags()]
  public enum UserStatus : int
  {
    Online = 0x0,
    Away = 0x1,
    DoNotDisturb = 0x2,
    NotAvailable = 0x4,
    Occupied = 0x10,
    FreeForChat = 0x20,
    Invisible = 0x100,
    Offline
  }
}

