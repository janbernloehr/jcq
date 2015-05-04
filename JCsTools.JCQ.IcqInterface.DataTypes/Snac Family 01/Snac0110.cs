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
  public class Snac0110 : Snac
  {
    public Snac0110() : base(0x1, 0x10)
    {
    }

    private int _NewWarningLevel;
    public int NewWarningLevel {
      get { return _NewWarningLevel; }
      set { _NewWarningLevel = value; }
    }

    private List<UserInfo> _UserInfos = new List<UserInfo>();
    public List<UserInfo> UserInfos {
      get { return _UserInfos; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _NewWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (index < data.Count) {
        UserInfo info;

        info = new UserInfo();
        index += info.Deserialize(index, data);

        UserInfos.Add(info);
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      int size = 2;
      foreach (UserInfo x in UserInfos) {
        size += x.CalculateTotalSize;
      }
      return size;
    }
  }

  public class TlvOnlineTime : Tlv
  {
    public TlvOnlineTime() : base(0xf)
    {
    }

    private DateTime _OnlineTime;
    public DateTime OnlineTime {
      get { return _OnlineTime; }
      set { _OnlineTime = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytesForUInt32Date(_OnlineTime));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _OnlineTime = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
    }

    public override int CalculateDataSize()
    {
      return 4;
    }
  }
}

