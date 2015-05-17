// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyingCollectionBinding.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Specialized;
using System.Windows.Threading;

namespace JCsTools.Core
{
    public class NotifyingCollectionBinding<T> : DispatcherObject
    {
        private readonly INotifyCollectionChanged _source;
        private readonly ObservableCollection<T> _target;

        public NotifyingCollectionBinding(INotifyCollectionChanged source, ObservableCollection<T> target)
        {
            _source = source;
            _target = target;

            _source.CollectionChanged += OnSourceChanged;
        }

        protected T this[int index]
        {
            get { return _target[index]; }
            set { Dispatcher.Invoke(DispatcherPriority.Normal, new Action<int, T>(TargetSetItem), index, value); }
        }

        public void Close()
        {
            _source.CollectionChanged -= OnSourceChanged;
        }

        protected virtual void OnSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        TargetInsert(e.NewStartingIndex, GetTargetItem(e.NewItems[0]));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        TargetRemove(GetTargetItem(e.OldItems[0]));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        this[e.NewStartingIndex] = GetTargetItem(e.NewItems[0]);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        TargetClear();
                        break;
                    case NotifyCollectionChangedAction.Move:
                        TargetMove(e.OldStartingIndex, e.NewStartingIndex);
                        break;
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected virtual T GetTargetItem(object item)
        {
            return (T) item;
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
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<T>(s => _target.Remove(s)), item);
        }

        protected void TargetSetItem(int index, T item)
        {
            _target[index] = item;
        }

        protected void TargetClear()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_target.Clear));
        }
    }
}