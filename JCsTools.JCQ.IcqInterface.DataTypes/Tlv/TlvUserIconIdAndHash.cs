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
  public class TlvUserIconIdAndHash : Tlv
  {
    public TlvUserIconIdAndHash() : base(0x1d)
    {
    }

    public override int CalculateDataSize()
    {
      return 2 + 1 + 1 + _IconMD5Hash.Count;
    }

    private int _IconId;
    public int IconId {
      get { return _IconId; }
      set { _IconId = value; }
    }

    private byte _IconFlags;
    public byte IconFlags {
      get { return _IconFlags; }
      set { _IconFlags = value; }
    }

    private byte _IconHashLength;
    public byte IconHashLenght {
      get { return _IconHashLength; }
      set { _IconHashLength = value; }
    }

    private List<byte> _IconMD5Hash = new List<byte>();
    public List<byte> IconMD5Hash {
      get { return _IconMD5Hash; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _IconId = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _IconFlags = data(index);
      index += 1;

      _IconHashLength = data(index);
      index += 1;

      _IconMD5Hash.AddRange(data.GetRange(index, _IconHashLength));
      index += _IconMD5Hash.Count;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_IconId));
      data.Add(_IconFlags);
      data.Add(_IconHashLength);
      data.AddRange(_IconMD5Hash);

      return data;
    }
  }
}

