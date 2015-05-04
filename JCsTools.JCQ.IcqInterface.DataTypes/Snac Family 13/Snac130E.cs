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
  public class Snac130E : Snac
  {
    public Snac130E() : base(0x13, 0xe)
    {
    }

    public override int CalculateDataSize()
    {
      return _ActionResultCodes.Count * 2;
    }

    private List<SSIActionResultCode> _ActionResultCodes = new List<SSIActionResultCode>();
    public List<SSIActionResultCode> ActionResultCodes {
      get { return _ActionResultCodes; }
      set { _ActionResultCodes = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      if (data.Count > index + 2) {
        if (data(index) == 0 & data(index + 1) == 6) {
          index += 2;
        }
      }

      if (data.Count > index + 2) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);
        index += desc.TotalSize;
      }

      while (index < data.Count) {
        SSIActionResultCode code;

        code = (SSIActionResultCode)ByteConverter.ToUInt16(data.GetRange(index, 2));
        _ActionResultCodes.Add(code);

        index += 2;
      }

      this.SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

  }

  public enum SSIActionResultCode : ushort
  {
    Success = 0x0,
    ItemNotFoundInList = 0x2,
    ItemAlreadyExists = 0x3,
    ErrorAddingItem_InvalidId_OR_AllreadyInList_OR_InvalidData = 0xa,
    LimitForThisTypeOfItemsExceeded = 0xc,
    TryingToAddICQContactToAIMList = 0xd,
    CantAddContactBecauseItRequiresAuthorization = 0xe
  }
}

