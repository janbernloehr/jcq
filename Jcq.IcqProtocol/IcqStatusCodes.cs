// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqStatusCodes.cs" company="Jan-Cornelius Molnar">
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

using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol
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
                case UserStatus.DoNotDisturb:
                    return _DoNotDisturb;
                case UserStatus.FreeForChat:
                    return _Free4Chat;
                case UserStatus.Invisible:
                    return _Invisible;
                case UserStatus.NotAvailable:
                    return _NotAvailable;
                case UserStatus.Occupied:
                    return _Occupied;
                case UserStatus.Offline:
                    return _Offline;
                case UserStatus.Online:
                    return _Online;
                default:
                    return _Unknown;
            }
        }
    }
}