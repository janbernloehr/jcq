// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivacyWindowViewModel.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.ObjectModel;
using System.Linq;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class PrivacyWindowViewModel
    {
        private readonly ObservableCollection<ContactViewModel> _IgnoredContacts;
        private readonly ObservableCollection<ContactViewModel> _InvisibleContacts;
        private readonly ObservableCollection<ContactViewModel> _VisibleContacts;
        private ContactNotifiyingCollectionBinding _IgnoredContactsBinding;
        private ContactNotifiyingCollectionBinding _InvisibileContactsBinding;
        private ContactNotifiyingCollectionBinding _VisibileContactsBinding;

        public PrivacyWindowViewModel()
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();

            _VisibleContacts =
                new ObservableCollection<ContactViewModel>(
                    (from x in svPrivacy.VisibleList select ContactViewModelCache.GetViewModel(x)).ToList());
            _InvisibleContacts =
                new ObservableCollection<ContactViewModel>(
                    (from x in svPrivacy.InvisibleList select ContactViewModelCache.GetViewModel(x)).ToList());
            _IgnoredContacts =
                new ObservableCollection<ContactViewModel>(
                    (from x in svPrivacy.IgnoreList select ContactViewModelCache.GetViewModel(x)).ToList());

            _VisibileContactsBinding = new ContactNotifiyingCollectionBinding(svPrivacy.VisibleList, _VisibleContacts);
            _InvisibileContactsBinding = new ContactNotifiyingCollectionBinding(svPrivacy.InvisibleList,
                _InvisibleContacts);
            _IgnoredContactsBinding = new ContactNotifiyingCollectionBinding(svPrivacy.IgnoreList, _IgnoredContacts);

            VisibleContacts = new ReadOnlyObservableCollection<ContactViewModel>(_VisibleContacts);
            InvisibleContacts = new ReadOnlyObservableCollection<ContactViewModel>(_InvisibleContacts);
            IgnoredContacts = new ReadOnlyObservableCollection<ContactViewModel>(_IgnoredContacts);
        }

        public ReadOnlyObservableCollection<ContactViewModel> VisibleContacts { get; private set; }
        public ReadOnlyObservableCollection<ContactViewModel> InvisibleContacts { get; private set; }
        public ReadOnlyObservableCollection<ContactViewModel> IgnoredContacts { get; private set; }

        public void AddInvisibleContact(string identifier)
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();
            var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();
            var contact = svStorage.GetContactByIdentifier(identifier);

            svPrivacy.AddContactToInvisibleList(contact);
        }

        public void RemoveInvisibleContact(ContactViewModel contact)
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();

            svPrivacy.RemoveContactFromInvisibleList(contact.Model);
        }

        public void AddVisibleContact(string identifier)
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();
            var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();
            var contact = svStorage.GetContactByIdentifier(identifier);

            svPrivacy.AddContactToVisibleList(contact);
        }

        public void RemoveVisibleContact(ContactViewModel contact)
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();

            svPrivacy.RemoveContactFromVisibleList(contact.Model);
        }

        public void AddIgnoreContact(string identifier)
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();
            var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();
            var contact = svStorage.GetContactByIdentifier(identifier);

            svPrivacy.AddContactToIgnoreList(contact);
        }

        public void RemoveIgnoreContact(ContactViewModel contact)
        {
            var svPrivacy = ApplicationService.Current.Context.GetService<IPrivacyService>();

            svPrivacy.RemoveContactFromIgnoreList(contact.Model);
        }
    }
}