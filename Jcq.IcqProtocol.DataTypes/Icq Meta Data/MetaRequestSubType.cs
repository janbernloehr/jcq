// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaRequestSubType.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.DataTypes
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