// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageService.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core.Interfaces;

namespace JCsTools.JCQ.IcqInterface.Interfaces
{
    public interface IStorageService : IContextService
    {
        bool IsContactListAvailable { get; }
        bool IsFreshContactList { get; }
        IContactListInfo Info { get; }
        INotifyingCollection<IGroup> Groups { get; }
        INotifyingCollection<IContact> Contacts { get; }
        IGroup MasterGroup { get; }
        event EventHandler ContactListActivated;
        event EventHandler<StatusChangedEventArgs> ContactStatusChanged;
        IContact GetContactByIdentifier(string identifier);
        IGroup GetGroupByIdentifier(string identifier);
        bool IsContactStored(IContact contact);
        void AddContact(IContact contact, IGroup @group);
        void AttachContact(IContact contact, IGroup @group, bool stored);
        void RemoveContact(IContact contact, IGroup @group);
        void UpdateContact(IContact contact);
        void AddGroup(IGroup @group);
        void RemoveGroup(IGroup @group);
        void UpdateGroup(IGroup @group);
        void RegisterLocalContactList(int itemCount, DateTime dateChanged);
    }
}