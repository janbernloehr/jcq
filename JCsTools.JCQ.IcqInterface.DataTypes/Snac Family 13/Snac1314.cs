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
  public class Snac1314 : Snac
  {
    public Snac1314() : base(0x13, 0x14)
    {
    }

    public override int CalculateDataSize()
    {
      return 1 + _Uin.Length + 2 + _Message.Length + 2;
    }

    private string _Uin;
    public string Uin {
      get { return _Uin; }
      set { _Uin = value; }
    }

    private string _Message;
    public string Message {
      get { return _Message; }
      set { _Message = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.Add((byte)_Uin.Length);
      data.AddRange(ByteConverter.GetBytes(_Uin));

      data.AddRange(ByteConverter.GetBytes((ushort)_Message.Length));
      data.AddRange(ByteConverter.GetBytes(_Message));

      data.AddRange(ByteConverter.GetBytes((ushort)0));

      return data;
    }
  }
}

