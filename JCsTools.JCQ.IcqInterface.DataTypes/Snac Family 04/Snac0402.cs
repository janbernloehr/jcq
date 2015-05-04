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
  public class Snac0402 : Snac
  {
    public Snac0402() : base(0x4, 0x2)
    {
    }

    public override int CalculateDataSize()
    {
      return 2 + 4 + 2 + 2 + 2 + 2 + 2;
    }

    private int _Channel;
    public int Channel {
      get { return _Channel; }
      set { _Channel = value; }
    }


    private int _MessageFlags;
    public int MessageFlags {
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
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_Channel));
      data.AddRange(ByteConverter.GetBytes((uint)_MessageFlags));
      data.AddRange(ByteConverter.GetBytes((ushort)_MaxMessageSnacSize));
      data.AddRange(ByteConverter.GetBytes((ushort)_MaxSenderWarningLevel));
      data.AddRange(ByteConverter.GetBytes((ushort)_MaxReceiverWarningLevel));
      data.AddRange(ByteConverter.GetBytes((ushort)_MinimumMessageInterval));
      data.AddRange(ByteConverter.GetBytes((ushort)0));

      return data;
    }


  }
}

