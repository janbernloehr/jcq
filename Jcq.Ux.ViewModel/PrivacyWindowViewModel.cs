// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivacyWindowViewModel.cs" company="Jan-Cornelius Molnar">
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