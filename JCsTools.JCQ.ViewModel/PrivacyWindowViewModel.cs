using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.ViewModel
{
  public class PrivacyWindowViewModel
  {
    private Collections.ObjectModel.ObservableCollection<ContactViewModel> _VisibleContacts;
    private Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel> _ReadOnlyVisibleContacts;

    private Collections.ObjectModel.ObservableCollection<ContactViewModel> _InvisibleContacts;
    private Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel> _ReadOnlyInvisibleContacts;

    private Collections.ObjectModel.ObservableCollection<ContactViewModel> _IgnoredContacts;
    private Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel> _ReadOnlyIgnoredContacts;

    private ContactNotifiyingCollectionBinding _VisibileContactsBinding;
    private ContactNotifiyingCollectionBinding _InvisibileContactsBinding;
    private ContactNotifiyingCollectionBinding _IgnoredContactsBinding;

    public PrivacyWindowViewModel()
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();

      _VisibleContacts = new Collections.ObjectModel.ObservableCollection<ContactViewModel>((from x in svPrivacy.VisibleListContactViewModelCache.GetViewModel(x)).ToList);
      _InvisibleContacts = new Collections.ObjectModel.ObservableCollection<ContactViewModel>((from x in svPrivacy.InvisibleListContactViewModelCache.GetViewModel(x)).ToList);
      _IgnoredContacts = new Collections.ObjectModel.ObservableCollection<ContactViewModel>((from x in svPrivacy.IgnoreListContactViewModelCache.GetViewModel(x)).ToList);

      _VisibileContactsBinding = new ContactNotifiyingCollectionBinding(svPrivacy.VisibleList, _VisibleContacts);
      _InvisibileContactsBinding = new ContactNotifiyingCollectionBinding(svPrivacy.InvisibleList, _InvisibleContacts);
      _IgnoredContactsBinding = new ContactNotifiyingCollectionBinding(svPrivacy.IgnoreList, _IgnoredContacts);

      _ReadOnlyVisibleContacts = new Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel>(_VisibleContacts);
      _ReadOnlyInvisibleContacts = new Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel>(_InvisibleContacts);
      _ReadOnlyIgnoredContacts = new Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel>(_IgnoredContacts);
    }

    public Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel> VisibleContacts {
      get { return _ReadOnlyVisibleContacts; }
    }

    public Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel> InvisibleContacts {
      get { return _ReadOnlyInvisibleContacts; }
    }

    public Collections.ObjectModel.ReadOnlyObservableCollection<ContactViewModel> IgnoredContacts {
      get { return _ReadOnlyIgnoredContacts; }
    }

    public void AddInvisibleContact(string identifier)
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();
      object svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();
      object contact = svStorage.GetContactByIdentifier(identifier);

      svPrivacy.AddContactToInvisibleList(contact);
    }

    public void RemoveInvisibleContact(ContactViewModel contact)
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();

      svPrivacy.RemoveContactFromInvisibleList(contact.Model);
    }

    public void AddVisibleContact(string identifier)
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();
      object svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();
      object contact = svStorage.GetContactByIdentifier(identifier);

      svPrivacy.AddContactToVisibleList(contact);
    }

    public void RemoveVisibleContact(ContactViewModel contact)
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();

      svPrivacy.RemoveContactFromVisibleList(contact.Model);
    }

    public void AddIgnoreContact(string identifier)
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();
      object svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();
      object contact = svStorage.GetContactByIdentifier(identifier);

      svPrivacy.AddContactToIgnoreList(contact);
    }

    public void RemoveIgnoreContact(ContactViewModel contact)
    {
      object svPrivacy = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IPrivacyService>();

      svPrivacy.RemoveContactFromIgnoreList(contact.Model);
    }

  }

  public class ContactNotifiyingCollectionBinding : Core.NotifyingCollectionBinding<ContactViewModel>
  {
    public ContactNotifiyingCollectionBinding(Collections.Specialized.INotifyCollectionChanged source, Collections.ObjectModel.ObservableCollection<ContactViewModel> target) : base(source, target)
    {
    }

    protected override ContactViewModel GetTargetItem(object item)
    {
      object contact = (IcqInterface.Interfaces.IContact)item;

      return ContactViewModelCache.GetViewModel(contact);
    }
  }
}

