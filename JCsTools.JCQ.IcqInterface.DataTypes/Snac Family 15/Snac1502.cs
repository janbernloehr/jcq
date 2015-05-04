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
  public class Snac1502 : Snac
  {
    public Snac1502() : base(0x15, 0x2)
    {
    }

    public override int CalculateDataSize()
    {
      return _MetaData.CalculateTotalSize;
    }

    private TlvMetaRequestData _MetaData = new TlvMetaRequestData();
    public TlvMetaRequestData MetaData {
      get { return _MetaData; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();
      data.AddRange(_MetaData.Serialize);
      return data;
    }

    public override string ToString()
    {
      if (MetaData.MetaRequest is MetaShortUserInformationRequest) {
        return string.Format("Search: {0}", ((MetaShortUserInformationRequest)MetaData.MetaRequest).SearchUin);
      } else {
        return base.ToString();
      }
    }
  }
}

