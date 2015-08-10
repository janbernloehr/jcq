// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqStorageService.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jcq.Core;
using Jcq.Core.Collections;
using Jcq.Core.Contracts;
using Jcq.Core.Contracts.Collections;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    public class IcqStorageService : ContextService, IStorageService
    {
        public delegate void ContactListActivatedEventHandler(object sender, EventArgs e);

        public delegate void ContactStatusChangedEventHandler(object sender, StatusChangedEventArgs e);

        private readonly KeyedNotifiyingCollection<string, IcqContact> _allContacts =
            new KeyedNotifiyingCollection<string, IcqContact>(c => c.Identifier);

        private readonly KeyedNotifiyingCollection<string, IcqGroup> _groups =
            new KeyedNotifiyingCollection<string, IcqGroup>(g => g.Identifier);

        private readonly KeyedNotifiyingCollection<string, IcqContact> _storedContacts =
            new KeyedNotifiyingCollection<string, IcqContact>(c => c.Identifier);

        private SSIActionResultCode _codeSsiAkk;
        private int _maxSsiItemId;
        private SemaphoreSlim _transactionSemaphore;

        public IcqStorageService(IContext context)
            : base(context)
        {
            _visibleList = new NotifyingCollection<IContact>();

            VisibleList = new ReadOnlyNotifyingCollection<IContact>(_visibleList);
            _invisibleList = new NotifyingCollection<IContact>();

            InvisibleList =
                new ReadOnlyNotifyingCollection<IContact>(_invisibleList);

            _ignoreList = new NotifyingCollection<IContact>();

            IgnoreList =
                new ReadOnlyNotifyingCollection<IContact>(_ignoreList);


            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            _allContacts.Add((IcqContact) context.Identity);

            context.Identity.PropertyChanged += OnContactPropertyChanged;

            connector.RegisterSnacHandler(0x13, 0x6, new Action<Snac1306>(AnalyseSnac1306));
            connector.RegisterSnacHandler(0x13, 0x8, new Action<Snac1308>(AnalyseSnac1308));
            connector.RegisterSnacHandler(0x13, 0xa, new Action<Snac130A>(AnalyseSnac130A));
            connector.RegisterSnacHandler(0x13, 0xe, new Action<Snac130E>(AnalyseSnac130E));
            connector.RegisterSnacHandler(0x13, 0xf, new Action<Snac130F>(AnalyseSnac130F));
        }

        public IcqGroup MasterGroup { get; private set; }

        public void RegisterLocalContactList(int itemCount, DateTime dateChanged)
        {
            Info = new ContactListInfo(itemCount, dateChanged);
            IsFreshContactList = false;
        }

        public IContactListInfo Info { get; private set; }

        public bool IsContactListAvailable
        {
            get { return Info != null; }
        }

        public bool IsFreshContactList { get; private set; } = true;

        public event EventHandler ContactListActivated;
        public event EventHandler<StatusChangedEventArgs> ContactStatusChanged;

        IGroup IStorageService.MasterGroup
        {
            get { return MasterGroup; }
        }

        IReadOnlyNotifyingCollection<IContact> IStorageService.Contacts
        {
            get { return _storedContacts; }
        }

        IReadOnlyNotifyingCollection<IGroup> IStorageService.Groups
        {
            get { return _groups; }
        }

        IContact IStorageService.GetContactByIdentifier(string identifier)
        {
            return GetContactByIdentifier(identifier);
        }

        IGroup IStorageService.GetGroupByIdentifier(string identifier)
        {
            return GetGroupByIdentifier(identifier);
        }

        public bool IsContactStored(IContact contact)
        {
            var icqContact = contact as IcqContact;

            if (icqContact == null)
                throw new ArgumentException("Argument of type IcqContact required", nameof(contact));

            return IsContactStored(icqContact);
        }

        public Task AddContact(IContact contact, IGroup group)
        {
            var icqContact = contact as IcqContact;
            var icqGroup = group as IcqGroup;

            if (icqContact == null)
                throw new ArgumentException("Argument of type IcqContact required", nameof(contact));

            if (icqGroup == null)
                throw new ArgumentException("Argument of type IcqGroup required", nameof(@group));

            return AddContact(icqContact, icqGroup);
        }

        public void AttachContact(IContact contact, IGroup group, bool stored)
        {
            var icqContact = contact as IcqContact;
            var icqGroup = group as IcqGroup;

            if (icqContact == null)
                throw new ArgumentException("Argument of type IcqContact required", nameof(contact));

            if (icqGroup == null)
                throw new ArgumentException("Argument of type IcqGroup required", nameof(@group));

            AttachContact(icqContact, icqGroup, stored);
        }

        public Task RemoveContact(IContact contact, IGroup group)
        {
            var icqContact = contact as IcqContact;
            var icqGroup = group as IcqGroup;

            if (icqContact == null)
                throw new ArgumentException("Argument of type IcqContact required", nameof(contact));

            if (icqGroup == null)
                throw new ArgumentException("Argument of type IcqGroup required", nameof(@group));

            return RemoveContact(icqContact, icqGroup);
        }

        public Task UpdateContact(IContact contact)
        {
            var icqContact = contact as IcqContact;

            if (icqContact == null)
                throw new ArgumentException("Argument of type IcqContact required", nameof(contact));

            return UpdateContact(icqContact);
        }

        public Task AddGroup(IGroup group)
        {
            var icqGroup = group as IcqGroup;

            if (icqGroup == null)
                throw new ArgumentException("Argument of type IcqGroup required", nameof(@group));

            return AddGroup(icqGroup);
        }

        public Task RemoveGroup(IGroup group)
        {
            var icqGroup = group as IcqGroup;

            if (icqGroup == null)
                throw new ArgumentException("Argument of type IcqGroup required", nameof(@group));

            return RemoveGroup(icqGroup);
        }

        public Task UpdateGroup(IGroup group)
        {
            var icqGroup = group as IcqGroup;

            if (icqGroup == null)
                throw new ArgumentException("Argument of type IcqGroup required", nameof(@group));

            return UpdateGroup(icqGroup);
        }

        public IcqContact GetContactByIdentifier(string identifier)
        {
            IcqContact contact;

            if (!_allContacts.Contains(identifier))
            {
                contact = new IcqContact(identifier, identifier);

                _allContacts.Add(contact);

                contact.PropertyChanged += OnContactPropertyChanged;
            }
            else
            {
                contact = _allContacts[identifier];
            }

            return contact;
        }

        public IcqGroup GetGroupByIdentifier(string identifier)
        {
            return _groups.Contains(identifier) ? _groups[identifier] : null;
        }

        public Task AddContact(IcqContact contact, IcqGroup group)
        {
            var trans = new AddContactTransaction(this, contact, group);

            return CommitSSITransaction(trans);
        }

        public Task RemoveContact(IcqContact contact, IcqGroup group)
        {
            var trans = new RemoveContactTransaction(this, contact);

            return CommitSSITransaction(trans);
        }

        public void AttachContact(IcqContact contact, IcqGroup group, bool stored)
        {
            if (!_allContacts.Contains(contact.Identifier))
            {
                _allContacts.Add(contact);

                contact.PropertyChanged += OnContactPropertyChanged;
            }

            if (!stored) return;

            if (!_storedContacts.Contains(contact.Identifier))
                _storedContacts.Add(contact);

            if (!group.Contacts.Contains(contact))
                group.Contacts.Add(contact);
        }

        public Task AddGroup(IcqGroup group)
        {
            throw new NotImplementedException();
        }

        public Task RemoveGroup(IcqGroup group)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContact(IcqContact contact)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroup(IcqGroup group)
        {
            throw new NotImplementedException();
        }

        public bool IsContactStored(IcqContact contact)
        {
            return _storedContacts.Contains(contact);
        }

        private IcqGroup GetGroupByGroupId(int groupId)
        {
            return _groups.Cast<IcqGroup>().FirstOrDefault(g => g.GroupId == groupId);
        }

        public int GetNextSsiItemId()
        {
            return Interlocked.Increment(ref _maxSsiItemId);
        }

        internal void InnerAddContactToStorage(IcqContact contact, IcqGroup group)
        {
            if (!_storedContacts.Contains(contact.Identifier))
                _storedContacts.Add(contact);

            contact.SetGroup(group);

            if (!contact.Group.Contacts.Contains(contact))
                group.Contacts.Add(contact);
        }

        internal void InnerAddContactToVisibleList(IcqContact contact)
        {
            if (!_visibleList.Contains(contact))
                _visibleList.Add(contact);
        }

        internal void InnerAddContactToInvisibleList(IcqContact contact)
        {
            if (!_invisibleList.Contains(contact))
                _invisibleList.Add(contact);
        }

        internal void InnerAddContactToIgnoreList(IcqContact contact)
        {
            if (!_ignoreList.Contains(contact))
                _ignoreList.Add(contact);
        }

        internal void InnerRemoveContactFromStorage(IcqContact contact)
        {
            if (!_storedContacts.Contains(contact.Identifier))
                _storedContacts.Remove(contact.Identifier);

            if (contact.Group.Contacts.Contains(contact))
                contact.Group.Contacts.Remove(contact);
        }

        internal void InnerRemoveContactFromVisibleList(IcqContact contact)
        {
            if (_visibleList.Contains(contact))
                _visibleList.Remove(contact);
        }

        internal void InnerRemoveContactFromInvisibleList(IcqContact contact)
        {
            if (_invisibleList.Contains(contact))
                _invisibleList.Remove(contact);
        }

        internal void InnerRemoveContactFromIgnoreList(IcqContact contact)
        {
            if (_ignoreList.Contains(contact))
                _ignoreList.Remove(contact);
        }

        public async Task CommitSSITransaction(ISsiTransaction transaction)
        {
            // Check wheter all prerequirements are met to commit the transaction.
            transaction.Validate();

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

            // Create the transaction data
            var beginTransaction = new Snac1311();
            var item = transaction.CreateSnac();
            var endTransaction = new Snac1312();

            //_transactionCompletionSource = new TaskCompletionSource<SSIActionResultCode>();
            // TODO: Check that there are no concurrent transactions!
            _transactionSemaphore = new SemaphoreSlim(0, 1);

            // Send data.
            await transfer.Send(beginTransaction, item, endTransaction);

            // Wait for server response.

            if (!await _transactionSemaphore.WaitAsync(TimeSpan.FromSeconds(5)))
            {
                throw new TimeoutException("Server did not respond.");
            }

            transaction.OnComplete(_codeSsiAkk);

            _transactionSemaphore = null;
        }

        public void DetachContact(IcqContact contact, IcqGroup group)
        {
            if (group.Contacts.Contains(contact))
                group.Contacts.Add(contact);

            if (_storedContacts.Contains(contact.Identifier))
                _storedContacts.Add(contact);
        }

        internal void AnalyseSnac130E(Snac130E dataIn)
        {
            if (_transactionSemaphore != null)
            {
                _codeSsiAkk = dataIn.ActionResultCodes.First();
                //_waitSsiAkk.Set();
                _transactionSemaphore.Release();
            }

            foreach (var code in dataIn.ActionResultCodes)
            {
                Debug.WriteLine(string.Format("SSI Change Akk: {0}", code), "IcqStorageService");
            }
        }

        internal void AnalyseSnac1306(Snac1306 dataIn)
        {
            try
            {
                _maxSsiItemId = Math.Max(_maxSsiItemId, dataIn.MaxItemId);

                foreach (var ssiGroup in dataIn.GroupRecords)
                {
                    var identifier = ssiGroup.ItemName;

                    var group = new IcqGroup(identifier, ssiGroup.GroupId);

                    if (group.GroupId == 0)
                    {
                        MasterGroup = group;
                    }
                    else
                    {
                        _groups.Add(group);
                    }
                }

                foreach (var x in _groups)
                {
                    MasterGroup.Groups.Add(x);
                }

                foreach (var ssiContact in dataIn.BuddyRecords)
                {
                    var identifier = ssiContact.ItemName;
                    var group = GetGroupByGroupId(ssiContact.GroupId);

                    int identifierId;

                    if (!int.TryParse(identifier, out identifierId))
                        continue;

                    var contact = GetContactByIdentifier(identifier);

                    if (contact.LastShortUserInfoRequest <= DateTime.MinValue)
                        contact.Name = ssiContact.LocalScreenName.LocalScreenName;
                    contact.ItemId = ssiContact.ItemId;
                    contact.SetGroup(group);

                    AttachContact(contact, group, true);
                }

                foreach (var record in dataIn.DenyRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    contact.DenyRecordItemId = record.ItemId;

                    _invisibleList.Add(contact);
                }

                foreach (var record in dataIn.PermitRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    contact.PermitRecordItemId = record.ItemId;

                    _visibleList.Add(contact);
                }

                foreach (var record in dataIn.IgnoreListRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    contact.IgnoreRecordItemId = record.ItemId;

                    _ignoreList.Add(contact);
                }

                Info = new ContactListInfo(dataIn.ItemCount, dataIn.LastChange);

                ContactListActivated?.Invoke(this, EventArgs.Empty);

                Kernel.Logger.Log("IcqStorageService", TraceEventType.Verbose, "Contact list activated.");
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        internal void AnalyseSnac130F(Snac130F dataIn)
        {
            try
            {
                Info = new ContactListInfo(dataIn.NumberOfItems, dataIn.ModificationDate);

                ContactListActivated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        internal void AnalyseSnac1308(Snac1308 dataIn)
        {
            // Sever informs the client that items have beend added to the SSI Store.
            try
            {
                foreach (var ssiGroup in dataIn.GroupRecords)
                {
                    var identifier = ssiGroup.ItemName;

                    var group = new IcqGroup(identifier, ssiGroup.GroupId);

                    _groups.Add(group);
                }

                foreach (var ssiContact in dataIn.BuddyRecords)
                {
                    var identifier = ssiContact.ItemName;
                    var group = GetGroupByGroupId(ssiContact.GroupId);

                    int identifierId;

                    if (!int.TryParse(identifier, out identifierId))
                        continue;

                    var contact = GetContactByIdentifier(identifier);

                    if (contact.LastShortUserInfoRequest <= DateTime.MinValue)
                        contact.Name = ssiContact.LocalScreenName.LocalScreenName;
                    contact.ItemId = ssiContact.ItemId;
                    contact.SetGroup(group);

                    AttachContact(contact, group, true);
                }

                foreach (var record in dataIn.DenyRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    contact.DenyRecordItemId = record.ItemId;

                    _invisibleList.Add(contact);
                }

                foreach (var record in dataIn.PermitRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    contact.PermitRecordItemId = record.ItemId;

                    _visibleList.Add(contact);
                }

                foreach (var record in dataIn.IgnoreListRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    contact.IgnoreRecordItemId = record.ItemId;

                    _ignoreList.Add(contact);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        internal void AnalyseSnac130A(Snac130A dataIn)
        {
            // Sever informs the client that items have beend removed from the SSI Store.

            try
            {
                foreach (var contact in dataIn.DenyRecords.Select(record => GetContactByIdentifier(record.ItemName)))
                {
                    contact.DenyRecordItemId = 0;

                    _invisibleList.Remove(contact);
                }

                foreach (var contact in dataIn.PermitRecords.Select(record => GetContactByIdentifier(record.ItemName)))
                {
                    contact.PermitRecordItemId = 0;

                    _visibleList.Remove(contact);
                }

                foreach (
                    var contact in dataIn.IgnoreListRecords.Select(record => GetContactByIdentifier(record.ItemName)))
                {
                    contact.IgnoreRecordItemId = 0;

                    _ignoreList.Remove(contact);
                }

                foreach (var ssiContact in dataIn.BuddyRecords)
                {
                    var identifier = ssiContact.ItemName;
                    var group = GetGroupByGroupId(ssiContact.GroupId);

                    int identifierId;

                    if (!int.TryParse(identifier, out identifierId))
                        continue;

                    var contact = GetContactByIdentifier(identifier);

                    DetachContact(contact, group);
                }

                foreach (var ssiGroup in dataIn.GroupRecords)
                {
                    var identifier = ssiGroup.ItemName;

                    var group = new IcqGroup(identifier, ssiGroup.GroupId);

                    _groups.Remove(group);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnContactPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Status") return;

            var contact = sender as IContact;

            if (contact == null) return;

            var args = new StatusChangedEventArgs(contact.Status, contact);

            ContactStatusChanged?.Invoke(this, args);
        }

        #region  Privacy Lists

        private readonly NotifyingCollection<IContact> _visibleList;

        public ReadOnlyNotifyingCollection<IContact> VisibleList { get; }

        private readonly NotifyingCollection<IContact> _invisibleList;

        public ReadOnlyNotifyingCollection<IContact> InvisibleList { get; }

        private readonly NotifyingCollection<IContact> _ignoreList;

        public ReadOnlyNotifyingCollection<IContact> IgnoreList { get; }

        #endregion
    }
}