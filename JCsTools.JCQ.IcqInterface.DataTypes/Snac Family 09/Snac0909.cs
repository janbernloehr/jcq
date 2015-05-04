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
  public class Snac0909 : Snac
  {
    public Snac0909() : base(0x9, 0x9)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    private int _ErrorCode;
    public int ErrorCode {
      get { return _ErrorCode; }
      set { _ErrorCode = value; }
    }

    private TlvErrorSubCode _ErrorSubCode = new TlvErrorSubCode();
    public TlvErrorSubCode ErrorSubCode {
      get { return _ErrorSubCode; }
    }

    private TlvErrorDescriptionUrl _ErrorDescriptionUrl = new TlvErrorDescriptionUrl();
    public TlvErrorDescriptionUrl ErrorDescriptionUrl {
      get { return _ErrorDescriptionUrl; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      _ErrorCode = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x8:
            _ErrorSubCode.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x4:
            _ErrorDescriptionUrl.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      this.SetTotalSize(index);
    }
  }

  public class TlvErrorDescriptionUrl : Tlv
  {
    public TlvErrorDescriptionUrl() : base(0x6)
    {
    }

    private string _ErrorDescriptionUrl;
    public string ErrorDescriptionUrl {
      get { return _ErrorDescriptionUrl; }
      set { _ErrorDescriptionUrl = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_ErrorDescriptionUrl));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      _ErrorDescriptionUrl = ByteConverter.ToString(data.GetRange(4, DataSize));
    }

    public override int CalculateDataSize()
    {
      return _ErrorDescriptionUrl.Length;
    }
  }
}

