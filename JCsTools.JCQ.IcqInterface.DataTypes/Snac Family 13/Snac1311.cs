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
/// The client/server sends Snac1311 to start a SSI modification transaction.
/// Sent by: Client, Server
/// Protocoll: AIM, ICQ
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
  public class Snac1311 : Snac
  {
    public Snac1311() : base(0x13, 0x11)
    {
    }

    public override int CalculateDataSize()
    {
      return _ImportContactsRequiringAuthorization ? 4 : 0;
    }

    private bool _ImportContactsRequiringAuthorization;
    public bool ImportContactsRequiringAuthorization {
      get { return _ImportContactsRequiringAuthorization; }
      set { _ImportContactsRequiringAuthorization = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = SizeFixPart;

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      if (_ImportContactsRequiringAuthorization) {
        data.AddRange(ByteConverter.GetBytes((uint)0x10000));
      }

      return data;
    }
  }
}

