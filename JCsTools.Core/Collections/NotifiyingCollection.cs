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
using System.ComponentModel;
namespace JCsTools.Core
{
  public class NotifyingCollection<T> : Collection<T>, Interfaces.INotifyingCollection<T>
  {
    public NotifyingCollection()
    {

    }

    public NotifyingCollection(IList<T> list) : base(list)
    {
    }

    public void Interfaces.INotifyingCollection<T>.Move(int oldIndex, int newIndex)
    {
      this.MoveItem(oldIndex, newIndex);
    }

    protected override void InsertItem(int index, T item)
    {
      base.InsertItem(index, item);

      OnPropertyChanged("Count");
      OnPropertyChanged("Item[]");
      OnCollectionChanged(Specialized.NotifyCollectionChangedAction.Add, item, index);
    }

    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
      T item = base.Item(oldIndex);
      base.RemoveItem(oldIndex);
      base.InsertItem(newIndex, item);
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(Specialized.NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
    }

    protected override void SetItem(int index, T item)
    {
      base.SetItem(index, item);
      T oldItem = base.Item(index);

      OnPropertyChanged("Item[]");
      OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
    }

    protected override void RemoveItem(int index)
    {
      T item = base.Item(index);

      base.RemoveItem(index);

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

    protected void OnCollectionChanged(NotifyCollectionChangedAction action, T changedItem, int index)
    {
      if (CollectionChanged != null) {
        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
      }
    }

    protected void OnCollectionChanged(NotifyCollectionChangedAction action, T oldItem, T newItem, int index)
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
  }
}

