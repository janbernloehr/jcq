// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactKeyedNotifiyingCollectionFormatter.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Xml;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.Xml.Formatter;

namespace JCsTools.JCQ.IcqInterface
{
    public class ContactKeyedNotifiyingCollectionFormatter : DefaultIListReferenceFormatter
    {
        private readonly NotifyingCollection<IContact> _Contacts;
        private readonly NotifyingCollection<IGroup> _Groups;

        public ContactKeyedNotifiyingCollectionFormatter(XmlSerializer parent)
            : base(parent, typeof (ContactKeyedNotifiyingCollectionFormatter))
        {
            _Contacts = new NotifyingCollection<IContact>();
            _Groups = new NotifyingCollection<IGroup>();
        }

        protected override object CreateObject(Type type, XmlReader reader)
        {
            object coll = new KeyedNotifiyingCollection<string, IContact>(c => c.Identifier);

            return coll;
        }
    }
}