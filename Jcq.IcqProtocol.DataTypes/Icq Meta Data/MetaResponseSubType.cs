// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaResponseSubType.cs" company="Jan-Cornelius Molnar">
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