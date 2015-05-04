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
  public class Snac1303 : Snac
  {
    public Snac1303() : base(0x13, 0x3)
    {
    }

    private TlvMaxItemNumbers _MaxItemNumbers = new TlvMaxItemNumbers();
    public TlvMaxItemNumbers MaxItemNumbers {
      get { return _MaxItemNumbers; }
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x4:
            _MaxItemNumbers.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

  }

  public class TlvMaxItemNumbers : Tlv
  {
    public TlvMaxItemNumbers() : base(0x4)
    {
    }

    public override int CalculateDataSize()
    {

    }


    private int _MaxContacts;
    public int MaxContacts {
      get { return _MaxContacts; }
      set { _MaxContacts = value; }
    }

    private int _MaxGroups;
    public int MaxGroups {
      get { return _MaxGroups; }
      set { _MaxGroups = value; }
    }

    private int _MaxVisibleContacts;
    public int MaxVisibleContacts {
      get { return _MaxVisibleContacts; }
      set { _MaxVisibleContacts = value; }
    }

    private int _MaxInvisibleContacts;
    public int MaxInvisibleContacts {
      get { return _MaxInvisibleContacts; }
      set { _MaxInvisibleContacts = value; }
    }

    private int _MaxVisibleInvisibleBitmasks;
    public int MaxVisibleInvisibleBitmasks {
      get { return _MaxVisibleInvisibleBitmasks; }
      set { _MaxVisibleInvisibleBitmasks = value; }
    }

    private int _MaxPresenseInfoFields;
    public int MaxPresenseInfoFields {
      get { return _MaxPresenseInfoFields; }
      set { _MaxPresenseInfoFields = value; }
    }

    private int _MaxIgnoreListEntries;
    public int MaxIgnoreListEntries {
      get { return _MaxIgnoreListEntries; }
      set { _MaxIgnoreListEntries = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _MaxContacts = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MaxGroups = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MaxVisibleContacts = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MaxInvisibleContacts = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _MaxPresenseInfoFields = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      index += 2 * 8;

      _MaxIgnoreListEntries = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

  }
}

