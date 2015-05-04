// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqGroup.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.Core;
using JCsTools.Core.Interfaces;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqGroup : BaseStorageItem, IGroup
    {
        private readonly NotifyingCollection<IContact> _Contacts;
        private readonly NotifyingCollection<IGroup> _Groups;

        public IcqGroup(string id, int groupId) : base(id, id)
        {
            _Contacts = new NotifyingCollection<IContact>();
            _Groups = new NotifyingCollection<IGroup>();

            Attributes["GroupId"] = groupId;
        }

        public int GroupId
        {
            get { return (int) Attributes["GroupId"]; }
            set { Attributes["GroupId"] = value; }
        }

        INotifyingCollection<IContact> IGroup.Contacts
        {
            get { return _Contacts; }
        }

        INotifyingCollection<IGroup> IGroup.Groups
        {
            get { return _Groups; }
        }
    }
}