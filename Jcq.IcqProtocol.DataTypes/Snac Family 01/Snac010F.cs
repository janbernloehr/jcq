// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac010F.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Jcq.IcqProtocol.DataTypes
{
    public class Snac010F : Snac
    {
        public Snac010F() : base(0x1, 0xf)
        {
        }

        public List<UserInfo> UserInfos { get; } = new List<UserInfo>();

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            while (index < data.Count)
            {
                var info = new UserInfo();
                info.Deserialize(index, data);
                UserInfos.Add(info);

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