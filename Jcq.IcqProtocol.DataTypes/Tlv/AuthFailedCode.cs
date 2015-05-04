// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthFailedCode.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
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