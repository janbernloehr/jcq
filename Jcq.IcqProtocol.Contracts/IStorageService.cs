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
using Jcq.Core.Contracts.Collections;

namespace Jcq.IcqProtocol.Contracts
{
    /// <summary>
    ///     Defines a service contract to access the server side storage.
    /// </summary>
    public interface IStorageService : IContextService
    {
        /// <summary>
        ///     Gets a value indicating whether the contact list has been loaded.
        /// </summary>
        bool IsContactListAvailable { get; }

        /// <summary>
        ///     Gets a value indicating whether the contact list is up to date.
        /// </summary>
        bool IsFreshContactList { get; }

        /// <summary>
        ///     Gets the contact list statistics.
        /// </summary>
        IContactListInfo Info { get; }

        /// <summary>
        ///     Gets a list of this identity's contact groups.
        /// </summary>
        IReadOnlyNotifyingCollection<IGroup> Groups { get; }

        /// <summary>
        ///     Gets a list of this identity's contacts.
        /// </summary>
        IReadOnlyNotifyingCollection<IContact> Contacts { get; }

        /// <summary>
        ///     Gets the master group.
        /// </summary>
        IGroup MasterGroup { get; }

        /// <summary>
        ///     Occurs when the contact list is activated.
        /// </summary>
        event EventHandler ContactListActivated;

        /// <summary>
        ///     Occurs when a contact changes status.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> ContactStatusChanged;

        /// <summary>
        ///     Get an IContact by its identifier.
        /// </summary>
        /// <param name="identifier">The contact unique identifier.</param>
        /// <returns></returns>
        IContact GetContactByIdentifier(string identifier);

        /// <summary>
        ///     Get an IGroup by its identifier.
        /// </summary>
        /// <param name="identifier">The contact unique identifier.</param>
        /// <returns></returns>
        IGroup GetGroupByIdentifier(string identifier);

        /// <summary>
        ///     Gets a value indicating whether the given contact is stored in the server side storage.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>True if the contact is stored, otherwise false.</returns>
        bool IsContactStored(IContact contact);

        /// <summary>
        ///     Add the given contact to the server side storage.
        /// </summary>
        /// <param name="contact">The contact to add.</param>
        /// <param name="group">The group the contact should be added to.</param>
        /// <returns></returns>
        Task AddContact(IContact contact, IGroup group);

        /// <summary>
        ///     Attach the given contact without adding it to server side storage.
        /// </summary>
        /// <param name="contact">The contact to add.</param>
        /// <param name="group">The group the contact should be added to.</param>
        /// <param name="stored">A value indicating whether the contact exists in the server side storage.</param>
        /// <returns></returns>
        void AttachContact(IContact contact, IGroup group, bool stored);

        /// <summary>
        ///     Add the given contact from the server side storage.
        /// </summary>
        /// <param name="contact">The contact to remove.</param>
        /// <param name="group">The group the contact should be removed from.</param>
        /// <returns></returns>
        Task RemoveContact(IContact contact, IGroup group);

        /// <summary>
        ///     Update the server side storage data of the given contact.
        /// </summary>
        /// <param name="contact">The contact to update.</param>
        /// <returns></returns>
        Task UpdateContact(IContact contact);


        /// <summary>
        ///     Add a group to the server side storage.
        /// </summary>
        /// <param name="group">The group to add.</param>
        /// <returns></returns>
        Task AddGroup(IGroup group);

        /// <summary>
        ///     Remove a group from the server side storage.
        /// </summary>
        /// <param name="group">The group to remove.</param>
        /// <returns></returns>
        Task RemoveGroup(IGroup group);

        /// <summary>
        ///     Update the server side storage data of the given group.
        /// </summary>
        /// <param name="group">The group to update.</param>
        /// <returns></returns>
        Task UpdateGroup(IGroup group);


        void RegisterLocalContactList(int itemCount, DateTime dateChanged);
    }
}