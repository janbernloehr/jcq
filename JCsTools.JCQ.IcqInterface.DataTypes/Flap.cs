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
  public class Flap : ISerializable
  {
    private int _DataSize;

    public const int SizeFixPart = 6;

    public Flap()
    {

    }

    public Flap(FlapChannel channel)
    {
      _Channel = channel;
    }

    private FlapChannel _Channel;
    public FlapChannel Channel {
      get { return _Channel; }
      set { _Channel = value; }
    }

    private int _DatagramSequenceNumber;
    public int DatagramSequenceNumber {
      get { return _DatagramSequenceNumber; }
      set { _DatagramSequenceNumber = value; }
    }

    private List<ISerializable> _DataItems = new List<ISerializable>();
    public List<ISerializable> DataItems {
      get { return _DataItems; }
    }

    private bool _HasData = false;

    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    public virtual int ISerializable.CalculateDataSize()
    {
      int size;
      foreach (ISerializable x in _DataItems) {
        size += x.CalculateTotalSize;
      }
      return size;
    }

    public int ISerializable.CalculateTotalSize()
    {
      return SizeFixPart + CalculateDataSize();
    }

    public int ISerializable.FlapDataSize {
      get { return _DataSize; }
    }

    public int ISerializable.FlapTotalSize {
      get { return SizeFixPart + FlapDataSize; }
    }

    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int index;

      index += 1;

      _Channel = (FlapChannel)data(index);
      index += 1;

      _DatagramSequenceNumber = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      switch (_Channel) {
        case FlapChannel.SnacData:
          SnacDescriptor desc = SnacDescriptor.GetDescriptor(index, data);
          Snac x = SerializationContext.DeserializeSnac(index, desc, data);

          try {
            if (x == null)
              throw new NotImplementedException(string.Format("Snac {0} is not implemented.", SnacDescriptor.GetKey(desc)));
            index += x.SnacTotalSize;

            _DataItems.Add(x);

            if (index < _DataSize)
              throw new InvalidOperationException(string.Format("Deserialization of Snac {0} failed {1} bytes remaining.", SnacDescriptor.GetKey(desc), data.Count - index));
          } catch (Exception ex) {
            JCsTools.Core.Kernel.Exceptions.PublishException(ex);
          }
          break;
        case FlapChannel.CloseConnectionNegotiation:
          while (index < FlapDataSize) {
            TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

            switch (desc.TypeId) {
              case 0x1:
                TlvScreenName tlv;

                tlv = new TlvScreenName();
                tlv.Deserialize(data.GetRange(index, desc.TotalSize));

                DataItems.Add(tlv);
                break;
              case 0x5:
                TlvBosServerAddress tlv;

                tlv = new TlvBosServerAddress();
                tlv.Deserialize(data.GetRange(index, desc.TotalSize));

                DataItems.Add(tlv);
                break;
              case 0x6:
                TlvAuthorizationCookie tlv;

                tlv = new TlvAuthorizationCookie();
                tlv.Deserialize(data.GetRange(index, desc.TotalSize));

                DataItems.Add(tlv);
                break;
              case 0x8:
                TlvAuthFailed tlv;

                tlv = new TlvAuthFailed();
                tlv.Deserialize(data.GetRange(index, desc.TotalSize));

                DataItems.Add(tlv);
                break;
            }

            index += desc.TotalSize;
          }
          break;
      }

      _HasData = true;
    }

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data;

      _DataSize = CalculateDataSize();

      data = new List<byte>(SizeFixPart + _DataSize);

      data.Add(0x2a);
      data.Add(_Channel);
      data.AddRange(ByteConverter.GetBytes((ushort)_DatagramSequenceNumber));

      if (_DataSize > UInt16.MaxValue)
        throw new OverflowException(string.Format("DataSize cannot exceed {0} bytes", UInt32.MaxValue));

      data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(_DataSize)));

      foreach (ISerializable x in DataItems) {
        data.AddRange(x.Serialize);
      }

      return data;
    }

  }

  public enum FlapChannel : byte
  {
    NewConnectionNegotiation = 1,
    SnacData = 2,
    FlapLevelError = 3,
    CloseConnectionNegotiation = 4,
    KeepAlive = 5
  }

  public class FlapDescriptor
  {
    public static FlapDescriptor GetDescriptor(int offset, List<byte> bytes)
    {
      List<byte> data = bytes.GetRange(offset, bytes.Count - offset);

      FlapDescriptor desc = new FlapDescriptor();
      desc.Deserialize(data);

      return desc;
    }

    private void Deserialize(List<byte> data)
    {
      int index = 0;

      if (data(index) != 0x2a) {
        string info = null;

        for (int i = 0; i <= Flap.SizeFixPart - 1; i++) {
          info += string.Format("{0:X} ", data(i));
        }

        throw new InvalidOperationException(string.Format("No flap at this position: {0}", info));
      }

      index += 1;

      _Channel = (FlapChannel)data(index);
      index += 1;

      _DatagramSequenceNumber = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;

      _DataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
      index += 2;
    }

    private FlapChannel _Channel;
    public FlapChannel Channel {
      get { return _Channel; }
      set { _Channel = value; }
    }

    private int _DatagramSequenceNumber;
    public int DatagramSequenceNumber {
      get { return _DatagramSequenceNumber; }
      set { _DatagramSequenceNumber = value; }
    }

    private int _DataSize;
    public int DataSize {
      get { return _DataSize; }
    }

    public int TotalSize {
      get { return 6 + DataSize; }
    }
  }

  public class TlvBosServerAddress : Tlv
  {
    public TlvBosServerAddress() : base(0x5)
    {
    }

    public override int CalculateDataSize()
    {
      return _BosServerAddress.Length;
    }

    private string _BosServerAddress;
    public string BosServerAddress {
      get { return _BosServerAddress; }
      set { _BosServerAddress = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _BosServerAddress = ByteConverter.ToString(data.GetRange(index, DataSize));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_BosServerAddress));

      return data;
    }
  }

  public class TlvAuthorizationCookie : Tlv
  {
    public TlvAuthorizationCookie() : base(0x6)
    {
    }

    private List<byte> _AuthorizationCookie = new List<byte>();
    public List<byte> AuthorizationCookie {
      get { return _AuthorizationCookie; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _AuthorizationCookie.AddRange(data.GetRange(index, DataSize));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(_AuthorizationCookie);

      return data;
    }

    public override int CalculateDataSize()
    {
      return _AuthorizationCookie.Count;
    }
  }

  public class FlapSendSignInCookie : Flap
  {
    public FlapSendSignInCookie() : base(FlapChannel.NewConnectionNegotiation)
    {
    }

    public override int CalculateDataSize()
    {
      return 4 + _AuthorizationCookie.CalculateTotalSize;
    }

    private TlvAuthorizationCookie _AuthorizationCookie = new TlvAuthorizationCookie();
    public TlvAuthorizationCookie AuthorizationCookie {
      get { return _AuthorizationCookie; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)1));

      data.AddRange(_AuthorizationCookie.Serialize);

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }
  }
}

