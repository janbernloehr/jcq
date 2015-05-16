// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaRequestSubType.cs" company="Jan-Cornelius Molnar">
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
    public enum MetaRequestSubType
    {
        SetUserBasicInfoRequest = 0x3ea,
        SetUserWorkInfoRequest = 0x3f3,
        SetUserMoreInfoRequest = 0x3fd,
        SetUserNotesInfoRequest = 0x406,
        SetUserExtendedEmailInfoRequest = 0x40b,
        SetUserInterestsInfoRequest = 0x410,
        SetUserAffilationsInfoRequest = 0x41a,
        SetUserPermissionsInfoRequest = 0x424,
        ChangeUserPasswordRequest = 0x42e,
        SetUserHomepageCategoryInfoRequest = 0x442,
        RequestFullUserInfo = 0x4b2,
        RequestShortUserInfo = 0x4ba,
        UnregisterUserRequest = 0x4c4,
        RequestFullUserInfo2 = 0x4d0,
        SearchByDetailsRequestPlain = 0x515,
        SearchByUinRequestPlain = 0x51f,
        SearchByEmailRequestPlain = 0x529,
        WhitePagesSearchRequestPlainSimple = 0x533,
        SearchByDetailsRequestPlainWildcard = 0x53d,
        SearchByEmailRequestPlainWildcard = 0x547,
        WhitePagesSearchRequestPlainWildcard = 0x551,
        SearchByUinRequestTlv = 0x569,
        WhitepagesSearchRequestTlv = 0x55f,
        SearchByEmailRequestTlv = 0x573,
        RandomChatUserSearchRequest = 0x74e,
        RequestServerVariableViaXml = 0x898,
        SendRegistrationStatsReport = 0xaa5,
        SendShortcutBarStatsReport = 0xaaf,
        SaveInfoTlvBasedRequest = 0xc3a,
        ClientSendSmsRequest = 0x1482,
        ClientSpamReportRequest = 0x2008
    }
}