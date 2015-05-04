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
  public abstract class Tlv : ISerializable
  {
    private int _DataSize;
    private int _TypeNumber;
    private bool _HasData = false;

    public const int SizeFixPart = 4;

    public Tlv(int typeNumber)
    {
      _TypeNumber = typeNumber;
    }

    public Tlv(int typeNumber, int dataSize) : this(typeNumber)
    {

      _DataSize = dataSize;
    }

    public int TypeNumber {
      get { return _TypeNumber; }
      set { _TypeNumber = value; }
    }

    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public virtual int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public abstract int ISerializable.CalculateDataSize();

    public int ISerializable.TotalSize {
      get { return Tlv.SizeFixPart + DataSize; }
    }

    public int ISerializable.CalculateTotalSize()
    {
      return Tlv.SizeFixPart + CalculateDataSize();
    }

    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      _TypeNumber = ByteConverter.ToUInt16(data.GetRange(0, 2));
      _DataSize = ByteConverter.ToUInt16(data.GetRange(2, 2));

      _HasData = true;
    }

    protected void SetDataSize(int value)
    {
      _DataSize = value;
      _HasData = true;
    }

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data;

      _DataSize = CalculateDataSize();

      data = new List<byte>(SizeFixPart + _DataSize);

      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(TypeNumber)));
      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(DataSize)));

      return data;
    }

  }

  public abstract class LETlv : Tlv
  {
    public LETlv(int typeNumber) : base(typeNumber)
    {
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      TypeNumber = ByteConverter.ToUInt16LE(data.GetRange(0, 2));
      SetDataSize(ByteConverter.ToUInt16LE(data.GetRange(2, 2)));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data;

      data = new List<byte>(CalculateTotalSize);

      data.AddRange(ByteConverter.GetBytesLE(Convert.ToUInt16(TypeNumber)));
      data.AddRange(ByteConverter.GetBytesLE(Convert.ToUInt16(CalculateDataSize())));

      return data;
    }

  }

  public class TlvDescriptor
  {
    private TlvDescriptor(int typeId, int dataSize)
    {
      _TypeId = typeId;
      _DataSize = dataSize;
    }

    private int _TypeId;
    public int TypeId {
      get { return _TypeId; }
    }

    private int _DataSize;
    public int DataSize {
      get { return _DataSize; }
    }

    public int TotalSize {
      get { return DataSize + 4; }
    }

    public static TlvDescriptor GetDescriptor(int offset, List<byte> data)
    {
      int typeId;
      int dataSize;

      if (offset + 4 > data.Count) {
        if (Debugger.IsAttached)
          Debugger.Break();
        throw new ArgumentException(string.Format("Offset and length were out of bounds for the present data: {0}/{1}", offset + 4, data.Count));
      }

      typeId = ByteConverter.ToUInt16(data.GetRange(offset, 2));
      dataSize = ByteConverter.ToUInt16(data.GetRange(offset + 2, 2));

      return new TlvDescriptor(typeId, dataSize);
    }
  }

  //Public Class UnknownTlv
  //    Inherits Tlv



  //    public Overrides Function CalculateDataSize() As Integer
  //        Throw New NotImplementedException
  //    End Function
  //End Class
}

