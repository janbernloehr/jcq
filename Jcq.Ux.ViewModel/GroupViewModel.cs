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