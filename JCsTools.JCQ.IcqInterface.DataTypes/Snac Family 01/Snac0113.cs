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
  public class Snac0113 : Snac
  {
    int _tlvsSize;

    public Snac0113() : base(0x1, 0x13)
    {
    }

    private MessageOfTheDayType _MessageOfTheDayType;
    public MessageOfTheDayType MessageOfTheDayType {
      get { return _MessageOfTheDayType; }
      set { _MessageOfTheDayType = value; }
    }

    private TlvMessageOfTheDay _MessageOfTheDay = new TlvMessageOfTheDay();
    public TlvMessageOfTheDay MessageOfTheDay {
      get { return _MessageOfTheDay; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _MessageOfTheDayType = (MessageOfTheDayType)(byte)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (index + 4 <= data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0xb:
            _MessageOfTheDay.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        _tlvsSize += desc.TotalSize;
        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      return 2 + _tlvsSize;
    }
  }

  public enum MessageOfTheDayType : byte
  {
    MandatoryUpgradeNeededNotice = 0x1,
    AdvisableUpgradeNotice = 0x2,
    AIMICQServiceSystemAnnouncements = 0x3,
    StandartNotice = 0x4,
    SomeNewsFromAOLService = 0x6
  }

  public class TlvMessageOfTheDay : Tlv
  {
    public TlvMessageOfTheDay() : base(0xb)
    {
    }

    private string _Message;
    public string Message {
      get { return _Message; }
      set { _Message = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_Message));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _Message = ByteConverter.ToString(data.GetRange(index, DataSize));
    }

    public override int CalculateDataSize()
    {
      return _Message.Length;
    }
  }
}

