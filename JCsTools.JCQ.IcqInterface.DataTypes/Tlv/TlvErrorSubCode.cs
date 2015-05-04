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
  public class TlvErrorSubCode : Tlv
  {
    public TlvErrorSubCode() : base(0x8, 2)
    {
    }

    private int _ErrorSubCode;
    public int ErrorSubCode {
      get { return _ErrorSubCode; }
      set { _ErrorSubCode = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data;

      data = base.Serialize();
      data.AddRange(ByteConverter.GetBytes((ushort)ErrorSubCode));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ErrorSubCode = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  public enum ErrorCode : short
  {
    InvalidSNACHeader = 0x1,
    ServerRateLimitExceeded = 0x2,
    ClientRateLimitExceeded = 0x3,
    RecipientIsNotLoggedIn = 0x4,
    RequestedServiceUnavailable = 0x5,
    RequestedServiceNotDefined = 0x6,
    YouSentObsoleteSNAC = 0x7,
    NotSupportedByServer = 0x8,
    NotSupportedByClient = 0x9,
    RefusedByClient = 0xa,
    ReplyTooBig = 0xb,
    ResponsesLost = 0xc,
    RequestDenied = 0xd,
    IncorrectSNACFormat = 0xe,
    InsufficientRights = 0xf,
    InLocalPermitDenyRecipientBlocked = 0x10,
    SenderTooEvil = 0x11,
    ReceiverTooEvil = 0x12,
    UserTemporarilyUnavailable = 0x13,
    NoMatch = 0x14,
    ListOverflow = 0x15,
    RequestAmbiguous = 0x16,
    ServerQueueFull = 0x17,
    NotWhileOnAOL = 0x18
  }
}

