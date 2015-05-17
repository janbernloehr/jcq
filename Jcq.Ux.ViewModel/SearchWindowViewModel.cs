// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchWindowViewModel.cs" company="Jan-Cornelius Molnar">
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