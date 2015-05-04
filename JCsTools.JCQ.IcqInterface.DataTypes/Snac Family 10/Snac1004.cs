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
  public class Snac1004 : Snac
  {
    public Snac1004() : base(0x10, 0x4)
    {
    }

    public override int CalculateDataSize()
    {
      return 1 + _Uin.Length + 1 + 2 + 1 + 1 + _IconHash.Count;
    }

    private string _Uin;
    public string Uin {
      get { return _Uin; }
      set { _Uin = value; }
    }

    private List<byte> _IconHash = new List<byte>();
    public List<byte> IconHash {
      get { return _IconHash; }
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
      data.Add(0x1);
      data.Add(0x0);
      data.Add(0x1);
      data.Add(0x1);
      data.Add((byte)_IconHash.Count);
      data.AddRange(_IconHash);

      //xx   byte   uin length 
      // xx ..   ascii   uin string 
      // 01   byte   unknown (command ?) 
      // 00 01   word   icon id (not sure) 
      // 01   byte   icon flags (bitmask, purpose unknown) 
      // 10   byte   md5 hash size (16) 
      // xx ..   array   requested icon md5 hash 


      return data;
    }
  }
}

