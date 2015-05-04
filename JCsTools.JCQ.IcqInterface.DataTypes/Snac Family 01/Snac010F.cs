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
  public class Snac010F : Snac
  {
    public Snac010F() : base(0x1, 0xf)
    {
    }

    //Private _Uin As String
    //Public Property Uin() As String
    //    Get
    //        Return _Uin
    //    End Get
    //    Set(ByVal value As String)
    //        _Uin = value
    //    End Set
    //End Property

    //Private _WarningLevel As Integer
    //Public Property WarningLevel() As Integer
    //    Get
    //        Return _WarningLevel
    //    End Get
    //    Set(ByVal value As Integer)
    //        _WarningLevel = value
    //    End Set
    //End Property

    //Private _DcInfo As New TlvDCInfo
    //Public ReadOnly Property DcInfo() As TlvDCInfo
    //    Get
    //        Return _DcInfo
    //    End Get
    //End Property

    //Private _UserClass As New TlvUserClass
    //Public ReadOnly Property UserClass() As TlvUserClass
    //    Get
    //        Return _UserClass
    //    End Get
    //End Property

    //Private _UserStatus As New TlvUserStatus
    //Public ReadOnly Property UserStatus() As TlvUserStatus
    //    Get
    //        Return _UserStatus
    //    End Get
    //End Property

    //Private _ExternalIpAddress As New TlvExternalIpAddress
    //Public ReadOnly Property ExternalIpAddress() As TlvExternalIpAddress
    //    Get
    //        Return _ExternalIpAddress
    //    End Get
    //End Property

    //Private _ClientIdleTime As New TlvClientIdleTime
    //Public ReadOnly Property ClientIdleTime() As TlvClientIdleTime
    //    Get
    //        Return _ClientIdleTime
    //    End Get
    //End Property

    //Private _SignOnTime As New TlvSignonTime
    //Public ReadOnly Property SignOnTime() As TlvSignonTime
    //    Get
    //        Return _SignOnTime
    //    End Get
    //End Property

    //Private _MemberSince As New TlvMemberSince
    //Public ReadOnly Property MemberSince() As TlvMemberSince
    //    Get
    //        Return _MemberSince
    //    End Get
    //End Property


    private List<UserInfo> _UserInfos = new List<UserInfo>();
    public List<UserInfo> UserInfos {
      get { return _UserInfos; }
    }

    //Private _TlvLenght As Integer

    //Private ReadOnly Property TlvLenght() As Integer
    //    Get
    //        If _TlvLenght = 0 Then
    //            Return _UserClass.TotalSize + _UserStatus.TotalSize + _DcInfo.TotalSize + _ExternalIpAddress.TotalSize + _ClientIdleTime.TotalSize + _SignOnTime.TotalSize + _MemberSince.TotalSize
    //        Else
    //            Return _TlvLenght
    //        End If
    //    End Get
    //End Property

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;

      while (index < data.Count) {
        UserInfo info = new UserInfo();
        info.Deserialize(index, data);
        _UserInfos.Add(info);

        index += info.TotalSize;
      }
      //Dim tlvCount As Integer
      //Dim tlvIndex As Integer

      //index = Snac.SizeFixPart

      //_Uin = ByteConverter.ToStringFromByteIndex(index, data)
      //index += 1 + _Uin.Length

      //_WarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2))
      //index += 2

      //tlvCount = ByteConverter.ToUInt16(data.GetRange(index, 2))
      //index += 2

      //Do While tlvIndex < tlvCount
      //    Dim desc As TlvDescriptor = TlvDescriptor.GetDescriptor(index, data)

      //    Select Case desc.TypeId
      //        Case &H1
      //            _UserClass.Deserialize(data.GetRange(index, desc.TotalSize))
      //        Case &H6
      //            _UserStatus.Deserialize(data.GetRange(index, desc.TotalSize))
      //        Case &HC
      //            _DcInfo.Deserialize(data.GetRange(index, desc.TotalSize))
      //        Case &HA
      //            _ExternalIpAddress.Deserialize(data.GetRange(index, desc.TotalSize))
      //        Case &HF
      //            _ClientIdleTime.Deserialize(data.GetRange(index, desc.TotalSize))
      //        Case &H3
      //            _SignOnTime.Deserialize(data.GetRange(index, desc.TotalSize))
      //        Case &H5
      //            _MemberSince.Deserialize(data.GetRange(index, desc.TotalSize))
      //    End Select

      //    index += desc.TotalSize
      //    _TlvLenght += desc.TotalSize

      //    tlvIndex += 1
      //Loop

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      //Dim data As List(Of Byte) =  MyBase.Serialize()

      //Dim temp As New List(Of Byte)
      //Dim tlvIndex As Integer

      //data.Add(CByte(_Uin.Length))
      //data.AddRange(ByteConverter.GetBytes(_Uin))
      //data.AddRange(ByteConverter.GetBytes(CUShort(_WarningLevel)))

      //If _UserClass IsNot Nothing Then
      //    temp.AddRange(_UserClass.Serialize)
      //    tlvIndex += 1
      //End If

      //If _UserStatus IsNot Nothing Then
      //    temp.AddRange(_UserStatus.Serialize)
      //    tlvIndex += 1
      //End If

      //If _DcInfo IsNot Nothing Then
      //    temp.AddRange(_DcInfo.Serialize)
      //    tlvIndex += 1
      //End If

      //If _ExternalIpAddress IsNot Nothing Then
      //    temp.AddRange(_ExternalIpAddress.Serialize)
      //    tlvIndex += 1
      //End If

      //If _ClientIdleTime IsNot Nothing Then
      //    temp.AddRange(_ClientIdleTime.Serialize)
      //    tlvIndex += 1
      //End If

      //If _SignOnTime IsNot Nothing Then
      //    temp.AddRange(_SignOnTime.Serialize)
      //    tlvIndex += 1
      //End If

      //If _MemberSince IsNot Nothing Then
      //    temp.AddRange(_MemberSince.Serialize)
      //    tlvIndex += 1
      //End If

      //data.AddRange(ByteConverter.GetBytes(CUShort(tlvIndex)))
      //data.AddRange(temp)

      //Return data

      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      //Return 1 + _Uin.Length + 2 + 2 + TlvLenght

      throw new NotImplementedException();
    }
  }
}

