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
  public class Snac1005 : Snac
  {
    public Snac1005() : base(0x10, 0x5)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
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

    private List<byte> _IconData = new List<byte>();
    public List<byte> IconData {
      get { return _IconData; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      // xx   byte   uin length 
      // xx ..   ascii   uin string 

      int index = Snac.SizeFixPart;

      _Uin = ByteConverter.ToStringFromByteIndex(index, data);
      index += 1 + _Uin.Length;

      // 00 01   word   icon id (not sure) 
      // 01   byte   icon flags (bitmask, purpose unknown) 

      index += 3;

      // 10   byte   md5 hash size (16) 
      // xx ..   array   requested icon md5 hash 

      byte length = data(index);
      index += 1;

      _IconHash.AddRange(data.GetRange(index, length));
      index += length;

      // xx xx   word   length of the icon 

      int iconLength = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      // xx ..   array   icon data (jfif - jpeg file interchange format) 

      _IconData.AddRange(data.GetRange(index, iconLength));
      index += iconLength;

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }
}

