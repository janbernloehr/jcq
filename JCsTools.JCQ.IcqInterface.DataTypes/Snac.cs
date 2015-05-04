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
  public abstract class Snac : ISerializable
  {
    public const int SizeFixPart = 10;

    private int _DataSize;

    public Snac(int serviceId, int subtypeId)
    {
      _ServiceId = serviceId;
      _SubtypeId = subtypeId;
    }

    public static string GetKey(Snac snac)
    {
      return string.Format("{0:X2},{1:X2}", snac.SubtypeId == 1 ? 1 : snac.ServiceId, snac.SubtypeId);
    }

    private int _ServiceId;
    public int ServiceId {
      get { return _ServiceId; }
      set { _ServiceId = value; }
    }

    private int _SubtypeId;
    public int SubtypeId {
      get { return _SubtypeId; }
      set { _SubtypeId = value; }
    }

    private int _Flags;
    public int Flags {
      get { return _Flags; }
      set { _Flags = value; }
    }

    private long _RequestId;
    public long RequestId {
      get { return _RequestId; }
      set { _RequestId = value; }
    }

    private bool _HasData;
    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public abstract int ISerializable.CalculateDataSize();

    public int ISerializable.CalculateTotalSize()
    {
      return SizeFixPart + CalculateDataSize();
    }

    public int ISerializable.SnacDataSize {
      get { return _DataSize; }
    }

    public int ISerializable.SnacTotalSize {
      get { return SizeFixPart + SnacDataSize; }
    }

    protected void SetTotalSize(int value)
    {
      _DataSize = value - SizeFixPart;
    }

    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      _ServiceId = ByteConverter.ToUInt16(data.GetRange(0, 2));
      _SubtypeId = ByteConverter.ToUInt16(data.GetRange(2, 2));
      _Flags = ByteConverter.ToUInt16(data.GetRange(4, 2));
      _RequestId = ByteConverter.ToUInt16(data.GetRange(6, 4));

      _HasData = true;
    }

    private static long snacRequestId = 0;

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data;

      _DataSize = CalculateDataSize();

      data = new List<byte>(SizeFixPart + _DataSize);

      _RequestId = Threading.Interlocked.Increment(snacRequestId);

      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(_ServiceId)));
      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(_SubtypeId)));
      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(_Flags)));
      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt32(_RequestId)));

      return data;
    }
  }


  public class SnacDescriptor
  {
    private int _ServiceId;
    public int ServiceId {
      get { return _ServiceId; }
      set { _ServiceId = value; }
    }

    private int _SubtypeId;
    public int SubtypeId {
      get { return _SubtypeId; }
      set { _SubtypeId = value; }
    }

    private int _Flags;
    public int Flags {
      get { return _Flags; }
      set { _Flags = value; }
    }

    private long _RequestId;
    public long RequestId {
      get { return _RequestId; }
      set { _RequestId = value; }
    }

    public static string GetKey(SnacDescriptor descriptor)
    {
      return string.Format("{0:X2},{1:X2}", descriptor.SubtypeId == 1 ? 1 : descriptor.ServiceId, descriptor.SubtypeId);
    }

    public virtual void Deserialize(System.Collections.Generic.List<byte> data)
    {
      _ServiceId = ByteConverter.ToUInt16(data.GetRange(0, 2));
      _SubtypeId = ByteConverter.ToUInt16(data.GetRange(2, 2));
      _Flags = ByteConverter.ToUInt16(data.GetRange(4, 2));
      _RequestId = ByteConverter.ToUInt16(data.GetRange(6, 4));
    }

    public static SnacDescriptor GetDescriptor(int offset, System.Collections.Generic.List<byte> bytes)
    {
      List<byte> data = bytes.GetRange(offset, bytes.Count - offset);
      SnacDescriptor desc = new SnacDescriptor();
      desc.Deserialize(data);
      return desc;
    }
  }
}

