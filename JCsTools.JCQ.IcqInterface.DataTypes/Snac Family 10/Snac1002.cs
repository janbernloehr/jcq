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
  public class Snac1002 : Snac
  {
    public Snac1002() : base(0x10, 0x2)
    {
    }

    // xx xx\t \tword\t \treference number
    // xx xx\t \tword\t \ticon length
    // xx ..\t \tarray\t \ticon data (jpg, gif, bmp, etc...)

    public override int CalculateDataSize()
    {
      return 2 + 2 + IconData.Count;
    }

    private int _ReferenceNumber;
    public int ReferenceNumber {
      get { return _ReferenceNumber; }
      set { _ReferenceNumber = value; }
    }

    private List<byte> _IconData = new List<byte>();
    public List<byte> IconData {
      get { return _IconData; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      object data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)ReferenceNumber));
      data.AddRange(ByteConverter.GetBytes((ushort)IconData.Count));
      data.AddRange(IconData);

      return data;
    }
  }
}

