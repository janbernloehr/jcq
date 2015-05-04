// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewModelCollectionBinding.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class ContactViewModelCollectionBinding : NotifyingCollectionBinding<ContactViewModel>
    {
        public ContactViewModelCollectionBinding(INotifyCollectionChanged source,
            ObservableCollection<ContactViewModel> target) : base(source, target)
        {
        }

        protected override void OnSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TargetInsert(e.NewStartingIndex, ContactViewModelCache.GetViewModel((IContact) e.NewItems[0]));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    TargetRemove(ContactViewModelCache.GetViewModel((IContact) e.OldItems[0]));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    this[e.NewStartingIndex] = ContactViewModelCache.GetViewModel((IContact) e.NewItems[0]);
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