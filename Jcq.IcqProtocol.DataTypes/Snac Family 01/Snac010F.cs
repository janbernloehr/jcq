// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac010F.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac010F : Snac
    {
        private readonly List<UserInfo> _UserInfos = new List<UserInfo>();

        public Snac010F() : base(0x1, 0xf)
        {
        }

        public List<UserInfo> UserInfos
        {
            get { return _UserInfos; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var info = new UserInfo();
                info.Deserialize(index, data);
                _UserInfos.Add(info);

                index += info.TotalSize;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
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