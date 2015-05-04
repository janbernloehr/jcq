// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvDcInfo.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class TlvDCInfo : Tlv
    {
        public TlvDCInfo() : base(0xc)
        {
        }

        public long DcInternalIpAddress { get; set; }
        public int DcPort { get; set; }
        public DcType DcByte { get; set; }
        public int DcProtocolVersion { get; set; }
        public long DcAuthCookie { get; set; }
        public long WebFrontPort { get; set; }
        public DateTime LastInfoUpdate { get; set; }
        public DateTime LastExtInfoUpdate { get; set; }
        public DateTime LastExtStatusUpdate { get; set; }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            try
            {
                DcInternalIpAddress = ByteConverter.ToUInt32(data.GetRange(index, 4));
                index += 4;

                DcPort = (int) ByteConverter.ToUInt32(data.GetRange(index, 4));
                index += 4;

                DcByte = (DcType) data[index];
                index += 1;

                DcProtocolVersion = ByteConverter.ToUInt16(data.GetRange(index, 2));
                index += 2;

                DcAuthCookie = ByteConverter.ToUInt32(data.GetRange(index, 4));
                index += 4;

                WebFrontPort = ByteConverter.ToUInt32(data.GetRange(index, 4));
                index += 4;

                //Client futures 
                index += 4;

                LastInfoUpdate = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
                index += 4;

                LastExtInfoUpdate = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
                index += 4;

                LastExtStatusUpdate = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
                index += 4;
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((uint) DcInternalIpAddress));
            data.AddRange(ByteConverter.GetBytes((uint) DcPort));
            data.Add((byte) DcByte);
            data.AddRange(ByteConverter.GetBytes((uint) DcProtocolVersion));
            data.AddRange(ByteConverter.GetBytes((uint) DcAuthCookie));
            data.AddRange(ByteConverter.GetBytes((uint) WebFrontPort));
            data.AddRange(ByteConverter.GetBytes((uint) 3));
            data.AddRange(ByteConverter.GetBytesForUInt32Date(LastInfoUpdate));
            data.AddRange(ByteConverter.GetBytesForUInt32Date(LastExtInfoUpdate));
            data.AddRange(ByteConverter.GetBytesForUInt32Date(LastExtStatusUpdate));

            return data;
        }

        //   xx xx xx xx   dword   DC internal ip address 
        //xx xx xx xx   dword   DC tcp port 
        //xx   byte   DC type 
        //xx xx   word   DC protocol version 
        //xx xx xx xx   dword   DC auth cookie 
        //xx xx xx xx   dword   Web front port 
        //00 00 00 03   dword   Client futures 
        //xx xx xx xx   dword   last info update time 
        //xx xx xx xx   dword   last ext info update time (i.e. icqphone status) 
        //xx xx xx xx   dword   last ext status update time (i.e. phonebook) 
        //xx xx   word   unknown 

        public override int CalculateDataSize()
        {
            return 4 + 4 + 1 + 2 + 4 + 4 + 4 + 4 + 4 + 4 + 2;
        }
    }
}