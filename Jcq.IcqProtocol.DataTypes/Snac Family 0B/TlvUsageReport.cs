// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvUsageReport.cs" company="Jan-Cornelius Molnar">
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
    public class TlvUsageReport : Tlv
    {
        public TlvUsageReport() : base(0x9)
        {
        }

        public string ScreenName { get; set; }
        public int MachineClass { get; set; }
        public string OperatingSystem { get; set; }
        public Version OperatingSystemVersion { get; set; }
        public string ProcessorType { get; set; }
        public string WinsockDllDescription { get; set; }
        public Version WinsockDllVersion { get; set; }

        public override int CalculateDataSize()
        {
            return 2 + 2 + 16 + 1 + ScreenName.Length + 2 + 2 + OperatingSystem.Length + 2 +
                   OperatingSystemVersion.ToString().Length + 2 + ProcessorType.Length + 2 +
                   WinsockDllDescription.Length +
                   2 + WinsockDllVersion.ToString().Length + 12;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            //00 09  \t   \tword  \t   \tTLV.Type(0x09) - local configuration (?)
            //xx xx \t  \tword \t  \tTLV.Length
            //00 00 00 00
            //00 00 00 00
            //00 00 00 00
            //00 00 00 00 \t  \t16 bytes \t  \tunknown (zero values)
            //xx \t  \tbyte \t  \tlength of the screenname (uin) string
            //xx .. \t  \tascii \t  \tscreenname (uin) string
            //xx xx \t  \tword \t  \tunknown (machine Class ?)
            //xx xx \t  \tword \t  \tlength of the OS name
            //xx .. \t  \tascii \t  \tOS name (ex: Windows 2000)
            //xx xx \t  \tword \t  \tlength of the OS version string
            //xx .. \t  \tascii \t  \tOS version (ex: 5.1.2600)
            //xx xx \t  \tword \t  \tlength of the processor type
            //xx .. \t  \tascii \t  \tprocessor type (ex: Intel.Pentium)
            //xx xx \t  \tword \t  \tlength of the winsock description string
            //xx .. \t  \tascii \t  \twinsock DLL description string
            //xx xx \t  \tword \t  \tlength of the winsock version string
            //xx .. \t  \tascii \t  \twinsock DLL version (ex: 5.1.2600.0)
            //00 00 \t  \tword \t  \tUnknown field
            //00 02 \t  \tword \t  \tUnknown field
            //00 01 \t  \tword \t  \tUnknown field
            //00 01 \t  \tword \t  \tUnknown field
            //00 02 \t  \tword \t  \tUnknown field
            //00 02 \t  \tword \t  \tUnknown field

            data.AddRange(new byte[]
            {
                0,
                0,
                0,
                0
            });

            data.AddRange(ByteConverter.GetBytesForUInt32Date(DateTime.Now));

            data.AddRange(new byte[]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            });

            data.AddRange(ByteConverter.GetBytesForStringWithLeadingByteLength(ScreenName));

            data.AddRange(new byte[]
            {
                0,
                0
            });

            data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(OperatingSystem));

            data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(OperatingSystemVersion.ToString()));

            data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(ProcessorType));

            data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(WinsockDllDescription));

            data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(WinsockDllVersion.ToString()));

            data.AddRange(new byte[]
            {
                0,
                0
            });
            data.AddRange(new byte[]
            {
                0,
                2
            });
            data.AddRange(new byte[]
            {
                0,
                1
            });
            data.AddRange(new byte[]
            {
                0,
                1
            });
            data.AddRange(new byte[]
            {
                0,
                2
            });
            data.AddRange(new byte[]
            {
                0,
                2
            });

            return data;
        }
    }
}