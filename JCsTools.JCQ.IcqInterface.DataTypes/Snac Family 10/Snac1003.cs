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
  public class Snac1003 : Snac
  {
    public Snac1003() : base(0x10, 0x3)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    private List<byte> _IconHash = new List<byte>(16);
    public List<byte> IconHash {
      get { return _IconHash; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      //00 00\t \tword\t \tunknown field(s)
      //01 01\t \tword\t \tunknown field(s)
      //10\t \tbyte\t \tsize of the icon md5 checksum
      //xx ..\t \tarray\t \ticon md5 checksum

      object index = SizeFixPart;

      byte hashLenght;

      index += 2;
      index += 2;

      hashLenght = data(index);
      index += 1;

      _IconHash.AddRange(data.GetRange(index, hashLenght));
      index += hashLenght;

      SetTotalSize(index);
    }
  }
}

