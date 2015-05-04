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
  public class Snac0405 : Snac
  {
    public Snac0405() : base(0x4, 0x5)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    private int _Channel;
    public int Channel {
      get { return _Channel; }
      set { _Channel = value; }
    }


    private long _MessageFlags;
    public long MessageFlags {
      get { return _MessageFlags; }
      set { _MessageFlags = value; }
    }


    private int _MaxMessageSnacSize;
    public int MaxMessageSnacSize {
      get { return _MaxMessageSnacSize; }
      set { _MaxMessageSnacSize = value; }
    }


    private int _MaxSenderWarningLevel;
    public int MaxSenderWarningLevel {
      get { return _MaxSenderWarningLevel; }
      set { _MaxSenderWarningLevel = value; }
    }


    private int _MaxReceiverWarningLevel;
    public int MaxReceiverWarningLevel {
      get { return _MaxReceiverWarningLevel; }
      set { _MaxReceiverWarningLevel = value; }
    }


    private int _MinimumMessageInterval;
    public int MinimumMessageInterval {
      get { return _MinimumMessageInterval; }
      set { _MinimumMessageInterval = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _Channel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MessageFlags = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _MaxMessageSnacSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MaxSenderWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MaxReceiverWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MinimumMessageInterval = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      //unknown param
      index += 2;

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }
}

