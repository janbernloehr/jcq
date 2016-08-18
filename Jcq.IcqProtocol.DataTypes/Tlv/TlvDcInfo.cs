// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvDcInfo.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;

namespace Jcq.IcqProtocol.DataTypes
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

        public override int Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

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
            catch (Exception)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }

            return index;
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