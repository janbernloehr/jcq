// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.Ux.ViewModel
{
    public class GroupViewModel : ViewModelBase
    {
        private ObservableCollection<ContactViewModel> _contacts;
        private ContactViewModelCollectionBinding _contactsBinding;
        private CollectionView _contactsView;
        private ObservableCollection<GroupViewModel> _groups;
        private GroupViewModelCollectionBinding _groupsBinding;
        private CollectionView _groupsView;

        public GroupViewModel(IGroup model)
        {
            Model = model;

            Model.PropertyChanged += HandlePropertyChanged;
        }

        public IGroup Model { get; private set; }

        public ObservableCollection<ContactViewModel> Contacts
        {
            get
            {
                if (_contacts == null)
                {
                    lock (Model.Contacts)
                    {
                        _contacts =
                            new ObservableCollection<ContactViewModel>(
                                (from c in Model.Contacts select ContactViewModelCache.GetViewModel(c)).ToList());
                        _contactsBinding = new ContactViewModelCollectionBinding(Model.Contacts, _contacts);
                    }
                }

                return _contacts;
            }
        }

        public ObservableCollection<GroupViewModel> Groups
        {
            get
            {
                if (_groups == null)
                {
                    lock (Model.Groups)
                    {
                        _groups =
                            new ObservableCollection<GroupViewModel>(
                                (from g in Model.Groups select GroupViewModelCache.GetViewModel(g)).ToList());
                        _groupsBinding = new GroupViewModelCollectionBinding(Model.Groups, _groups);
                    }
                }

                return _groups;
            }
        }

        public CollectionView GroupsView
        {
            get
            {
                if (_groupsView == null)
                {
                    _groupsView = (CollectionView)CollectionViewSource.GetDefaultView(Groups);
                    _groupsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }

                return _groupsView;
            }
        }

        public CollectionView ContactsView
        {
            get
            {
                if (_contactsView == null)
                {
                    _contactsView = (CollectionView)CollectionViewSource.GetDefaultView(Contacts);

                    _contactsView.SortDescriptions.Add(new SortDescription("StatusFlag",
                        ListSortDirection.Ascending));
                    _contactsView.SortDescriptions.Add(new SortDescription("Name",
                        ListSortDirection.Ascending));
                }

                return _contactsView;
            }
        }

        public string Identifier
        {
            get { return Model.Identifier; }
        }

        public string Name
        {
            get { return Model.Name; }
        }

        internal void OnContactPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var c = (ContactViewModel)sender;

            if (e.PropertyName == "StatusFlag" | e.PropertyName == "Name")
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ContactsView.Refresh));
            }
        }

        protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action<PropertyChangedEventArgs>(OnPropertyChanged), e);
        }
    }
}