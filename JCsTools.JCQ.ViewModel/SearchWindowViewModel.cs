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
using System.Linq;
using System.Collections.ObjectModel;
namespace JCsTools.JCQ.ViewModel
{
  public class SearchWindowViewModel : System.Windows.Threading.DispatcherObject, System.ComponentModel.INotifyPropertyChanged
  {
    private Collection<ContactViewModel> _SearchResult;

    public SearchWindowViewModel()
    {
      IcqInterface.Interfaces.ISearchService sv;

      sv = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.ISearchService>();

      sv.SearchResult += OnSearchResult;
    }

    private void OnSearchResult(object sender, IcqInterface.Interfaces.SearchResultEventArgs e)
    {
      try {
        _SearchResult = new Collection<ContactViewModel>((from x in e.ContactsContactViewModelCache.GetViewModel(x)).ToList);

        OnPropertyChanged("SearchResult");
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public Collection<ContactViewModel> SearchResult {
      get { return _SearchResult; }
    }

    public void Search(string uinQuery)
    {
      IcqInterface.Interfaces.ISearchService sv;

      sv = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.ISearchService>();

      sv.SearchByUin(uinQuery);
    }

    public void BuildContactContextMenu(ContextMenu menu, ContactViewModel contact)
    {
      IContactContextMenuService sv;

      sv = ApplicationService.Current.Context.GetService<IContactContextMenuService>();
      menu.Items.Clear();

      foreach (MenuItem x in sv.GetMenuItems(contact)) {
        menu.Items.Add(x);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }
  }
}

