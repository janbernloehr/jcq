// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchWindowViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Windows.Controls;
using System.Windows.Threading;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class SearchWindowViewModel : DispatcherObject, INotifyPropertyChanged
    {
        public SearchWindowViewModel()
        {
            var sv = ApplicationService.Current.Context.GetService<ISearchService>();

            sv.SearchResult += OnSearchResult;
        }

        public Collection<ContactViewModel> SearchResult { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnSearchResult(object sender, SearchResultEventArgs e)
        {
            try
            {
                SearchResult =
                    new Collection<ContactViewModel>(
                        (from x in e.Contacts select ContactViewModelCache.GetViewModel(x)).ToList());

                OnPropertyChanged("SearchResult");
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        public void Search(string uinQuery)
        {
            var sv = ApplicationService.Current.Context.GetService<ISearchService>();

            sv.SearchByUin(uinQuery);
        }

        public void BuildContactContextMenu(ContextMenu menu, ContactViewModel contact)
        {
            var sv = ApplicationService.Current.Context.GetService<IContactContextMenuService>();
            menu.Items.Clear();

            foreach (var x in sv.GetMenuItems(contact))
            {
                menu.Items.Add(x);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}