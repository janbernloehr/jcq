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
  public class Snac0105 : Snac
  {
    public Snac0105() : base(0x1, 0x5)
    {
    }

    private TlvServiceFamilyId _ServiceFamily = new TlvServiceFamilyId();
    public TlvServiceFamilyId ServiceFamily {
      get { return _ServiceFamily; }
    }

    private TlvServerAddress _ServerAddress = new TlvServerAddress();
    public TlvServerAddress ServerAddress {
      get { return _ServerAddress; }
    }

    private TlvAuthorizationCookie _AuthorizationCookie = new TlvAuthorizationCookie();
    public TlvAuthorizationCookie AuthorizationCookie {
      get { return _AuthorizationCookie; }
    }

    public override int CalculateDataSize()
    {
      return _ServiceFamily.CalculateTotalSize + _ServerAddress.CalculateTotalSize + _AuthorizationCookie.CalculateTotalSize;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      index += 8;

      while (index < data.Count) {
        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

        switch (desc.TypeId) {
          case 0xd:
            _ServiceFamily.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x5:
            _ServerAddress.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
          case 0x6:
            _AuthorizationCookie.Deserialize(data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(_ServiceFamily.Serialize);
      data.AddRange(_ServerAddress.Serialize);
      data.AddRange(_AuthorizationCookie.Serialize);

      return data;
    }
  }

  public class TlvServiceFamilyId : Tlv
  {
    public TlvServiceFamilyId() : base(0xd)
    {
    }

    private int _ServiceFamilyId;
    public int ServiceFamilyId {
      get { return _ServiceFamilyId; }
      set { _ServiceFamilyId = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ServiceFamilyId = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ServiceFamilyId));

      return data;
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  public class TlvServerAddress : Tlv
  {
    public TlvServerAddress() : base(0x5)
    {
    }

    private string _ServerAddress;
    public string ServerAddress {
      get { return _ServerAddress; }
      set { _ServerAddress = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_ServerAddress));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      _ServerAddress = ByteConverter.ToString(data.GetRange(4, DataSize));
    }

    public override int CalculateDataSize()
    {
      return _ServerAddress.Length;
    }
  }

  //Public Class TlvScreenName
  //    Inherits Tlv

  //    Private _ScreenName As String
  //    Public Property ScreenName() As String
  //        Get
  //            Return _ScreenName
  //        End Get
  //        Set(ByVal value As String)
  //            _ScreenName = value
  //        End Set
  //    End Property

  //    Public Overrides Function Serialize() As System.Collections.Generic.List(Of Byte)
  //        Dim data As List(Of Byte) =  MyBase.Serialize()

  //        data.AddRange(ByteConverter.GetBytes(_ScreenName))

  //        Return data
  //    End Function

  //    Public Overrides Sub Deserialize(ByVal data As System.Collections.Generic.List(Of Byte))
  //        MyBase.Deserialize(data)

  //        _ScreenName = ByteConverter.ToString(data.GetRange(4, DataSize))
  //    End Sub

  //    public Overrides Function CalculateDataSize() As Integer
  //        Return _ScreenName.Length
  //    End Function
  //End Class

  //Public Class TlvAuthorizationCookie
  //    Inherits Tlv

  //    Public Sub New()
  //        MyBase.New(&H6)
  //    End Sub

  //    Private _AutorizationCode As String
  //    Public Property AutorizationCode() As String
  //        Get
  //            Return _AutorizationCode
  //        End Get
  //        Set(ByVal value As String)
  //            _AutorizationCode = value
  //        End Set
  //    End Property

  //    Public Overrides Function Serialize() As System.Collections.Generic.List(Of Byte)
  //        Dim data As List(Of Byte) =  MyBase.Serialize()

  //        data.AddRange(ByteConverter.GetBytes(_AutorizationCode))

  //        Return data
  //    End Function

  //    Public Overrides Sub Deserialize(ByVal data As System.Collections.Generic.List(Of Byte))
  //        MyBase.Deserialize(data)

  //        _AutorizationCode = ByteConverter.ToString(data.GetRange(4, DataSize))
  //    End Sub

  //    Public Overrides Function CalculateDataSize() As Integer
  //        Return _AutorizationCode.Length
  //    End Function
  //End Class
}

