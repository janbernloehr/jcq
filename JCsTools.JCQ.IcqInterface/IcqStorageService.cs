//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqStorageService : ContextService, Interfaces.IStorageService
  {
    private readonly Core.KeyedNotifiyingCollection<string, Interfaces.IContact> _StoredContacts = new Core.KeyedNotifiyingCollection<string, Interfaces.IContact>(c => c.Identifier);
    private readonly Core.KeyedNotifiyingCollection<string, Interfaces.IContact> _AllContacts = new Core.KeyedNotifiyingCollection<string, Interfaces.IContact>(c => c.Identifier);
    private readonly Core.KeyedNotifiyingCollection<string, Interfaces.IGroup> _Groups = new Core.KeyedNotifiyingCollection<string, Interfaces.IGroup>(g => g.Identifier);
    private Interfaces.IGroup _MasterGroup;

    private int _MaxSSIItemId;

    private readonly System.Threading.ManualResetEvent waitSSIAkk = new System.Threading.ManualResetEvent(false);
    private DataTypes.SSIActionResultCode codeSSIAkk;

    public IcqStorageService(Interfaces.IContext context) : base(context)
    {

      IcqConnector connector = context.GetService<Interfaces.IConnector>() as IcqConnector;

      if (connector == null)
        throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

      _AllContacts.Add(context.Identity);

      context.Identity.PropertyChanged += OnContactPropertyChanged;

      connector.RegisterSnacHandler(0x13, 0x6, new Action<DataTypes.Snac1306>(AnalyseSnac1306));
      connector.RegisterSnacHandler(0x13, 0x8, new Action<DataTypes.Snac1308>(AnalyseSnac1308));
      connector.RegisterSnacHandler(0x13, 0xa, new Action<DataTypes.Snac130A>(AnalyseSnac130A));
      connector.RegisterSnacHandler(0x13, 0xe, new Action<DataTypes.Snac130E>(AnalyseSnac130E));
      connector.RegisterSnacHandler(0x13, 0xf, new Action<DataTypes.Snac130F>(AnalyseSnac130F));
    }

    public Interfaces.IContact Interfaces.IStorageService.GetContactByIdentifier(string identifier)
    {
      Interfaces.IContact contact;

      if (!_AllContacts.Contains(identifier)) {
        contact = new IcqContact(identifier, identifier);

        _AllContacts.Add(contact);

        contact.PropertyChanged += OnContactPropertyChanged;
      } else {
        contact = _AllContacts(identifier);
      }

      return contact;
    }

    public Interfaces.IGroup Interfaces.IStorageService.GetGroupByIdentifier(string identifier)
    {
      if (_Groups.Contains(identifier))
        return _Groups(identifier);
      else
        return null;
    }

    private Interfaces.IGroup GetGroupByGroupId(int groupId)
    {
      foreach (Interfaces.IGroup @group in _Groups) {
        if (Convert.ToInt32(@group.Attributes("GroupId")) == groupId)
          return @group;
      }
    }

    public int GetNextSSIItemId()
    {
      return Threading.Interlocked.Increment(_MaxSSIItemId);
    }

#region  Privacy Lists 
    private Core.NotifyingCollection<Interfaces.IContact> _VisibleList = new Core.NotifyingCollection<Interfaces.IContact>();
    private Core.ReadOnlyNotifyingCollection<Interfaces.IContact> _ReadOnlyVisibleList = new Core.ReadOnlyNotifyingCollection<Interfaces.IContact>(_VisibleList);

    public Core.ReadOnlyNotifyingCollection<Interfaces.IContact> VisibleList {
      get { return _ReadOnlyVisibleList; }
    }

    private Core.NotifyingCollection<Interfaces.IContact> _InvisibleList = new Core.NotifyingCollection<Interfaces.IContact>();
    private Core.ReadOnlyNotifyingCollection<Interfaces.IContact> _ReadOnlyInvisibleList = new Core.ReadOnlyNotifyingCollection<Interfaces.IContact>(_InvisibleList);

    public Core.ReadOnlyNotifyingCollection<Interfaces.IContact> InvisibleList {
      get { return _ReadOnlyInvisibleList; }
    }

    private Core.NotifyingCollection<Interfaces.IContact> _IgnoreList = new Core.NotifyingCollection<Interfaces.IContact>();
    private Core.ReadOnlyNotifyingCollection<Interfaces.IContact> _ReadOnlyIgnoreList = new Core.ReadOnlyNotifyingCollection<Interfaces.IContact>(_IgnoreList);

    public Core.ReadOnlyNotifyingCollection<Interfaces.IContact> IgnoreList {
      get { return _ReadOnlyIgnoreList; }
    }
#endregion

    internal void InnerAddContactToStorage(Interfaces.IContact contact, Interfaces.IGroup @group)
    {
      if (!_StoredContacts.Contains(contact.Identifier))
        _StoredContacts.Add(contact);

      ((IcqContact)contact).SetGroup(@group);

      if (!contact.Group.Contacts.Contains(contact))
        contact.Group.Contacts.Add(contact);
    }

    internal void InnerAddContactToVisibleList(Interfaces.IContact contact)
    {
      if (!_VisibleList.Contains(contact))
        _VisibleList.Add(contact);
    }

    internal void InnerAddContactToInvisibleList(Interfaces.IContact contact)
    {
      if (!_InvisibleList.Contains(contact))
        _InvisibleList.Add(contact);
    }

    internal void InnerAddContactToIgnoreList(Interfaces.IContact contact)
    {
      if (!_IgnoreList.Contains(contact))
        _IgnoreList.Add(contact);
    }

    internal void InnerRemoveContactFromStorage(Interfaces.IContact contact)
    {
      if (!_StoredContacts.Contains(contact.Identifier))
        _StoredContacts.Remove(contact.Identifier);

      if (contact.Group.Contacts.Contains(contact))
        contact.Group.Contacts.Remove(contact);
    }

    internal void InnerRemoveContactFromVisibleList(Interfaces.IContact contact)
    {
      if (_VisibleList.Contains(contact))
        _VisibleList.Remove(contact);
    }

    internal void InnerRemoveContactFromInvisibleList(Interfaces.IContact contact)
    {
      if (_InvisibleList.Contains(contact))
        _InvisibleList.Remove(contact);
    }

    internal void InnerRemoveContactFromIgnoreList(Interfaces.IContact contact)
    {
      if (_IgnoreList.Contains(contact))
        _IgnoreList.Remove(contact);
    }

    public void Interfaces.IStorageService.AddContact(Interfaces.IContact contact, Interfaces.IGroup @group)
    {
      AddContactTransaction trans;
      IcqContact icontact;
      IcqGroup igroup;

      icontact = (IcqContact)contact;
      igroup = (IcqGroup)@group;

      trans = new AddContactTransaction(this, icontact, igroup);

      CommitSSITransaction(trans);
    }

    public void Interfaces.IStorageService.RemoveContact(Interfaces.IContact contact, Interfaces.IGroup @group)
    {
      RemoveContactTransaction trans;
      IcqContact icontact;

      icontact = (IcqContact)contact;

      trans = new RemoveContactTransaction(this, icontact);

      CommitSSITransaction(trans);
    }

    public void CommitSSITransaction(ISSITransaction trans)
    {
      DataTypes.Snac1311 beginTransaction;
      DataTypes.Snac1312 endTransaction;
      DataTypes.Snac item;

      IIcqDataTranferService transfer;

      // Check wheter all prerequirements are met to commit the transaction.
      trans.Validate();

      transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();

      // Create the transaction data
      beginTransaction = new DataTypes.Snac1311();
      item = trans.CreateSnac();
      endTransaction = new DataTypes.Snac1312();

      // Send data.
      transfer.Send(beginTransaction, item, endTransaction);

      // Wait for server response.
      if (!waitSSIAkk.WaitOne(TimeSpan.FromSeconds(5), false)) {
        throw new TimeoutException("Server did not respond.");
      } else {
        trans.OnComplete(codeSSIAkk);
      }
    }

    public void Interfaces.IStorageService.AttachContact(Interfaces.IContact contact, Interfaces.IGroup @group, bool stored)
    {
      if (!_AllContacts.Contains(contact.Identifier)) {
        _AllContacts.Add(contact);

        contact.PropertyChanged += OnContactPropertyChanged;
      }

      if (stored) {
        if (!_StoredContacts.Contains(contact.Identifier))
          _StoredContacts.Add(contact);

        if (!@group.Contacts.Contains(contact))
          @group.Contacts.Add(contact);
      }
    }

    public void DetachContact(Interfaces.IContact contact, Interfaces.IGroup @group)
    {
      if (@group.Contacts.Contains(contact))
        @group.Contacts.Add(contact);

      if (_StoredContacts.Contains(contact.Identifier))
        _StoredContacts.Add(contact);
    }

    internal void AnalyseSnac130E(DataTypes.Snac130E dataIn)
    {
      codeSSIAkk = dataIn.ActionResultCodes.First;
      waitSSIAkk.Set();

      foreach (DataTypes.SSIActionResultCode code in dataIn.ActionResultCodes) {
        Debug.WriteLine(string.Format("SSI Change Akk: {0}", code), "IcqStorageService");
      }
    }

    internal void AnalyseSnac1306(DataTypes.Snac1306 dataIn)
    {
      try {
        _MaxSSIItemId = Math.Max(_MaxSSIItemId, dataIn.MaxItemId);

        foreach (DataTypes.SSIGroupRecord ssiGroup in dataIn.GroupRecords) {
          string identifier = ssiGroup.ItemName;

          IcqGroup @group = new IcqGroup(identifier, ssiGroup.GroupId);

          if (@group.GroupId == 0) {
            _MasterGroup = @group;
          } else {
            _Groups.Add(@group);
          }
        }

        foreach (Interfaces.IGroup x in _Groups) {
          _MasterGroup.Groups.Add(x);
        }

        foreach (DataTypes.SSIBuddyRecord ssiContact in dataIn.BuddyRecords) {
          string identifier = ssiContact.ItemName;
          Interfaces.IGroup @group = GetGroupByGroupId(ssiContact.GroupId);

          if (!int.TryParse(identifier, null))
            continue;

          IcqContact contact = (IcqContact)GetContactByIdentifier(identifier);

          if (!contact.LastShortUserInfoRequest > DateTime.MinValue)
            contact.Name = ssiContact.LocalScreenName.LocalScreenName;
          contact.ItemId = ssiContact.ItemId;
          contact.SetGroup(@group);

          AttachContact(contact, @group, true);
        }

        foreach (DataTypes.SSIDenyRecord record in dataIn.DenyRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).DenyRecordItemId = record.ItemId;

          _InvisibleList.Add(contact);
        }

        foreach (DataTypes.SSIPermitRecord record in dataIn.PermitRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).PermitRecordItemId = record.ItemId;

          _VisibleList.Add(contact);
        }

        foreach (DataTypes.SSIIgnoreListRecord record in dataIn.IgnoreListRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).IgnoreRecordItemId = record.ItemId;

          _IgnoreList.Add(contact);
        }

        _Info = new ContactListInfo(dataIn.ItemCount, dataIn.LastChange);

        if (ContactListActivated != null) {
          ContactListActivated(this, System.EventArgs.Empty);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    internal void AnalyseSnac130F(DataTypes.Snac130F dataIn)
    {
      try {
        _Info = new ContactListInfo(dataIn.NumberOfItems, dataIn.ModificationDate);

        if (ContactListActivated != null) {
          ContactListActivated(this, System.EventArgs.Empty);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    internal void AnalyseSnac1308(DataTypes.Snac1308 dataIn)
    {
      // Sever informs the client that items have beend added to the SSI Store.
      try {
        foreach (DataTypes.SSIGroupRecord ssiGroup in dataIn.GroupRecords) {
          string identifier = ssiGroup.ItemName;

          IcqGroup @group = new IcqGroup(identifier, ssiGroup.GroupId);

          _Groups.Add(@group);
        }

        foreach (DataTypes.SSIBuddyRecord ssiContact in dataIn.BuddyRecords) {
          string identifier = ssiContact.ItemName;
          Interfaces.IGroup @group = GetGroupByGroupId(ssiContact.GroupId);

          if (!int.TryParse(identifier, null))
            continue;

          IcqContact contact = (IcqContact)GetContactByIdentifier(identifier);

          if (!contact.LastShortUserInfoRequest > DateTime.MinValue)
            contact.Name = ssiContact.LocalScreenName.LocalScreenName;
          contact.ItemId = ssiContact.ItemId;
          contact.SetGroup(@group);

          AttachContact(contact, @group, true);
        }

        foreach (DataTypes.SSIDenyRecord record in dataIn.DenyRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).DenyRecordItemId = record.ItemId;

          _InvisibleList.Add(contact);
        }

        foreach (DataTypes.SSIPermitRecord record in dataIn.PermitRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).PermitRecordItemId = record.ItemId;

          _VisibleList.Add(contact);
        }

        foreach (DataTypes.SSIIgnoreListRecord record in dataIn.IgnoreListRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).IgnoreRecordItemId = record.ItemId;

          _IgnoreList.Add(contact);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    internal void AnalyseSnac130A(DataTypes.Snac130A dataIn)
    {
      // Sever informs the client that items have beend removed from the SSI Store.

      try {
        foreach (DataTypes.SSIDenyRecord record in dataIn.DenyRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).DenyRecordItemId = 0;

          _InvisibleList.Remove(contact);
        }

        foreach (DataTypes.SSIPermitRecord record in dataIn.PermitRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).PermitRecordItemId = 0;

          _VisibleList.Remove(contact);
        }

        foreach (DataTypes.SSIIgnoreListRecord record in dataIn.IgnoreListRecords) {
          string identifier = record.ItemName;
          Interfaces.IContact contact = GetContactByIdentifier(identifier);

          ((IcqContact)contact).IgnoreRecordItemId = 0;

          _IgnoreList.Remove(contact);
        }

        foreach (DataTypes.SSIBuddyRecord ssiContact in dataIn.BuddyRecords) {
          string identifier = ssiContact.ItemName;
          Interfaces.IGroup @group = GetGroupByGroupId(ssiContact.GroupId);

          if (!int.TryParse(identifier, null))
            continue;

          IcqContact contact = (IcqContact)GetContactByIdentifier(identifier);

          DetachContact(contact, @group);
        }

        foreach (DataTypes.SSIGroupRecord ssiGroup in dataIn.GroupRecords) {
          string identifier = ssiGroup.ItemName;

          IcqGroup @group = new IcqGroup(identifier, ssiGroup.GroupId);

          _Groups.Remove(@group);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public void Interfaces.IStorageService.AddGroup(Interfaces.IGroup @group)
    {
      throw new NotImplementedException();
    }

    public void Interfaces.IStorageService.RemoveGroup(Interfaces.IGroup @group)
    {
      throw new NotImplementedException();
    }

    public void Interfaces.IStorageService.UpdateContact(Interfaces.IContact contact)
    {
      throw new NotImplementedException();
    }

    public void Interfaces.IStorageService.UpdateGroup(Interfaces.IGroup @group)
    {
      throw new NotImplementedException();
    }

    public event ContactListActivatedEventHandler ContactListActivated;
    public delegate void ContactListActivatedEventHandler(object sender, System.EventArgs e);

    public bool Interfaces.IStorageService.IsContactStored(Interfaces.IContact contact)
    {
      return _StoredContacts.Contains(contact);
    }

    public Core.Interfaces.INotifyingCollection<Interfaces.IContact> Interfaces.IStorageService.Contacts {
      get {
        //TODO: Return ReadOnly list
        return _StoredContacts;
      }
    }

    public Core.Interfaces.INotifyingCollection<Interfaces.IGroup> Interfaces.IStorageService.Groups {
      get {
        //TODO: Return ReadOnly list
        return _Groups;
      }
    }

    public Interfaces.IGroup Interfaces.IStorageService.MasterGroup {
      get { return _MasterGroup; }
    }

    private Interfaces.IContactListInfo _Info;

    public void Interfaces.IStorageService.RegisterLocalContactList(int itemCount, System.DateTime dateChanged)
    {
      _Info = new ContactListInfo(itemCount, dateChanged);
      _IsFreshContactList = false;
    }

    public Interfaces.IContactListInfo Interfaces.IStorageService.Info {
      get { return _Info; }
    }

    public bool Interfaces.IStorageService.IsContactListAvailable {
      get { return _Info != null; }
    }

    private bool _IsFreshContactList = true;

    public bool Interfaces.IStorageService.IsFreshContactList {
      get { return _IsFreshContactList; }
    }

    public event ContactStatusChangedEventHandler ContactStatusChanged;
    public delegate void ContactStatusChangedEventHandler(object sender, Interfaces.StatusChangedEventArgs e);

    protected void OnContactPropertyChanged(object sender, ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Status") {
        Interfaces.IContact contact = sender as Interfaces.IContact;

        if (contact != null) {
          object args = new Interfaces.StatusChangedEventArgs(contact.Status, contact);

          if (ContactStatusChanged != null) {
            ContactStatusChanged(this, args);
          }
        }
      }
    }
  }

  public class ContactListInfo : Interfaces.IContactListInfo
  {
    private System.DateTime _DateChanged;
    private int _ItemCount;

    public ContactListInfo(int itemCount, System.DateTime dateChanged)
    {
      _ItemCount = itemCount;
      _DateChanged = dateChanged;
    }

    public System.DateTime Interfaces.IContactListInfo.DateChanged {
      get { return _DateChanged; }
    }

    public int Interfaces.IContactListInfo.ItemCount {
      get { return _ItemCount; }
    }
  }





}

