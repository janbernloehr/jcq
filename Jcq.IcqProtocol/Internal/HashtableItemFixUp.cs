// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashtableItemFixUp.cs" company="Jan-Cornelius Molnar">
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

using System.Collections;
using JCsTools.Xml.Formatter;

namespace JCsTools.JCQ.IcqInterface
{
    public class HashtableItemFixUp : IFixUp
    {
        private readonly Hashtable _Hashtable;
        private readonly int _ItemId;
        private readonly string _Key;

        public HashtableItemFixUp(string key, int itemId, Hashtable hashtable)
        {
            _Key = Key;
            _ItemId = itemId;
            _Hashtable = hashtable;
        }

        public Hashtable Hashtable
        {
            get { return _Hashtable; }
        }

        public string Key
        {
            get { return _Key; }
        }

        public int ItemId
        {
            get { return _ItemId; }
        }

        void IFixUp.Execute(ISerializer serializer)
        {
            var item = serializer.GetDeserializeObjectById(ItemId);

            _Hashtable.Add(Key, item);
        }
    }
}