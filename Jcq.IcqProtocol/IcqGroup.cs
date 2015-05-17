// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqGroup.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.Core;
using JCsTools.Core.Interfaces;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqGroup : BaseStorageItem, IGroup
    {
        private int _groupId;

        public IcqGroup(string identifier, int groupId)
            : base(identifier, identifier)
        {
            Contacts = new NotifyingCollection<IcqContact>();
            Groups = new NotifyingCollection<IcqGroup>();

            _groupId = groupId;
        }

        public int GroupId
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                OnPropertyChanged();
            }
        }

        public NotifyingCollection<IcqContact> Contacts { get; private set; }
        public NotifyingCollection<IcqGroup> Groups { get; private set; }

        #region IGroup Interface Members

        IReadOnlyNotifyingCollection<IContact> IGroup.Contacts
        {
            get { return Contacts; }
        }

        IReadOnlyNotifyingCollection<IGroup> IGroup.Groups
        {
            get { return Groups; }
        }

        #endregion
    }
}