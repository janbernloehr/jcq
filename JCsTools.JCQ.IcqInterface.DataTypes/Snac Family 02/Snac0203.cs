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
  public class Snac0203 : Snac
  {
    public Snac0203() : base(0x2, 0x3)
    {
    }

    private TlvProfileMaxLength _ProfileMaxLenght = new TlvProfileMaxLength();
    public TlvProfileMaxLength TlvProfileMaxLength {
      get { return _ProfileMaxLenght; }
    }

    private TlvMaxCapabilities _MaxCapabilities = new TlvMaxCapabilities();
    public TlvMaxCapabilities MaxCapabilities {
      get { return _MaxCapabilities; }
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
            _ProfileMaxLenght.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x2:
            _MaxCapabilities.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }
  }

  public class TlvProfileMaxLength : Tlv
  {
    public TlvProfileMaxLength() : base(0x1)
    {
    }

    private int _ClientMaxProfileLength;
    public int ClientMaxProfileLength {
      get { return _ClientMaxProfileLength; }
      set { _ClientMaxProfileLength = value; }
    }

    public override int DataSize {
      get {
        if (base.DataSize == 0) {
          return 2;
        } else {
          return base.DataSize;
        }
      }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      _ClientMaxProfileLength = ByteConverter.ToUInt16(data.GetRange(Tlv.SizeFixPart, 2));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientMaxProfileLength));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  public class TlvMaxCapabilities : Tlv
  {
    public TlvMaxCapabilities() : base(0x2)
    {
    }

    private int _MaxCapabilities;
    public int MaxCapabilities {
      get { return _MaxCapabilities; }
      set { _MaxCapabilities = value; }
    }

    public override int DataSize {
      get {
        if (base.DataSize == 0) {
          return 2;
        } else {
          return base.DataSize;
        }
      }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      _MaxCapabilities = ByteConverter.ToUInt16(data.GetRange(Tlv.SizeFixPart, 2));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_MaxCapabilities));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }
}

