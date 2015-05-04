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
/// <summary>
/// The client uses Snac0B03 to send a usage report to the server.
/// Sent by: Client
/// Protocoll: AIM, ICQ
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public class Snac0B03 : Snac
  {
    public Snac0B03() : base(0xb, 0x3)
    {
    }

    public override int CalculateDataSize()
    {
      return TlvUsageReport.CalculateTotalSize;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize;

      data.AddRange(TlvUsageReport.Serialize);

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    private readonly TlvUsageReport _TlvUsageReport = new TlvUsageReport();
    public TlvUsageReport TlvUsageReport {
      get { return _TlvUsageReport; }
    }
  }

  public class TlvUsageReport : Tlv
  {
    public TlvUsageReport() : base(0x9)
    {
    }

    public override int CalculateDataSize()
    {
      return 2 + 2 + 16 + 1 + ScreenName.Length + 2 + 2 + OperatingSystem.Length + 2 + OperatingSystemVersion.ToString.Length + 2 + ProcessorType.Length + 2 + WinsockDllDescription.Length + 2 + WinsockDllVersion.ToString.Length + 12;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize;

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

      data.AddRange(new byte[] {
        0,
        0,
        0,
        0
      });

      data.AddRange(ByteConverter.GetBytesForUInt32Date(System.DateTime.Now));

      data.AddRange(new byte[] {
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

      data.AddRange(new byte[] {
        0,
        0
      });

      data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(OperatingSystem));

      data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(OperatingSystemVersion.ToString));

      data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(ProcessorType));

      data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(WinsockDllDescription));

      data.AddRange(ByteConverter.GetBytesForStringWithLeadingUInt16Length(WinsockDllVersion.ToString));

      data.AddRange(new byte[] {
        0,
        0
      });
      data.AddRange(new byte[] {
        0,
        2
      });
      data.AddRange(new byte[] {
        0,
        1
      });
      data.AddRange(new byte[] {
        0,
        1
      });
      data.AddRange(new byte[] {
        0,
        2
      });
      data.AddRange(new byte[] {
        0,
        2
      });

      return data;
    }



    private string _ScreenName;
    public string ScreenName {
      get { return _ScreenName; }
      set { _ScreenName = value; }
    }

    private int _MachineClass;
    public int MachineClass {
      get { return _MachineClass; }
      set { _MachineClass = value; }
    }

    private string _OperatingSystem;
    public string OperatingSystem {
      get { return _OperatingSystem; }
      set { _OperatingSystem = value; }
    }

    private Version _OperatingSystemVersion;
    public Version OperatingSystemVersion {
      get { return _OperatingSystemVersion; }
      set { _OperatingSystemVersion = value; }
    }

    private string _ProcessorType;
    public string ProcessorType {
      get { return _ProcessorType; }
      set { _ProcessorType = value; }
    }

    private string _WinsockDllDescription;
    public string WinsockDllDescription {
      get { return _WinsockDllDescription; }
      set { _WinsockDllDescription = value; }
    }

    private Version _WinsockDllVersion;
    public Version WinsockDllVersion {
      get { return _WinsockDllVersion; }
      set { _WinsockDllVersion = value; }
    }
  }
}

