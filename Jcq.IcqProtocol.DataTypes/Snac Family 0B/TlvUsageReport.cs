// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvUsageReport.cs" company="Jan-Cornelius Molnar">
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