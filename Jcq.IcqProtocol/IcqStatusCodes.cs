// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqStatusCodes.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqStatusCodes
    {
        private static readonly IcqStatusCode _Online = new IcqStatusCode("Online", UserStatus.Online, 0);
        private static readonly IcqStatusCode _Free4Chat = new IcqStatusCode("Free4Chat", UserStatus.FreeForChat, 1);
        private static readonly IcqStatusCode _Invisible = new IcqStatusCode("Invisible", UserStatus.Invisible, 2);
        private static readonly IcqStatusCode _Away = new IcqStatusCode("Away", UserStatus.Away, 3);

        private static readonly IcqStatusCode _DoNotDisturb = new IcqStatusCode("Do not disturb",
            UserStatus.DoNotDisturb, 4);

        private static readonly IcqStatusCode _Occupied = new IcqStatusCode("Occupied", UserStatus.Occupied, 5);

        private static readonly IcqStatusCode _NotAvailable = new IcqStatusCode("Not Available", UserStatus.NotAvailable,
            6);

        private static readonly IcqStatusCode _Offline = new IcqStatusCode("Offline", UserStatus.Offline, 7);
        private static readonly IcqStatusCode _Unknown = new IcqStatusCode("Unknown status", UserStatus.Offline, 8);

        public static IcqStatusCode Online
        {
            get { return _Online; }
        }

        public static IcqStatusCode Offline
        {
            get { return _Offline; }
        }

        public static IcqStatusCode Occupied
        {
            get { return _Occupied; }
        }

        public static IcqStatusCode NotAvailable
        {
            get { return _NotAvailable; }
        }

        public static IcqStatusCode Invisible
        {
            get { return _Invisible; }
        }

        public static IcqStatusCode Free4Chat
        {
            get { return _Free4Chat; }
        }

        public static IcqStatusCode Away
        {
            get { return _Away; }
        }

        public static IcqStatusCode DoNotDisturb
        {
            get { return _DoNotDisturb; }
        }

        public static IcqStatusCode GetStatusCode(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Away:
                    return _Away;
                    break;
                case UserStatus.DoNotDisturb:
                    return _DoNotDisturb;
                    break;
                case UserStatus.FreeForChat:
                    return _Free4Chat;
                    break;
                case UserStatus.Invisible:
                    return _Invisible;
                    break;
                case UserStatus.NotAvailable:
                    return _NotAvailable;
                    break;
                case UserStatus.Occupied:
                    return _Occupied;
                    break;
                case UserStatus.Offline:
                    return _Offline;
                    break;
                case UserStatus.Online:
                    return _Online;
                    break;
                default:
                    return _Unknown;
                    break;
            }
        }
    }
}