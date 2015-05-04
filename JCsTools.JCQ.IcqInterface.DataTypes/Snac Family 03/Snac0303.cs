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
  public class Snac0303 : Snac
  {
    public Snac0303() : base(0x3, 0x3)
    {
    }

    private TlvMaxBuddylistSize _MaxBuddylistSize = new TlvMaxBuddylistSize();
    public TlvMaxBuddylistSize MaxBuddylistSize {
      get { return _MaxBuddylistSize; }
    }

    private TlvMaxNumberOfWatchers _MaxNumberOfWatchers = new TlvMaxNumberOfWatchers();
    public TlvMaxNumberOfWatchers MaxNumberOfWatchers {
      get { return _MaxNumberOfWatchers; }
    }

    private TlvMaxOnlineNotifications _MaxOnlineNotifications = new TlvMaxOnlineNotifications();
    public TlvMaxOnlineNotifications MaxOnlineNotifications {
      get { return _MaxOnlineNotifications; }
    }

    public override int CalculateDataSize()
    {
      return _MaxBuddylistSize.CalculateTotalSize + _MaxNumberOfWatchers.CalculateTotalSize + _MaxOnlineNotifications.CalculateTotalSize;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0x1:
            _MaxBuddylistSize.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x2:
            _MaxNumberOfWatchers.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x3:
            _MaxOnlineNotifications.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }

  public class TlvMaxBuddylistSize : Tlv
  {
    public TlvMaxBuddylistSize() : base(0x1)
    {
    }

    private int _MaxNumberOfContactListEntries;
    public int MaxNumberOfContactListEntries {
      get { return _MaxNumberOfContactListEntries; }
      set { _MaxNumberOfContactListEntries = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _MaxNumberOfContactListEntries = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_MaxNumberOfContactListEntries));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  public class TlvMaxNumberOfWatchers : Tlv
  {
    public TlvMaxNumberOfWatchers() : base(0x2)
    {
    }

    private int _MaxNumberOfWatchers;
    public int MaxNumberOfWatchers {
      get { return _MaxNumberOfWatchers; }
      set { _MaxNumberOfWatchers = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _MaxNumberOfWatchers = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_MaxNumberOfWatchers));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  public class TlvMaxOnlineNotifications : Tlv
  {
    public TlvMaxOnlineNotifications() : base(0x3)
    {
    }

    private int _MaxOnlineNotifications;
    public int MaxOnlineNotifications {
      get { return _MaxOnlineNotifications; }
      set { _MaxOnlineNotifications = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _MaxOnlineNotifications = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_MaxOnlineNotifications));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }
}

