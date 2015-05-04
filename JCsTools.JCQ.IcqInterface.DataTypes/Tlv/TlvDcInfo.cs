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
  public class TlvDcInfo : Tlv
  {
    public TlvDcInfo() : base(0xc)
    {
    }

    private long _DcInternalIpAddress;
    public long DcInternalIpAddress {
      get { return _DcInternalIpAddress; }
      set { _DcInternalIpAddress = value; }
    }

    private int _DcPort;
    public int DcPort {
      get { return _DcPort; }
      set { _DcPort = value; }
    }

    private DcType _DCType;
    public DcType DcByte {
      get { return _DCType; }
      set { _DCType = value; }
    }

    private int _DcProtocolVersion;
    public int DcProtocolVersion {
      get { return _DcProtocolVersion; }
      set { _DcProtocolVersion = value; }
    }

    private long _DcAuthCookie;
    public long DcAuthCookie {
      get { return _DcAuthCookie; }
      set { _DcAuthCookie = value; }
    }

    private long _WebFrontPort;
    public long WebFrontPort {
      get { return _WebFrontPort; }
      set { _WebFrontPort = value; }
    }

    private DateTime _LastInfoUpdate;
    public DateTime LastInfoUpdate {
      get { return _LastInfoUpdate; }
      set { _LastInfoUpdate = value; }
    }

    private System.DateTime _LastExtInfoUpdate;
    public System.DateTime LastExtInfoUpdate {
      get { return _LastExtInfoUpdate; }
      set { _LastExtInfoUpdate = value; }
    }

    private System.DateTime _LastExtStatusUpdate;
    public System.DateTime LastExtStatusUpdate {
      get { return _LastExtStatusUpdate; }
      set { _LastExtStatusUpdate = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      try {

        _DcInternalIpAddress = ByteConverter.ToUInt32(data.GetRange(index, 4));
        index += 4;

        _DcPort = (int)ByteConverter.ToUInt32(data.GetRange(index, 4));
        index += 4;

        _DCType = (DcType)data(index);
        index += 1;

        _DcProtocolVersion = ByteConverter.ToUInt16(data.GetRange(index, 2));
        index += 2;

        _DcAuthCookie = ByteConverter.ToUInt32(data.GetRange(index, 4));
        index += 4;

        _WebFrontPort = ByteConverter.ToUInt32(data.GetRange(index, 4));
        index += 4;

        //Client futures 
        index += 4;

        _LastInfoUpdate = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
        index += 4;

        _LastExtInfoUpdate = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
        index += 4;

        _LastExtStatusUpdate = ByteConverter.ToDateTimeFromUInt32FileStamp(data.GetRange(index, 4));
        index += 4;
      } catch (Exception ex) {
        if (Debugger.IsAttached)
          Debugger.Break();
      }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)_DcInternalIpAddress));
      data.AddRange(ByteConverter.GetBytes((uint)_DcPort));
      data.Add(_DCType);
      data.AddRange(ByteConverter.GetBytes((uint)_DcProtocolVersion));
      data.AddRange(ByteConverter.GetBytes((uint)_DcAuthCookie));
      data.AddRange(ByteConverter.GetBytes((uint)_WebFrontPort));
      data.AddRange(ByteConverter.GetBytes((uint)3));
      data.AddRange(ByteConverter.GetBytesForUInt32Date(_LastInfoUpdate));
      data.AddRange(ByteConverter.GetBytesForUInt32Date(_LastExtInfoUpdate));
      data.AddRange(ByteConverter.GetBytesForUInt32Date(_LastExtStatusUpdate));

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

  public enum DcType : byte
  {
    DirectConnectionDisabledAuthRequired = 0x0,
    DirectConnectionThruFirewallOrHttpsProxy = 0x1,
    DirectConnectionThruSocks45ProxyServer = 0x2,
    NormalDirectConnectionWithoutProxyFirewall = 0x4,
    WebClientNoDirectConnection = 0x6
  }
}

