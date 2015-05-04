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
  public class Snac1305 : Snac
  {
    public Snac1305() : base(0x13, 0x5)
    {
    }

    private System.DateTime _ModificationDate;
    public System.DateTime ModificationDate {
      get { return _ModificationDate; }
      set { _ModificationDate = value; }
    }

    private int _NumberOfItems;
    public int NumberOfItems {
      get { return _NumberOfItems; }
      set { _NumberOfItems = value; }
    }

    public override int CalculateDataSize()
    {
      return 6;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytesForUInt32Date(_ModificationDate));
      data.AddRange(ByteConverter.GetBytes((ushort)_NumberOfItems));

      return data;
    }
  }
}

