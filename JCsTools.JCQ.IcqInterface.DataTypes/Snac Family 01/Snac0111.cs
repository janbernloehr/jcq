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
  public class Snac0111 : Snac
  {
    public Snac0111() : base(0x1, 0x11)
    {
    }

    private TimeSpan _IdleTime;
    public TimeSpan IdleTime {
      get { return _IdleTime; }
      set { _IdleTime = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _IdleTime = TimeSpan.FromSeconds(ByteConverter.ToUInt32(data.GetRange(index, 4)));
      index += 4;

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)_IdleTime.TotalSeconds));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 4;
    }
  }
}

