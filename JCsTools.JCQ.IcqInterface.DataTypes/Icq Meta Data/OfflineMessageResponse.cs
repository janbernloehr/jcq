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
  public class OfflineMessageResponse : MetaResponse
  {
    public OfflineMessageResponse() : base(MetaResponseType.OfflineMessageResponse)
    {
    }

    public override int CalculateDataSize()
    {
      //Return 2 + 4 + 2 + 2 + 4 + 6 + 1 + 1 + 2 + _MessageText.Length
      throw new NotImplementedException();
    }

    private long _SenderUin;
    public long SenderUin {
      get { return _SenderUin; }
      set { _SenderUin = value; }
    }

    private DateTime _DateSent;
    public DateTime DateSent {
      get { return _DateSent; }
      set { _DateSent = value; }
    }

    private string _MessageText;
    public string MessageText {
      get { return _MessageText; }
      set { _MessageText = value; }
    }

    private MessageType _MessageType;
    public MessageType MessageType {
      get { return _MessageType; }
      set { _MessageType = value; }
    }

    private MessageFlag _MessageFlags;
    public MessageFlag MessageFlags {
      get { return _MessageFlags; }
      set { _MessageFlags = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = MetaResponse.SizeFixPart;

      _SenderUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4));
      index += 4;

      int year;
      int month;
      int day;
      int hour;
      int minute;

      year = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
      index += 2;

      month = data(index);
      index += 1;

      day = data(index);
      index += 1;

      hour = data(index);
      index += 1;

      minute = data(index);
      index += 1;

      _DateSent = new System.DateTime(year, month, day, hour, minute, 0);

      _MessageType = (DataTypes.MessageType)data(index);
      index += 1;

      _MessageFlags = (MessageFlag)data(index);
      index += 1;

      _MessageText = ByteConverter.ToStringFromUInt16LEIndex(index, data);
      if (_MessageText.EndsWith(Chr(0)))
        _MessageText = _MessageText.Substring(0, _MessageText.Length - 1);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public enum MessageType : byte
  {
    PlainTextMessage = 0x1,
    ChatRequestMessage = 0x2,
    FileRequestMessage = 0x3,
    URLMessage = 0x4,
    AuthorizationRequestMessage = 0x6,
    AuthorizationDeniedMessage = 0x7,
    AuthorizationGivenMessage = 0x8,
    MessageFromOSCARServer = 0x9,
    YouWereAddedMessage = 0xc,
    WebPagerMessage = 0xd,
    EmailExpressMessage = 0xe,
    ContactListMessage = 0x13,
    PluginMessage = 0x1a,
    AutoAwayMessage = 0xe8,
    AutoOccupiedMessage = 0xe9,
    AutoNotAvailableMessage = 0xea,
    AutoDoNotDisturbMessage = 0xeb,
    AutoFreeForChatMessage = 0xec
  }

  public enum MessageFlag : byte
  {
    NormalMessage = 0x1,
    AutoMessage = 0x3,
    MultipleRecipientsMessage = 0x80
  }
}

