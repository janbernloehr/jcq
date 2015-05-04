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
  public class Snac0107 : Snac
  {
    public Snac0107() : base(0x1, 0x7)
    {
    }

    private List<RateClass> _RateClasses = new List<RateClass>();
    public List<RateClass> RateClasses {
      get { return _RateClasses; }
    }

    private List<RateGroup> _RateGroups = new List<RateGroup>();
    public List<RateGroup> RateGroups {
      get { return _RateGroups; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      int classCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      int classIndex = 0;

      while (classIndex < classCount) {
        RateClass cls;
        cls = new RateClass();
        cls.Deserialize(data.GetRange(index, data.Count - index));

        _RateClasses.Add(cls);

        index += cls.TotalSize;
        classIndex += 1;
      }

      while (index + 4 <= data.Count) {
        RateGroup @group;
        @group = new RateGroup();
        @group.Deserialize(data.GetRange(index, data.Count - index));

        _RateGroups.Add(@group);

        index += @group.TotalSize;
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      int size = 2;
      foreach (RateClass x in _RateClasses) {
        size += x.CalculateTotalSize;
      }
      foreach (RateGroup x in _RateGroups) {
        size += x.CalculateTotalSize;
      }
      return size;
    }
  }

  public class RateClass : ISerializable
  {
    public int ISerializable.TotalSize {
      get { return DataSize; }
    }

    private int _DataSize;
    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public int ISerializable.CalculateDataSize()
    {
      return 35;
    }

    public int ISerializable.CalculateTotalSize()
    {
      return CalculateDataSize();
    }

    public void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int index = 0;

      _ClassId = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _WindowSize = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _ClearLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _AlertLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _LimitLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _DisconnectLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _CurrentLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _MaxLevel = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _LastTime = ByteConverter.ToUInt32(data.GetRange(index, 4));
      index += 4;

      _CurrentState = data(index);
      index += 1;

      _DataSize = index;
      _HasData = true;
    }

    public System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      throw new NotImplementedException();
    }

    private long _ClassId;
    public long ClassId {
      get { return _ClassId; }
      set { _ClassId = value; }
    }

    private long _WindowSize;
    public long WindowSize {
      get { return _WindowSize; }
      set { _WindowSize = value; }
    }

    private long _ClearLevel;
    public long ClearLevel {
      get { return _ClearLevel; }
      set { _ClearLevel = value; }
    }

    private long _AlertLevel;
    public long AlertLevel {
      get { return _AlertLevel; }
      set { _AlertLevel = value; }
    }

    private long _LimitLevel;
    public long LimitLevel {
      get { return _LimitLevel; }
      set { _LimitLevel = value; }
    }

    private long _DisconnectLevel;
    public long DisconnectLevel {
      get { return _DisconnectLevel; }
      set { _DisconnectLevel = value; }
    }

    private long _CurrentLevel;
    public long CurrentLevel {
      get { return _CurrentLevel; }
      set { _CurrentLevel = value; }
    }

    private long _MaxLevel;
    public long MaxLevel {
      get { return _MaxLevel; }
      set { _MaxLevel = value; }
    }

    private long _LastTime;
    public long LastTime {
      get { return _LastTime; }
      set { _LastTime = value; }
    }

    private byte _CurrentState;
    public byte CurrentState {
      get { return _CurrentState; }
      set { _CurrentState = value; }
    }

    private bool _HasData;

    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public override string ToString()
    {
      return string.Format("Window size: {0}\\nClear level: {1}\\nAlert level: {2}\\n" + "Limit level: {3}\\nDisconnect level: {4}\\nCurrent level: {5}\\n" + "Max level: {6}\\nLast time: {7}", WindowSize, ClearLevel, AlertLevel, LimitLevel, DisconnectLevel, CurrentLevel, MaxLevel, LastTime).Replace("\\n", Environment.NewLine);
    }
  }

  public class RateGroup : ISerializable
  {
    private int _GroupId;
    public int GroupId {
      get { return _GroupId; }
      set { _GroupId = value; }
    }

    private List<FamilySubtypePair> _ServiceFamilyIdSubTypeIdPairs = new List<FamilySubtypePair>();
    public List<FamilySubtypePair> ServiceFamilyIdSubTypeIdPairs {
      get { return _ServiceFamilyIdSubTypeIdPairs; }
    }

    public int ISerializable.TotalSize {
      get { return 4 + DataSize; }
    }

    private int _DataSize;
    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int pairCount;
      int pairIndex;
      int index;

      _GroupId = ByteConverter.ToUInt16(data.GetRange(0, 2));
      pairCount = ByteConverter.ToUInt16(data.GetRange(2, 2));

      index = 4;

      while (pairIndex < pairCount) {
        int familyId = ByteConverter.ToUInt16(data.GetRange(index, 2));
        int subtypeId = ByteConverter.ToUInt16(data.GetRange(index + 2, 2));

        _ServiceFamilyIdSubTypeIdPairs.Add(new FamilySubtypePair(familyId, subtypeId));

        pairIndex += 1;
        index += 4;
      }

      _DataSize = index - 4;
      _HasData = true;
    }

    public System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      throw new NotImplementedException();
    }

    private bool _HasData;

    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public int ISerializable.CalculateDataSize()
    {
      return _ServiceFamilyIdSubTypeIdPairs.Count * 4;
    }

    public int ISerializable.CalculateTotalSize()
    {
      return 4 + CalculateDataSize();
    }
  }

  public class FamilySubtypePair
  {
    public FamilySubtypePair(int family, int subtype)
    {
      _FamilyId = family;
      _SubtypeId = subtype;
    }

    private int _FamilyId;
    public int FamilyId {
      get { return _FamilyId; }
      set { _FamilyId = value; }
    }

    private int _SubtypeId;
    public int SubtypeId {
      get { return _SubtypeId; }
      set { _SubtypeId = value; }
    }
  }
}

