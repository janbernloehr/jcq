// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqStorageService.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using JCsTools.Core;
using JCsTools.Core.Interfaces;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqStorageService : ContextService, IStorageService
    {
        public delegate void ContactListActivatedEventHandler(object sender, EventArgs e);

        public delegate void ContactStatusChangedEventHandler(object sender, StatusChangedEventArgs e);

        private readonly KeyedNotifiyingCollection<string, IContact> _AllContacts =
            new KeyedNotifiyingCollection<string, IContact>(c => c.Identifier);

        private readonly KeyedNotifiyingCollection<string, IGroup> _Groups =
            new KeyedNotifiyingCollection<string, IGroup>(g => g.Identifier);

        private readonly KeyedNotifiyingCollection<string, IContact> _StoredContacts =
            new KeyedNotifiyingCollection<string, IContact>(c => c.Identifier);

        private readonly ManualResetEvent waitSSIAkk = new ManualResetEvent(false);
        private bool _IsFreshContactList = true;
        private int _MaxSSIItemId;
        private SSIActionResultCode codeSSIAkk;

        public IcqStorageService(IContext context)
            : base(context)
        {
            _visibleList = new NotifyingCollection<IContact>();

            _readOnlyVisibleList = new ReadOnlyNotifyingCollection<IContact>(_visibleList);
            _invisibleList = new NotifyingCollection<IContact>();

            _readOnlyInvisibleList =
                new ReadOnlyNotifyingCollection<IContact>(_invisibleList);

            _ignoreList = new NotifyingCollection<IContact>();

            _readOnlyIgnoreList =
                new ReadOnlyNotifyingCollection<IContact>(_ignoreList);


            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            _AllContacts.Add(context.Identity);

            context.Identity.PropertyChanged += OnContactPropertyChanged;

            connector.RegisterSnacHandler(0x13, 0x6, new Action<Snac1306>(AnalyseSnac1306));
            connector.RegisterSnacHandler(0x13, 0x8, new Action<Snac1308>(AnalyseSnac1308));
            connector.RegisterSnacHandler(0x13, 0xa, new Action<Snac130A>(AnalyseSnac130A));
            connector.RegisterSnacHandler(0x13, 0xe, new Action<Snac130E>(AnalyseSnac130E));
            connector.RegisterSnacHandler(0x13, 0xf, new Action<Snac130F>(AnalyseSnac130F));
        }

        public IContact GetContactByIdentifier(string identifier)
        {
            IContact contact;

            if (!_AllContacts.Contains(identifier))
            {
                contact = new IcqContact(identifier, identifier);

                _AllContacts.Add(contact);

                contact.PropertyChanged += OnContactPropertyChanged;
            }
            else
            {
                contact = _AllContacts[identifier];
            }

            return contact;
        }

        public IGroup GetGroupByIdentifier(string identifier)
        {
            if (_Groups.Contains(identifier))
                return _Groups[identifier];
            return null;
        }

        public void AddContact(IContact contact, IGroup @group)
        {
            AddContactTransaction trans;
            IcqContact icontact;
            IcqGroup igroup;

            icontact = (IcqContact) contact;
            igroup = (IcqGroup) @group;

            trans = new AddContactTransaction(this, icontact, igroup);

            CommitSSITransaction(trans);
        }

        public void RemoveContact(IContact contact, IGroup @group)
        {
            RemoveContactTransaction trans;
            IcqContact icontact;

            icontact = (IcqContact) contact;

            trans = new RemoveContactTransaction(this, icontact);

            CommitSSITransaction(trans);
        }

        public void AttachContact(IContact contact, IGroup @group, bool stored)
        {
            if (!_AllContacts.Contains(contact.Identifier))
            {
                _AllContacts.Add(contact);

                contact.PropertyChanged += OnContactPropertyChanged;
            }

            if (stored)
            {
                if (!_StoredContacts.Contains(contact.Identifier))
                    _StoredContacts.Add(contact);

                if (!@group.Contacts.Contains(contact))
                    @group.Contacts.Add(contact);
            }
        }

        public void AddGroup(IGroup @group)
        {
            throw new NotImplementedException();
        }

        public void RemoveGroup(IGroup @group)
        {
            throw new NotImplementedException();
        }

        public void UpdateContact(IContact contact)
        {
            throw new NotImplementedException();
        }

        public void UpdateGroup(IGroup @group)
        {
            throw new NotImplementedException();
        }

        public bool IsContactStored(IContact contact)
        {
            return _StoredContacts.Contains(contact);
        }

        public INotifyingCollection<IContact> Contacts
        {
            get
            {
                //TODO: Return ReadOnly list
                return _StoredContacts;
            }
        }

        public INotifyingCollection<IGroup> Groups
        {
            get
            {
                //TODO: Return ReadOnly list
                return _Groups;
            }
        }

        public IGroup MasterGroup { get; private set; }

        public void RegisterLocalContactList(int itemCount, DateTime dateChanged)
        {
            Info = new ContactListInfo(itemCount, dateChanged);
            _IsFreshContactList = false;
        }

        public IContactListInfo Info { get; private set; }

        public bool IsContactListAvailable
        {
            get { return Info != null; }
        }

        public bool IsFreshContactList
        {
            get { return _IsFreshContactList; }
        }

        public event EventHandler ContactListActivated;
        public event EventHandler<StatusChangedEventArgs> ContactStatusChanged;

        private IGroup GetGroupByGroupId(int groupId)
        {
            return _Groups.FirstOrDefault(@group => Convert.ToInt32(@group.Attributes["GroupId"]) == groupId);
        }

        public int GetNextSSIItemId()
        {
            return Interlocked.Increment(ref _MaxSSIItemId);
        }

        internal void InnerAddContactToStorage(IContact contact, IGroup @group)
        {
            if (!_StoredContacts.Contains(contact.Identifier))
                _StoredContacts.Add(contact);

            ((IcqContact) contact).SetGroup(@group);

            if (!contact.Group.Contacts.Contains(contact))
                contact.Group.Contacts.Add(contact);
        }

        internal void InnerAddContactToVisibleList(IContact contact)
        {
            if (!_visibleList.Contains(contact))
                _visibleList.Add(contact);
        }

        internal void InnerAddContactToInvisibleList(IContact contact)
        {
            if (!_invisibleList.Contains(contact))
                _invisibleList.Add(contact);
        }

        internal void InnerAddContactToIgnoreList(IContact contact)
        {
            if (!_ignoreList.Contains(contact))
                _ignoreList.Add(contact);
        }

        internal void InnerRemoveContactFromStorage(IContact contact)
        {
            if (!_StoredContacts.Contains(contact.Identifier))
                _StoredContacts.Remove(contact.Identifier);

            if (contact.Group.Contacts.Contains(contact))
                contact.Group.Contacts.Remove(contact);
        }

        internal void InnerRemoveContactFromVisibleList(IContact contact)
        {
            if (_visibleList.Contains(contact))
                _visibleList.Remove(contact);
        }

        internal void InnerRemoveContactFromInvisibleList(IContact contact)
        {
            if (_invisibleList.Contains(contact))
                _invisibleList.Remove(contact);
        }

        internal void InnerRemoveContactFromIgnoreList(IContact contact)
        {
            if (_ignoreList.Contains(contact))
                _ignoreList.Remove(contact);
        }

        public void CommitSSITransaction(ISSITransaction trans)
        {
            Snac1311 beginTransaction;
            Snac1312 endTransaction;
            Snac item;

            IIcqDataTranferService transfer;

            // Check wheter all prerequirements are met to commit the transaction.
            trans.Validate();

            transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

            // Create the transaction data
            beginTransaction = new Snac1311();
            item = trans.CreateSnac();
            endTransaction = new Snac1312();

            // Send data.
            transfer.Send(beginTransaction, item, endTransaction);

            // Wait for server response.
            if (!waitSSIAkk.WaitOne(TimeSpan.FromSeconds(5), false))
            {
                throw new TimeoutException("Server did not respond.");
            }
            trans.OnComplete(codeSSIAkk);
        }

        public void DetachContact(IContact contact, IGroup @group)
        {
            if (@group.Contacts.Contains(contact))
                @group.Contacts.Add(contact);

            if (_StoredContacts.Contains(contact.Identifier))
                _StoredContacts.Add(contact);
        }

        internal void AnalyseSnac130E(Snac130E dataIn)
        {
            codeSSIAkk = dataIn.ActionResultCodes.First();
            waitSSIAkk.Set();

            foreach (var code in dataIn.ActionResultCodes)
            {
                Debug.WriteLine(string.Format("SSI Change Akk: {0}", code), "IcqStorageService");
            }
        }

        internal void AnalyseSnac1306(Snac1306 dataIn)
        {
            try
            {
                _MaxSSIItemId = Math.Max(_MaxSSIItemId, dataIn.MaxItemId);

                foreach (var ssiGroup in dataIn.GroupRecords)
                {
                    var identifier = ssiGroup.ItemName;

                    var @group = new IcqGroup(identifier, ssiGroup.GroupId);

                    if (@group.GroupId == 0)
                    {
                        MasterGroup = @group;
                    }
                    else
                    {
                        _Groups.Add(@group);
                    }
                }

                foreach (var x in _Groups)
                {
                    MasterGroup.Groups.Add(x);
                }

                foreach (var ssiContact in dataIn.BuddyRecords)
                {
                    var identifier = ssiContact.ItemName;
                    var @group = GetGroupByGroupId(ssiContact.GroupId);

                    int identifierId;

                    if (!int.TryParse(identifier, out identifierId))
                        continue;

                    var contact = (IcqContact) GetContactByIdentifier(identifier);

                    if (contact.LastShortUserInfoRequest <= DateTime.MinValue)
                        contact.Name = ssiContact.LocalScreenName.LocalScreenName;
                    contact.ItemId = ssiContact.ItemId;
                    contact.SetGroup(@group);

                    AttachContact(contact, @group, true);
                }

                foreach (var record in dataIn.DenyRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).DenyRecordItemId = record.ItemId;

                    _invisibleList.Add(contact);
                }

                foreach (var record in dataIn.PermitRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).PermitRecordItemId = record.ItemId;

                    _visibleList.Add(contact);
                }

                foreach (var record in dataIn.IgnoreListRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).IgnoreRecordItemId = record.ItemId;

                    _ignoreList.Add(contact);
                }

                Info = new ContactListInfo(dataIn.ItemCount, dataIn.LastChange);

                if (ContactListActivated != null)
                {
                    ContactListActivated(this, EventArgs.Empty);
                }
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

                if (ContactListActivated != null)
                {
                    ContactListActivated(this, EventArgs.Empty);
                }
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

                    var @group = new IcqGroup(identifier, ssiGroup.GroupId);

                    _Groups.Add(@group);
                }

                foreach (var ssiContact in dataIn.BuddyRecords)
                {
                    var identifier = ssiContact.ItemName;
                    var @group = GetGroupByGroupId(ssiContact.GroupId);

                    int identifierId;

                    if (!int.TryParse(identifier, out identifierId))
                        continue;

                    var contact = (IcqContact) GetContactByIdentifier(identifier);

                    if (contact.LastShortUserInfoRequest <= DateTime.MinValue)
                        contact.Name = ssiContact.LocalScreenName.LocalScreenName;
                    contact.ItemId = ssiContact.ItemId;
                    contact.SetGroup(@group);

                    AttachContact(contact, @group, true);
                }

                foreach (var record in dataIn.DenyRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).DenyRecordItemId = record.ItemId;

                    _invisibleList.Add(contact);
                }

                foreach (var record in dataIn.PermitRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).PermitRecordItemId = record.ItemId;

                    _visibleList.Add(contact);
                }

                foreach (var record in dataIn.IgnoreListRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).IgnoreRecordItemId = record.ItemId;

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
                foreach (var record in dataIn.DenyRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).DenyRecordItemId = 0;

                    _invisibleList.Remove(contact);
                }

                foreach (var record in dataIn.PermitRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).PermitRecordItemId = 0;

                    _visibleList.Remove(contact);
                }

                foreach (var record in dataIn.IgnoreListRecords)
                {
                    var identifier = record.ItemName;
                    var contact = GetContactByIdentifier(identifier);

                    ((IcqContact) contact).IgnoreRecordItemId = 0;

                    _ignoreList.Remove(contact);
                }

                foreach (var ssiContact in dataIn.BuddyRecords)
                {
                    var identifier = ssiContact.ItemName;
                    var @group = GetGroupByGroupId(ssiContact.GroupId);

                    int identifierId;

                    if (!int.TryParse(identifier, out identifierId))
                        continue;

                    var contact = (IcqContact) GetContactByIdentifier(identifier);

                    DetachContact(contact, @group);
                }

                foreach (var ssiGroup in dataIn.GroupRecords)
                {
                    var identifier = ssiGroup.ItemName;

                    var @group = new IcqGroup(identifier, ssiGroup.GroupId);

                    _Groups.Remove(@group);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnContactPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                var contact = sender as IContact;

                if (contact != null)
                {
                    var args = new StatusChangedEventArgs(contact.Status, contact);

                    if (ContactStatusChanged != null)
                    {
                        ContactStatusChanged(this, args);
                    }
                }
            }
        }

        #region  Privacy Lists

        private readonly NotifyingCollection<IContact> _visibleList;

        private readonly ReadOnlyNotifyingCollection<IContact> _readOnlyVisibleList;

        public ReadOnlyNotifyingCollection<IContact> VisibleList
        {
            get { return _readOnlyVisibleList; }
        }

        private readonly NotifyingCollection<IContact> _invisibleList;

        private readonly ReadOnlyNotifyingCollection<IContact> _readOnlyInvisibleList;

        public ReadOnlyNotifyingCollection<IContact> InvisibleList
        {
            get { return _readOnlyInvisibleList; }
        }

        private readonly NotifyingCollection<IContact> _ignoreList;

        private readonly ReadOnlyNotifyingCollection<IContact> _readOnlyIgnoreList;

        public ReadOnlyNotifyingCollection<IContact> IgnoreList
        {
            get { return _readOnlyIgnoreList; }
        }

        #endregion
    }
}