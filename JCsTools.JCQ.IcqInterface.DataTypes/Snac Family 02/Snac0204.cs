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
  public class Snac0204 : Snac
  {
    public Snac0204() : base(0x2, 0x4)
    {
    }

    private TlvMimeType _MimeType = new TlvMimeType();
    public TlvMimeType MimeType {
      get { return _MimeType; }
    }

    private TlvCapabilities _Capabilities = new TlvCapabilities();
    public TlvCapabilities Capabilities {
      get { return _Capabilities; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      if (_MimeType.CalculateDataSize > 0) {
        data.AddRange(_MimeType.Serialize);
      }

      data.AddRange(_Capabilities.Serialize);

      return data;
    }

    public override int CalculateDataSize()
    {
      if (_MimeType.CalculateDataSize == 0) {
        return _Capabilities.CalculateTotalSize;
      } else {
        return _MimeType.CalculateTotalSize + _Capabilities.CalculateTotalSize;
      }
    }
  }

  public class TlvMimeType : Tlv
  {
    public TlvMimeType() : base(0x1)
    {
    }

    private string _MimeTypeName;
    public string MimeTypeName {
      get { return _MimeTypeName; }
      set { _MimeTypeName = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_MimeTypeName));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      _MimeTypeName = ByteConverter.ToString(data.GetRange(4, DataSize));
    }

    public override int CalculateDataSize()
    {
      if (string.IsNullOrEmpty(_MimeTypeName)) {
        return 0;
      } else {
        return _MimeTypeName.Length;
      }
    }
  }

  public class TlvCapabilities : Tlv
  {
    public TlvCapabilities() : base(0x5)
    {
    }

    private List<Guid> _Capabilites = new List<Guid>();
    public List<Guid> Capabilites {
      get { return _Capabilites; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      foreach (Guid cap in _Capabilites) {
        data.AddRange(ByteConverter.GetBytes(cap));
      }

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      while (index + 16 <= data.Count) {
        _Capabilites.Add(ByteConverter.ToGuid(data.GetRange(index, 16)));

        index += 16;
      }
    }

    public override int CalculateDataSize()
    {
      return _Capabilites.Count * 16;
    }
  }

  public sealed class IcqClientCapabilities
  {
    private IcqClientCapabilities()
    {

    }

    private static readonly Guid _IcqFlag = new Guid("09461349-4C7F-11D1-8222-444553540000");
    private static readonly Guid _IcqRouteFinder = new Guid("09461344-4C7F-11D1-8222-444553540000");
    private static readonly Guid _RtfMessages = new Guid("97B12751-243C-4334-AD22-D6ABF73F1492");

    public static Guid IcqFlag {
      get { return _IcqFlag; }
    }

    public static Guid IcqRouteFinder {
      get { return _IcqRouteFinder; }
    }

    public static Guid RtfMessages {
      get { return _RtfMessages; }
    }
  }
}

