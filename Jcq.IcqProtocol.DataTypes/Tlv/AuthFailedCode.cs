// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthFailedCode.cs" company="Jan-Cornelius Molnar">
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