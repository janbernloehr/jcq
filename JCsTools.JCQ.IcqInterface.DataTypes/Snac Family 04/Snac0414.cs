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
  public class Snac0414 : Snac
  {
    public Snac0414() : base(0x4, 0x14)
    {
    }

    public override int CalculateDataSize()
    {
      return 8 + 2 + 1 + _ScreenName.Length + 2;
    }

    private long _CookieID;
    public long CookieID {
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


    private NotificationType _NotificationType;
    public NotificationType NotificationType {
      get { return _NotificationType; }
      set { _NotificationType = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _CookieID = (long)ByteConverter.ToUInt64(data.GetRange(index, 8));
      index += 8;

      _Channel = (MessageChannel)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _ScreenName = ByteConverter.ToStringFromByteIndex(index, data);
      index += 1 + _ScreenName.Length;

      this._NotificationType = (NotificationType)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ulong)_CookieID));
      data.AddRange(ByteConverter.GetBytes((ushort)_Channel));
      data.Add((byte)_ScreenName.Length);
      data.AddRange(ByteConverter.GetBytes(_ScreenName));

      data.AddRange(ByteConverter.GetBytes((ushort)_NotificationType));

      return data;
    }
  }

  public enum NotificationType
  {
    TextCleared = 0x0,
    TextTyped = 0x1,
    TypingBegun = 0x2
  }
}

