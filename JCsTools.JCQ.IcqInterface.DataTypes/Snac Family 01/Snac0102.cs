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
  public class Snac0102 : Snac
  {
    public Snac0102() : base(0x1, 0x2)
    {
    }

    private List<FamilyIdToolPair> _Families = new List<FamilyIdToolPair>();

    public List<FamilyIdToolPair> Families {
      get { return _Families; }
    }

    public override int CalculateDataSize()
    {
      return _Families.Count * FamilyIdToolPair.SizeFixPart;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      foreach (FamilyIdToolPair x in _Families) {
        data.AddRange(x.Serialize);
      }

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      while (index < data.Count) {
        FamilyIdToolPair fam = new FamilyIdToolPair();

        fam.Deserialize(data.GetRange(index, 8));

        _Families.Add(fam);

        index += FamilyIdToolPair.SizeFixPart;
      }

      SetTotalSize(index);
    }
  }

  public class FamilyIdToolPair : ISerializable
  {
    public const int SizeFixPart = 8;

    public FamilyIdToolPair()
    {

    }

    public FamilyIdToolPair(int serviceId, int version, int toolId, int toolVersion)
    {
      _FamilyNumber = serviceId;
      _FamilyVersion = version;
      _ToolId = toolId;
      _ToolVersion = toolVersion;
    }

    private int _FamilyNumber;
    public int FamilyNumber {
      get { return _FamilyNumber; }
      set { _FamilyNumber = value; }
    }

    private int _FamilyVersion;
    public int FamilyVersion {
      get { return _FamilyVersion; }
      set { _FamilyVersion = value; }
    }

    private int _ToolId;
    public int ToolId {
      get { return _ToolId; }
      set { _ToolId = value; }
    }

    private int _ToolVersion;
    public int ToolVersion {
      get { return _ToolVersion; }
      set { _ToolVersion = value; }
    }

    public int ISerializable.DataSize {
      get { return 0; }
    }

    public int ISerializable.TotalSize {
      get { return SizeFixPart; }
    }

    public int ISerializable.CalculateDataSize()
    {
      return 0;
    }

    public int ISerializable.CalculateTotalSize()
    {
      return SizeFixPart;
    }

    public void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      _FamilyNumber = ByteConverter.ToUInt16(data.GetRange(0, 2));
      _FamilyVersion = ByteConverter.ToUInt16(data.GetRange(2, 2));
      _ToolId = ByteConverter.ToUInt16(data.GetRange(4, 2));
      _ToolVersion = ByteConverter.ToUInt16(data.GetRange(6, 2));

      _HasData = true;
    }

    public System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data;

      data = new List<byte>();

      data.AddRange(ByteConverter.GetBytes((ushort)_FamilyNumber));
      data.AddRange(ByteConverter.GetBytes((ushort)_FamilyVersion));
      data.AddRange(ByteConverter.GetBytes((ushort)_ToolId));
      data.AddRange(ByteConverter.GetBytes((ushort)_ToolVersion));

      return data;
    }

    private bool _HasData;
    public bool ISerializable.HasData {
      get { return _HasData; }
    }

  }
}

