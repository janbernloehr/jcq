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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace JCsTools.JCQ.ViewModel
{
  public class GroupViewModelCache
  {
    private static Dictionary<IcqInterface.Interfaces.IGroup, GroupViewModel> _Cache = new Dictionary<IcqInterface.Interfaces.IGroup, GroupViewModel>();

    public static GroupViewModel GetViewModel(IcqInterface.Interfaces.IGroup @group)
    {
      GroupViewModel vm;

      lock (_Cache) {
        if (_Cache.ContainsKey(@group)) {
          vm = _Cache(@group);
        } else {
          vm = new GroupViewModel(@group);
          _Cache.Add(@group, vm);
        }
      }

      return vm;
    }
  }

  public class GroupViewModelCollectionBinding : Core.NotifyingCollectionBinding<GroupViewModel>
  {
    public GroupViewModelCollectionBinding(INotifyCollectionChanged source, ObservableCollection<GroupViewModel> target) : base(source, target)
    {
    }

    protected override void OnSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action) {
        case Specialized.NotifyCollectionChangedAction.Add:
          TargetInsert(e.NewStartingIndex, GroupViewModelCache.GetViewModel((IcqInterface.Interfaces.IGroup)e.NewItems(0)));
          break;
        case Specialized.NotifyCollectionChangedAction.Remove:
          TargetRemove(GroupViewModelCache.GetViewModel((IcqInterface.Interfaces.IGroup)e.NewItems(0)));
          break;
        case Specialized.NotifyCollectionChangedAction.Replace:
          TargetItem(e.NewStartingIndex) = GroupViewModelCache.GetViewModel((IcqInterface.Interfaces.IGroup)e.NewItems(0));
          break;
        case Specialized.NotifyCollectionChangedAction.Reset:
          TargetClear();
          break;
        case Specialized.NotifyCollectionChangedAction.Move:
          TargetMove(e.OldStartingIndex, e.NewStartingIndex);
          break;
      }
    }
  }

  public class GroupViewModel : System.Windows.Threading.DispatcherObject, INotifyPropertyChanged
  {
    private IcqInterface.Interfaces.IGroup _Model;

    private ObservableCollection<ContactViewModel> _Contacts;
    private ContactViewModelCollectionBinding _ContactsBinding;
    private CollectionView _ContactsView;

    private ObservableCollection<GroupViewModel> _Groups;
    private CollectionView _GroupsView;
    private GroupViewModelCollectionBinding _GroupsBinding;

    public GroupViewModel(IcqInterface.Interfaces.IGroup model)
    {
      _Model = model;

      _Model.PropertyChanged += HandlePropertyChanged;
    }

    internal void OnContactPropertyChanged(object sender, ComponentModel.PropertyChangedEventArgs e)
    {
      ContactViewModel c = (ContactViewModel)sender;

      if (e.PropertyName == "StatusFlag" | e.PropertyName == "Name") {
        Dispatcher.BeginInvoke(Windows.Threading.DispatcherPriority.Background, new Action(ContactsView.Refresh));
      }
    }

    public IcqInterface.Interfaces.IGroup Model {
      get { return _Model; }
    }

    public ObservableCollection<ContactViewModel> Contacts {
      get {
        if (_Contacts == null) {
          lock (_Model.Contacts) {
            _Contacts = new ObservableCollection<ContactViewModel>((from c in _Model.ContactsContactViewModelCache.GetViewModel(c)).ToList);
            _ContactsBinding = new ContactViewModelCollectionBinding(_Model.Contacts, _Contacts);
          }
        }

        return _Contacts;
      }
    }

    public ObservableCollection<GroupViewModel> Groups {
      get {
        if (_Groups == null) {
          lock (_Model.Groups) {
            _Groups = new ObservableCollection<GroupViewModel>((from g in _Model.GroupsGroupViewModelCache.GetViewModel(g)).ToList);
            _GroupsBinding = new GroupViewModelCollectionBinding(_Model.Groups, _Groups);
          }
        }

        return _Groups;
      }
    }

    public CollectionView GroupsView {
      get {
        if (_GroupsView == null) {
          _GroupsView = (CollectionView)CollectionViewSource.GetDefaultView(Groups);
          _GroupsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        return _GroupsView;
      }
    }

    public CollectionView ContactsView {
      get {
        if (_ContactsView == null) {
          _ContactsView = (CollectionView)CollectionViewSource.GetDefaultView(Contacts);

          _ContactsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("StatusFlag", ComponentModel.ListSortDirection.Ascending));
          _ContactsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", ComponentModel.ListSortDirection.Ascending));
        }

        return _ContactsView;
      }
    }

    public System.Collections.Hashtable Attributes {
      get { return _Model.Attributes; }
    }

    public string Identifier {
      get { return _Model.Identifier; }
    }

    public string Name {
      get { return _Model.Name; }
    }

    protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, new Action<PropertyChangedEventArgs>(OnPropertyChanged), e);
    }

    protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, e);
      }
    }

    //Protected Sub XO(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
    //    Debug.WriteLine(String.Format("Changed: {0}", e.PropertyName), "XO")
    //End Sub

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
  }
}

