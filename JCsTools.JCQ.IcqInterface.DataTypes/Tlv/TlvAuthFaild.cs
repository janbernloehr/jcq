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
  public class TlvAuthFailed : Tlv
  {
    public TlvAuthFailed() : base(0x8, 2)
    {
    }

    private AuthFailedCode _ErrorSubCode;
    public AuthFailedCode ErrorSubCode {
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

      _ErrorSubCode = (AuthFailedCode)(int)ByteConverter.ToUInt16(data.GetRange(index, 2));
    }

    public override int CalculateDataSize()
    {
      return 2;
    }
  }

  public enum AuthFailedCode
  {
    ServiceTemporarilyUnavailable = 0x2,
    AllOtherErrors = 0x3,
    IncorrectNickOrPasswordReEnter = 0x4,
    MismatchNickOrPasswordReEnter = 0x5,
    InternalClientErrorBadInputToAuthorizer = 0x6,
    InvalidAccount = 0x7,
    DeletedAccount = 0x8,
    ExpiredAccount = 0x9,
    NoAccessToDatabase = 0xa,
    NoAccessToResolver = 0xb,
    InvalidDatabaseFields = 0xc,
    BadDatabaseStatus = 0xd,
    BadResolverStatus = 0xe,
    InternalError = 0xf,
    ServiceTemporarilyOffline = 0x10,
    SuspendedAccount = 0x11,
    DBSendError = 0x12,
    DBLinkError = 0x13,
    ReservationMapError = 0x14,
    ReservationLinkError = 0x15,
    TheUsersNumConnectedFromThisIPHasReachedTheMaximum = 0x16,
    TheUsersNumConnectedFromThisIPHasReachedTheMaximumReservation = 0x17,
    RateLimitExceededReservationPleaseTryToReconnectInAFewMinutes = 0x18,
    UserTooHeavilyWarned = 0x19,
    ReservationTimeout = 0x1a,
    YouAreUsingAnOlderVersionOfICQUpgradeRequired = 0x1b,
    YouAreUsingAnOlderVersionOfICQUpgradeRecommended = 0x1c,
    RateLimitExceededPleaseTryToReconnectInAFewMinutes = 0x1d,
    CanTRegisterOnTheICQNetworkReconnectInAFewMinutes = 0x1e,
    InvalidSecurID = 0x20,
    AccountSuspendedBecauseOfYourAgeAge13 = 0x22
  }
}

