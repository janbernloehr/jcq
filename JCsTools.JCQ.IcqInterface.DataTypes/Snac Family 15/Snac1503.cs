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
  public class Snac1503 : Snac
  {
    public Snac1503() : base(0x15, 0x3)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    private TlvMetaResponseData _MetaData = new TlvMetaResponseData();
    public TlvMetaResponseData MetaData {
      get { return _MetaData; }
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

        Core.Kernel.Logger.Log("Snac1503", TraceEventType.Information, "tlv {0:X2} found at index {1}; data size: {2} total lenght: {3}", desc.TypeId, index, desc.DataSize, data.Count);

        switch (desc.TypeId) {
          case 0x1:
            _MetaData.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      this.SetTotalSize(index);
    }
  }
}

