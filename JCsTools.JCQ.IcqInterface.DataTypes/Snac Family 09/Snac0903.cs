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
  public class Snac0903 : Snac
  {
    public Snac0903() : base(0x9, 0x3)
    {
    }

    private TlvMaxVisibleListSize _MaxVisibleListSize = new TlvMaxVisibleListSize();
    public TlvMaxVisibleListSize MaxVisibleListSize {
      get { return _MaxVisibleListSize; }
    }

    private TlvMaxInvisibleListSize _MaxInvisibleListSize = new TlvMaxInvisibleListSize();
    public TlvMaxInvisibleListSize MaxInvisibleListSize {
      get { return _MaxInvisibleListSize; }
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
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
          case 0x1:
            _MaxVisibleListSize.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x2:
            _MaxInvisibleListSize.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      this.SetTotalSize(index);
    }
  }

  public class TlvMaxVisibleListSize : Tlv
  {
    public TlvMaxVisibleListSize() : base(0x1)
    {
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    private int _MaxNumberOfVisibleListEntries;
    public int MaxNumberOfVisibleListEntries {
      get { return _MaxNumberOfVisibleListEntries; }
      set { _MaxNumberOfVisibleListEntries = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _MaxNumberOfVisibleListEntries = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_MaxNumberOfVisibleListEntries));

      return data;
    }
  }

  public class TlvMaxInvisibleListSize : Tlv
  {
    public TlvMaxInvisibleListSize() : base(0x1)
    {
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    private int _MaxNumberOfInvisibleListEntries;
    public int MaxNumberOfInvisibleListEntries {
      get { return _MaxNumberOfInvisibleListEntries; }
      set { _MaxNumberOfInvisibleListEntries = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _MaxNumberOfInvisibleListEntries = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_MaxNumberOfInvisibleListEntries));

      return data;
    }
  }
}

