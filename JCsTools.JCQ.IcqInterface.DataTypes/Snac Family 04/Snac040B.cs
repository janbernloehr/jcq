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
  public class Snac040B : Snac
  {
    public Snac040B() : base(0x4, 0xb)
    {
    }

    public override int CalculateDataSize()
    {
      return 8 + 2 + 1 + _ScreenName.Length + 2 + 2 + 2 + _RequiredCapabilities.Count + 2 + 2 + 2 + 2 + _MessageText.Length;
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

    private int _AckReasonCode;
    public int AckReasonCode {
      get { return _AckReasonCode; }
      set { _AckReasonCode = value; }
    }

    private List<byte> _RequiredCapabilities = new List<byte>();
    public List<byte> RequiredCapabilities {
      get { return _RequiredCapabilities; }
    }

    private string _MessageText;
    public string MessageText {
      get { return _MessageText; }
      set { _MessageText = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ulong)_CookieID));
      data.AddRange(ByteConverter.GetBytes((ushort)_Channel));
      data.Add((byte)_ScreenName.Length);
      data.AddRange(ByteConverter.GetBytes(_ScreenName));

      data.AddRange(ByteConverter.GetBytes((ushort)_AckReasonCode));

      // fragment identifier
      data.Add(0x5);
      // fragment version
      data.Add(0x1);

      data.AddRange(ByteConverter.GetBytes((ushort)_RequiredCapabilities.Count));
      data.AddRange(_RequiredCapabilities);

      // fragment identifier
      data.Add(0x1);
      // fragment version
      data.Add(0x1);

      data.AddRange(ByteConverter.GetBytes((ushort)_MessageText.Length + 2 + 2));
      data.AddRange(new byte[] {
        0x0,
        0x0,
        0xff,
        0xff
      });

      data.AddRange(ByteConverter.GetBytes(_MessageText));

      return data;
    }
  }
}

