// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupViewModelCollectionBinding.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Jcq.Core.Collections;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.Ux.ViewModel
{
    public class GroupViewModelCollectionBinding : NotifyingCollectionBinding<GroupViewModel>
    {
        public GroupViewModelCollectionBinding(INotifyCollectionChanged source,
            ObservableCollection<GroupViewModel> target) : base(source, target)
        {
        }

        protected override void OnSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TargetInsert(e.NewStartingIndex, GroupViewModelCache.GetViewModel((IGroup) e.NewItems[0]));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    TargetRemove(GroupViewModelCache.GetViewModel((IGroup) e.NewItems[0]));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    this[e.NewStartingIndex] = GroupViewModelCache.GetViewModel((IGroup) e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TargetClear();
                    break;
                case NotifyCollectionChangedAction.Move:
                    TargetMove(e.OldStartingIndex, e.NewStartingIndex);
                    break;
            }
        }
    }
}