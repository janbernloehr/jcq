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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
namespace JCsTools.JCQ.ViewModel
{
  public class ContactViewModelCollectionBinding : Core.NotifyingCollectionBinding<ContactViewModel>
  {
    public ContactViewModelCollectionBinding(INotifyCollectionChanged source, ObservableCollection<ContactViewModel> target) : base(source, target)
    {
    }

    protected override void OnSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action) {
        case Specialized.NotifyCollectionChangedAction.Add:
          TargetInsert(e.NewStartingIndex, ContactViewModelCache.GetViewModel((IcqInterface.Interfaces.IContact)e.NewItems(0)));
          break;
        case Specialized.NotifyCollectionChangedAction.Remove:
          TargetRemove(ContactViewModelCache.GetViewModel((IcqInterface.Interfaces.IContact)e.OldItems(0)));
          break;
        case Specialized.NotifyCollectionChangedAction.Replace:
          TargetItem(e.NewStartingIndex) = ContactViewModelCache.GetViewModel((IcqInterface.Interfaces.IContact)e.NewItems(0));
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
}

