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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
namespace JCsTools.Core
{
  public class KeyedNotifiyingCollection<TKey, TValue> : KeyedCollection<TKey, TValue>, Interfaces.INotifyingCollection<TValue>
  {
    private Func<TValue, TKey> _keySelector;

    public KeyedNotifiyingCollection(Func<TValue, TKey> keySelector)
    {
      _keySelector = keySelector;
    }

    public void Interfaces.INotifyingCollection<TValue>.Move(int oldIndex, int newIndex)
    {
      this.MoveItem(oldIndex, newIndex);
    }

    protected override void InsertItem(int index, TValue item)
    {
      base.InsertItem(index, item);

      OnPropertyChanged("Count");
      OnPropertyChanged("Item[]");
      OnCollectionChanged(Specialized.NotifyCollectionChangedAction.Add, item, index);
    }

    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
      TValue item = base.Item(oldIndex);
      base.RemoveItem(oldIndex);
      base.InsertItem(newIndex, item);
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(Specialized.NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
    }

    protected override void SetItem(int index, TValue item)
    {
      base.SetItem(index, item);

      TValue oldItem = base.Item(index);

      OnPropertyChanged("Item[]");
      OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
    }

    protected override void RemoveItem(int index)
    {
      base.RemoveItem(index);

      TValue item = base.Item(index);

      OnPropertyChanged("Count");
      OnPropertyChanged("Item[]");
      OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
    }

    protected override void ClearItems()
    {
      base.ClearItems();

      OnPropertyChanged("Count");
      OnPropertyChanged("Item[]");
      OnCollectionChanged(NotifyCollectionChangedAction.Reset);
    }

    protected void OnCollectionChanged(NotifyCollectionChangedAction action)
    {
      if (CollectionChanged != null) {
        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
      }
    }

    protected void OnCollectionChanged(NotifyCollectionChangedAction action, TValue changedItem, int index)
    {
      if (CollectionChanged != null) {
        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
      }
    }

    protected void OnCollectionChanged(NotifyCollectionChangedAction action, TValue oldItem, TValue newItem, int index)
    {
      if (CollectionChanged != null) {
        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
      }
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
    {
      if (CollectionChanged != null) {
        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
      }
    }

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }

    public event CollectionChangedEventHandler CollectionChanged;
    public delegate void CollectionChangedEventHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);

    protected override TKey GetKeyForItem(TValue item)
    {
      return _keySelector(item);
    }

  }
}

