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
        private static readonly IcqStatusCode _Unknown = new IcqStatusCode("Unknown status", UserStatus.Offline, 8);

        public static IcqStatusCode Online { get; } = new IcqStatusCode("Online", UserStatus.Online, 0);

        public static IcqStatusCode Offline { get; } = new IcqStatusCode("Offline", UserStatus.Offline, 7);

        public static IcqStatusCode Occupied { get; } = new IcqStatusCode("Occupied", UserStatus.Occupied, 5);

        public static IcqStatusCode NotAvailable { get; } = new IcqStatusCode("Not Available", UserStatus.NotAvailable,
            6);

        public static IcqStatusCode Invisible { get; } = new IcqStatusCode("Invisible", UserStatus.Invisible, 2);

        public static IcqStatusCode Free4Chat { get; } = new IcqStatusCode("Free4Chat", UserStatus.FreeForChat, 1);

        public static IcqStatusCode Away { get; } = new IcqStatusCode("Away", UserStatus.Away, 3);

        public static IcqStatusCode DoNotDisturb { get; } = new IcqStatusCode("Do not disturb",
            UserStatus.DoNotDisturb, 4);

        public static IcqStatusCode GetStatusCode(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Away:
                    return Away;
                case UserStatus.DoNotDisturb:
                    return DoNotDisturb;
                case UserStatus.FreeForChat:
                    return Free4Chat;
                case UserStatus.Invisible:
                    return Invisible;
                case UserStatus.NotAvailable:
                    return NotAvailable;
                case UserStatus.Occupied:
                    return Occupied;
                case UserStatus.Offline:
                    return Offline;
                case UserStatus.Online:
                    return Online;
                default:
                    return _Unknown;
            }
        }
    }
}