// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Jan-Cornelius Molnar">
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
    public enum MessageType : byte
    {
        PlainTextMessage = 0x1,
        ChatRequestMessage = 0x2,
        FileRequestMessage = 0x3,
        URLMessage = 0x4,
        AuthorizationRequestMessage = 0x6,
        AuthorizationDeniedMessage = 0x7,
        AuthorizationGivenMessage = 0x8,
        MessageFromOSCARServer = 0x9,
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