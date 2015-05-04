// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContact.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.Interfaces
{
    public interface IContact : IStorageItem
    {
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
        event EventHandler IconHashReceived;
        event EventHandler IconDataReceived;
    }

    public enum ContactGender : byte
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }
}