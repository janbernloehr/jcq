// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaResponseSubType.cs" company="Jan-Cornelius Molnar">
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
    public enum MetaResponseSubType
    {
        MetaProcessingErrorServerReply = 0x1,
        SetUserHomeInfoServerAck = 0x64,
        SetUserWorkInfoServerAck = 0x6e,
        SetUserMoreInfoServerAck = 0x78,
        SetUserNotesInfoServerAck = 0x82,
        SetUserEmailSInfoServerAck = 0x87,
        SetUserInterestsInfoServerAck = 0x8c,
        SetUserAffilationsInfoServerAck = 0x96,
        ServerSmsResponseDeliveryReceipt = 0x96,
        SetUserPermissionsServerAck = 0xa0,
        SetUserPasswordServerAck = 0xaa,
        UnregisterAccountServerAck = 0xb4,
        SetUserHomepageCategoryServerAck = 0xbe,
        UserBasicInfoReply = 0xc8,
        UserWorkInfoReply = 0xd2,
        UserMoreInfoReply = 0xdc,
        UserNotesAboutInfoReply = 0xe6,
        UserExtendedEmailInfoReply = 0xeb,
        UserInterestsInfoReply = 0xf0,
        UserPastAffilationsInfoReply = 0xfa,
        ShortUserInformationReply = 0x104,
        UserHomepageCategoryInformationReply = 0x10e,
        SearchUserFoundReply = 0x1a4,
        SearchLastUserFoundReply = 0x1ae,
        RegistrationStatsAck = 0x302,
        RandomSearchServerReply = 0x366,
        ServerVariableRequestedViaXml = 0x8a2,
        ServerAckForSetFullinfoCommand = 0xc3f,
        ServerAckForUserSpamReport = 0x2012
    }
}