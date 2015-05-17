// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContact.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Collections.Generic;

namespace Jcq.IcqProtocol.Contracts
{
    /// <summary>
    /// Defines a Contact.
    /// </summary>
    public interface IContact : IStorageItem
    {
        /// <summary>
        /// Gets the Group.
        /// </summary>
        IGroup Group { get; }

        DateTime MemberSince { get; set; }
        DateTime SignOnTime { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
        ContactGender Gender { get; set; }
        bool AuthorizationRequired { get; set; }
        List<byte> IconHash { get; }
        List<byte> IconData { get; }
        IStatusCode Status { get; set; }
        void SetIconHash(List<byte> value);
        void SetIconData(List<byte> value);

        /// <summary>
        /// Occurs when an IconHash is received.
        /// </summary>
        event EventHandler IconHashReceived;

        /// <summary>
        /// Occurs when an Icon is received.
        /// </summary>
        event EventHandler IconDataReceived;
    }

    public enum ContactGender : byte
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }
}