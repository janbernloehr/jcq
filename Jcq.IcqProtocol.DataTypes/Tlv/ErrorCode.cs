// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="Jan-Cornelius Molnar">
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