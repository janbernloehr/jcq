// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigTypeKeySection.cs" company="Jan-Cornelius Molnar">
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
using System.Configuration;

namespace JCsTools.Core.Configuration
{
    public abstract class ConfigTypeKeySection<T> : ConfigurationElementCollection where T : ConfigurationElement
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public new string AddElementName
        {
            get { return base.AddElementName; }
            set { base.AddElementName = value; }
        }

        public new string ClearElementName
        {
            get { return base.ClearElementName; }
            set { base.AddElementName = value; }
        }

        public new string RemoveElementName
        {
            get { return base.RemoveElementName; }
        }

        public new int Count
        {
            get { return base.Count; }
        }

        public T this[int index]
        {
            get { return (T) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new T this[string typeKey]
        {
            get { return (T) BaseGet(typeKey); }
        }

        public T this[Type type]
        {
            get { return (T) BaseGet(GetTypeKey(type)); }
        }

        protected abstract override ConfigurationElement CreateNewElement();
        protected abstract override object GetElementKey(ConfigurationElement element);

        public bool Contains(string typeKey)
        {
            return BaseGet(typeKey) != null;
        }

        public bool Contains(Type type)
        {
            return BaseGet(GetTypeKey(type)) != null;
        }

        public bool Contains(T element)
        {
            return BaseIndexOf(element) > -1;
        }

        public int IndexOf(T element)
        {
            return BaseIndexOf(element);
        }

        protected string GetTypeKey(Type type)
        {
            return type.ToString().Split('`')[0];
        }

        public void Add(T element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            base.BaseAdd(element, false);
        }

        public void Remove(T element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(GetElementKey(element));
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string typeKey)
        {
            BaseRemove(typeKey);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}