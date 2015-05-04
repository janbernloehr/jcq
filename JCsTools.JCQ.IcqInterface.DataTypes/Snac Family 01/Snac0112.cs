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
  public class Snac0112 : Snac
  {
    private int _tlvsSize;

    public Snac0112() : base(0x1, 0x12)
    {
    }

    private List<int> _Families = new List<int>();
    public List<int> Families {
      get { return _Families; }
    }


    private TlvServerAddress _ServerAddress = new TlvServerAddress();
    public TlvServerAddress ServerAddress {
      get { return _ServerAddress; }
    }

    private TlvAuthorizationCookie _AuthorizationCookie = new TlvAuthorizationCookie();
    public TlvAuthorizationCookie AuthorizationCookie {
      get { return _AuthorizationCookie; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      int familyCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
      int familyIndex;

      index += 2;

      while (familyIndex < familyCount) {
        _Families.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

        index += 2;
      }

      while (index + 4 <= data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x5:
            _ServerAddress.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x6:
            _ServerAddress.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        _tlvsSize += desc.TotalSize;
        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      return 2 + 2 * _Families.Count + _tlvsSize;
    }
  }
}

