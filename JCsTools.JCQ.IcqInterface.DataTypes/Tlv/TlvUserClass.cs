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
  public class TlvUserClass : Tlv
  {
    public TlvUserClass() : base(0x1)
    {
    }

    private UserClass _UserClass;
    public UserClass UserClass {
      get { return _UserClass; }
      set { _UserClass = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)_UserClass));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _UserClass = (UserClass)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  [Flags()]
  public enum UserClass
  {
    AOLUnconfirmedUserFlag = 0x1,
    AOLAdministratorFlag = 0x2,
    AOLStaffUserFlag = 0x4,
    AOLCommercialAccountFlag = 0x8,
    ICQNonCommercialAccountFlag = 0x10,
    AwayStatusFlag = 0x20,
    ICQUserSign = 0x40,
    AOLWirelessUser = 0x80
  }
}

