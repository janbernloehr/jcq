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
  public class Snac0406 : Snac
  {
    public Snac0406() : base(0x4, 0x6)
    {
    }

    public override int CalculateDataSize()
    {
      return 8 + 2 + 1 + _ScreenName.Length + _MessageData.CalculateTotalSize + _RequestAnAckFromServer ? 4 : 0 + _StoreMessageIfRecipientIsOffline ? 4 : 0;
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


    private TlvMessageData _MessageData = new TlvMessageData();
    public TlvMessageData MessageData {
      get { return _MessageData; }
    }

    private bool _RequestAnAckFromServer;
    public bool RequestAnAckFromServer {
      get { return _RequestAnAckFromServer; }
      set { _RequestAnAckFromServer = value; }
    }


    private bool _StoreMessageIfRecipientIsOffline;
    public bool StoreMessageIfRecipientIsOffline {
      get { return _StoreMessageIfRecipientIsOffline; }
      set { _StoreMessageIfRecipientIsOffline = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      if (!_Channel == MessageChannel.Channel1PlainText)
        throw new NotImplementedException();

      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ulong)_CookieID));
      data.AddRange(ByteConverter.GetBytes((ushort)_Channel));
      data.Add((byte)_ScreenName.Length);
      data.AddRange(ByteConverter.GetBytes(_ScreenName));

      data.AddRange(_MessageData.Serialize);

      if (_RequestAnAckFromServer) {
        data.AddRange((new TlvRequestAckFromServer()).Serialize);
      }

      if (_StoreMessageIfRecipientIsOffline) {
        data.AddRange((new TlvStoreMessageIfRecipientOffline()).Serialize);
      }

      return data;
    }
  }

  public enum MessageChannel : int
  {
    Channel1PlainText = 1
  }

  public class TlvMessageData : Tlv
  {
    public TlvMessageData() : base(0x2)
    {
    }

    public override int CalculateDataSize()
    {
      return 4 + _RequiredCapabilities.Count + 4 + 4 + _MessageText.Length;
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

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

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

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      index += 2;

      int length;

      length = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _RequiredCapabilities.AddRange(data.GetRange(index, length));
      index += length;

      index += 2;

      length = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      index += 4;

      _MessageText = ByteConverter.ToString(data.GetRange(index, length - 4));

      index += length - 4;
    }
  }

  public class TlvRequestAckFromServer : Tlv
  {
    public TlvRequestAckFromServer() : base(0x3)
    {
    }

    public override int CalculateDataSize()
    {
      return 0;
    }
  }

  public class TlvStoreMessageIfRecipientOffline : Tlv
  {
    public TlvStoreMessageIfRecipientOffline() : base(0x6)
    {
    }

    public override int CalculateDataSize()
    {
      return 0;
    }
  }
}

