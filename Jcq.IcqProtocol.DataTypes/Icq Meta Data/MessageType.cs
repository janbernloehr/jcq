// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Jan-Cornelius Molnar">
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
    public enum MessageType : byte
    {
        PlainTextMessage = 0x1,
        ChatRequestMessage = 0x2,
        FileRequestMessage = 0x3,
        UrlMessage = 0x4,
        AuthorizationRequestMessage = 0x6,
        AuthorizationDeniedMessage = 0x7,
        AuthorizationGivenMessage = 0x8,
        MessageFromOscarServer = 0x9,
        YouWereAddedMessage = 0xc,
        WebPagerMessage = 0xd,
        EmailExpressMessage = 0xe,
        ContactListMessage = 0x13,
        PluginMessage = 0x1a,
        AutoAwayMessage = 0xe8,
        AutoOccupiedMessage = 0xe9,
        AutoNotAvailableMessage = 0xea,
        AutoDoNotDisturbMessage = 0xeb,
        AutoFreeForChatMessage = 0xec
    }
}