// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="Jan-Cornelius Molnar">
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