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
/// The server sends Snac0B02 to inform the client about the minimum usage report
/// intervall. The default is every 1200 hours.
/// Sent by: Server
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
  public class Snac0B02 : Snac
  {
    public Snac0B02() : base(0xb, 0x2)
    {
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

      ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      SetTotalSize(index);
    }

    private TimeSpan _MinimumReportIntervall;
    public TimeSpan MinimumReportIntervall {
      get { return _MinimumReportIntervall; }
      set { _MinimumReportIntervall = value; }
    }
  }
}

