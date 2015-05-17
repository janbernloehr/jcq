// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyedNotifiyingCollection.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using JCsTools.Core.Interfaces;

namespace JCsTools.Core
{
    public class KeyedNotifiyingCollection<TKey, TValue> : KeyedCollection<TKey, TValue>, INotifyingCollection<TValue>
    {
        private readonly Func<TValue, TKey> _keySelector;

        public KeyedNotifiyingCollection(Func<TValue, TKey> keySelector)
        {
            _keySelector = keySelector;
        }

        void INotifyingCollection<TValue>.Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void InsertItem(int index, TValue item)
        {
            base.InsertItem(index, item);

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            var item = base[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
        }

        protected override void SetItem(int index, TValue item)
        {
            base.SetItem(index, item);

            var oldItem = base[index];

            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            var item = base[index];

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
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, TValue changedItem, int index)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
            }
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, TValue oldItem, TValue newItem,
            int index)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected override TKey GetKeyForItem(TValue item)
        {
            return _keySelector(item);
        }
    }
}