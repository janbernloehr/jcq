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
  public class MetaSearchByTlvRequest : MetaInformationRequest
  {
    public MetaSearchByTlvRequest(MetaRequestSubType subtype) : base(subtype)
    {
    }

    public override int CalculateDataSize()
    {
      int size;

      size = 2;

      foreach (Tlv x in _SearchTlvs) {
        size += x.CalculateTotalSize();
      }

      return size;
    }

    private List<Tlv> _SearchTlvs = new List<Tlv>();

    public List<Tlv> SearchTlvs {
      get { return _SearchTlvs; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      foreach (Tlv x in _SearchTlvs) {
        data.AddRange(x.Serialize);
      }

      return data;
    }
  }

  public class SearchByUinTlv : LETlv
  {
    public SearchByUinTlv() : base(0x136)
    {
    }

    public override int CalculateDataSize()
    {
      return 4;
    }

    private int _Uin;
    public int Uin {
      get { return _Uin; }
      set { _Uin = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data;

      data = base.Serialize();

      data.AddRange(ByteConverter.GetBytesLE((uint)_Uin));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }
  }
}

