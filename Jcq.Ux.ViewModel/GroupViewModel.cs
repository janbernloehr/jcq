// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class GroupViewModel : DispatcherObject, INotifyPropertyChanged
    {
        private ObservableCollection<ContactViewModel> _Contacts;
        private ContactViewModelCollectionBinding _ContactsBinding;
        private CollectionView _ContactsView;
        private ObservableCollection<GroupViewModel> _Groups;
        private GroupViewModelCollectionBinding _GroupsBinding;
        private CollectionView _GroupsView;

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
                if (_Contacts == null)
                {
                    lock (Model.Contacts)
                    {
                        _Contacts =
                            new ObservableCollection<ContactViewModel>(
                                (from c in Model.Contacts select ContactViewModelCache.GetViewModel(c)).ToList());
                        _ContactsBinding = new ContactViewModelCollectionBinding(Model.Contacts, _Contacts);
                    }
                }

                return _Contacts;
            }
        }

        public ObservableCollection<GroupViewModel> Groups
        {
            get
            {
                if (_Groups == null)
                {
                    lock (Model.Groups)
                    {
                        _Groups =
                            new ObservableCollection<GroupViewModel>(
                                (from g in Model.Groups select GroupViewModelCache.GetViewModel(g)).ToList());
                        _GroupsBinding = new GroupViewModelCollectionBinding(Model.Groups, _Groups);
                    }
                }

                return _Groups;
            }
        }

        public CollectionView GroupsView
        {
            get
            {
                if (_GroupsView == null)
                {
                    _GroupsView = (CollectionView) CollectionViewSource.GetDefaultView(Groups);
                    _GroupsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }

                return _GroupsView;
            }
        }

        public CollectionView ContactsView
        {
            get
            {
                if (_ContactsView == null)
                {
                    _ContactsView = (CollectionView) CollectionViewSource.GetDefaultView(Contacts);

                    _ContactsView.SortDescriptions.Add(new SortDescription("StatusFlag",
                        ListSortDirection.Ascending));
                    _ContactsView.SortDescriptions.Add(new SortDescription("Name",
                        ListSortDirection.Ascending));
                }

                return _ContactsView;
            }
        }

        //public Hashtable Attributes
        //{
        //    get { return Model.Attributes; }
        //}

        public string Identifier
        {
            get { return Model.Identifier; }
        }

        public string Name
        {
            get { return Model.Name; }
        }

        //Protected Sub XO(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        //    Debug.WriteLine(String.Format("Changed: {0}", e.PropertyName), "XO")
        //End Sub

        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnContactPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var c = (ContactViewModel) sender;

            if (e.PropertyName == "StatusFlag" | e.PropertyName == "Name")
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ContactsView.Refresh));
            }
        }

        protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<PropertyChangedEventArgs>(OnPropertyChanged), e);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}