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
  public class Snac0117 : Snac
  {
    public Snac0117() : base(0x1, 0x17)
    {
    }

    private List<FamilyVersionPair> _FamilyNameVersionPairs = new List<FamilyVersionPair>();
    public List<FamilyVersionPair> FamilyNameVersionPairs {
      get { return _FamilyNameVersionPairs; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      foreach (FamilyVersionPair pair in _FamilyNameVersionPairs) {
        data.AddRange(ByteConverter.GetBytes((ushort)pair.FamilyNumber));
        data.AddRange(ByteConverter.GetBytes((ushort)pair.FamilyVersion));
      }

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      return _FamilyNameVersionPairs.Count * 4;
    }
  }

  public class FamilyVersionPair
  {
    public FamilyVersionPair(int number, int version)
    {
      _FamilyNumber = number;
      _FamilyVersion = version;
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
  }
}

