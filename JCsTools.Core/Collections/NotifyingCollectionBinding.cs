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
using System.Collections.Specialized;
using System.Windows.Threading;
namespace JCsTools.Core
{
  public class NotifyingCollectionBinding<T> : DispatcherObject
  {
    private INotifyCollectionChanged _source;
    private ObservableCollection<T> _target;

    public NotifyingCollectionBinding(INotifyCollectionChanged source, ObservableCollection<T> target)
    {
      _source = source;
      _target = target;

      _source.CollectionChanged += OnSourceChanged;
    }

    public void Close()
    {
      _source.CollectionChanged -= OnSourceChanged;
    }

    protected virtual void OnSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      try {
        switch (e.Action) {
          case Specialized.NotifyCollectionChangedAction.Add:
            TargetInsert(e.NewStartingIndex, GetTargetItem(e.NewItems(0)));
            break;
          case Specialized.NotifyCollectionChangedAction.Remove:
            TargetRemove(GetTargetItem(e.OldItems(0)));
            break;
          case Specialized.NotifyCollectionChangedAction.Replace:
            TargetItem(e.NewStartingIndex) = GetTargetItem(e.NewItems(0));
            break;
          case Specialized.NotifyCollectionChangedAction.Reset:
            TargetClear();
            break;
          case Specialized.NotifyCollectionChangedAction.Move:
            TargetMove(e.OldStartingIndex, e.NewStartingIndex);
            break;
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected virtual T GetTargetItem(object item)
    {
      return (T)item;
    }

    protected void TargetInsert(int index, T item)
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<int, T>(_target.Insert), index, item);
    }

    protected void TargetAdd(T item)
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<T>(_target.Add), item);
    }

    protected void TargetMove(int oldIndex, int newIndex)
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<int, int>(_target.Move), oldIndex, newIndex);
    }

    protected void TargetRemove(T item)
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<T>(_target.Remove), item);
    }

    protected T TargetItem {
      get { return _target(index); }
      set { Dispatcher.Invoke(DispatcherPriority.Normal, new Action<int, T>(TargetSetItem), index, value); }
    }

    protected void TargetSetItem(int index, T item)
    {
      _target(index) = item;
    }

    protected void TargetClear()
    {
      Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_target.Clear));
    }
  }
}

