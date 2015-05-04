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
  public class Snac0121 : Snac
  {
    public Snac0121() : base(0x1, 0x21)
    {
    }

    private ExtendedStatusNotification _Notification;
    public ExtendedStatusNotification Notification {
      get { return _Notification; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      if (data.Count > index + 2 && (data(index) == 0 & data(index + 1) == 6)) {
        // Icq sends information about the service version

        index += 2;

        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        index += desc.TotalSize;
      }

      ExtendedStatusNotificationType type;

      type = (ExtendedStatusNotificationType)ByteConverter.ToUInt16(data.GetRange(index, 2));

      switch (type) {
        case ExtendedStatusNotificationType.UploadIconRequest:
          _Notification = new UploadIconNotification();
          break;
        case ExtendedStatusNotificationType.iChatAvialable:
          _Notification = new iChatAvailableNotification();
          break;
        default:
          throw new NotImplementedException();
          break;
      }

      _Notification.Deserialize(data.GetRange(index, data.Count - index));

      index += _Notification.TotalSize;

      SetTotalSize(index);
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }
  }

  public abstract class ExtendedStatusNotification : ISerializable
  {
    public const int SizeFixPart = 2;

    private int _DataSize;

    public abstract int ISerializable.CalculateDataSize();

    public int ISerializable.CalculateTotalSize()
    {
      return SizeFixPart + CalculateDataSize();
    }

    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    public int ISerializable.TotalSize {
      get { return SizeFixPart + DataSize; }
    }

    protected void SetDataSize(int value)
    {
      _DataSize = value;
    }

    private ExtendedStatusNotificationType _Type;
    public ExtendedStatusNotificationType Type {
      get { return _Type; }
      set { _Type = value; }
    }

    private bool _HasData;
    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      _Type = (ExtendedStatusNotificationType)ByteConverter.ToUInt16(data.GetRange(0, 2));

      _HasData = true;
    }

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data;

      data = new List<byte>();

      data.AddRange(ByteConverter.GetBytes(_Type));

      return data;
    }
  }

  public class UploadIconNotification : ExtendedStatusNotification
  {
    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }


    private UploadIconFlag _IconFlag;
    public UploadIconFlag IconFlag {
      get { return _IconFlag; }
      set { _IconFlag = value; }
    }

    private List<byte> _IconHash = new List<byte>(16);
    public List<byte> IconHash {
      get { return _IconHash; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = SizeFixPart;

      byte hashLenght;

      _IconFlag = (UploadIconFlag)data(index);
      index += 1;
      hashLenght = data(index);
      index += 1;

      _IconHash.AddRange(data.GetRange(index, hashLenght));
      index += hashLenght;

      SetDataSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public enum UploadIconFlag : byte
  {
    FirstUpload = 0x41
  }

  public class iChatAvailableNotification : ExtendedStatusNotification
  {
    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      base.Deserialize(data);

      int index = SizeFixPart;

      string message;

      message = ByteConverter.ToStringFromUInt16Index(index, data);
      index += 2;
      if (!string.IsNullOrEmpty(message))
        index += message.Length;

      SetDataSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public enum ExtendedStatusNotificationType : ushort
  {
    UploadIconRequest = 1,
    iChatAvialable = 2
  }
}

