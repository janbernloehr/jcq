// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigTypeKeySection.cs" company="Jan-Cornelius Molnar">
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
using System.Configuration;

namespace Jcq.Core.Configuration
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