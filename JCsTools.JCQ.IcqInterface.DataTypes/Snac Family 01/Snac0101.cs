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
/// <summary>
/// Client / server error
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public class Snac0101 : Snac
  {
    public Snac0101() : base(0x1, 0x1)
    {
    }

    private ErrorCode _ErrorCode;
    public ErrorCode ErrorCode {
      get { return _ErrorCode; }
      set { _ErrorCode = value; }
    }

    private TlvErrorSubCode _SubError = new TlvErrorSubCode();
    public TlvErrorSubCode SubError {
      get { return _SubError; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data;

      data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)_ErrorCode));
      data.AddRange(_SubError.Serialize);

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _ErrorCode = (ErrorCode)(short)ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        if (desc.TypeId == 0x8) {
          _SubError.Deserialize(data.GetRange(index, desc.TotalSize));
        }

        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override int CalculateDataSize()
    {
      return 2 + 6;
    }
  }
}

