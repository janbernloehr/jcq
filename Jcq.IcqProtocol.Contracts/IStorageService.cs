// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageService.cs" company="Jan-Cornelius Molnar">
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
using System.Threading.Tasks;
using JCsTools.Core.Interfaces;

namespace JCsTools.JCQ.IcqInterface.Interfaces
{
    public interface IStorageService : IContextService
    {
        bool IsContactListAvailable { get; }
        bool IsFreshContactList { get; }
        IContactListInfo Info { get; }
        IReadOnlyNotifyingCollection<IGroup> Groups { get; }
        IReadOnlyNotifyingCollection<IContact> Contacts { get; }
        IGroup MasterGroup { get; }
        event EventHandler ContactListActivated;
        event EventHandler<StatusChangedEventArgs> ContactStatusChanged;
        IContact GetContactByIdentifier(string identifier);
        IGroup GetGroupByIdentifier(string identifier);
        bool IsContactStored(IContact contact);
        Task AddContact(IContact contact, IGroup @group);
        void AttachContact(IContact contact, IGroup @group, bool stored);
        Task RemoveContact(IContact contact, IGroup @group);
        Task UpdateContact(IContact contact);
        Task AddGroup(IGroup @group);
        Task RemoveGroup(IGroup @group);
        Task UpdateGroup(IGroup @group);
        void RegisterLocalContactList(int itemCount, DateTime dateChanged);
    }
}