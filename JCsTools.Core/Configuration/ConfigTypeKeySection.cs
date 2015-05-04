//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core.Configuration
{
  public abstract class ConfigTypeKeySection<T> : System.Configuration.ConfigurationElementCollection where T : System.Configuration.ConfigurationElement
  {
    public ConfigTypeKeySection()
    {

    }

    public override System.Configuration.ConfigurationElementCollectionType CollectionType {
      get { return System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMap; }
    }

    protected override abstract System.Configuration.ConfigurationElement CreateNewElement();

    protected override abstract object GetElementKey(System.Configuration.ConfigurationElement element);

    public new string AddElementName {
      get { return base.AddElementName; }
      set { base.AddElementName = value; }
    }

    public new string ClearElementName {
      get { return base.ClearElementName; }
      set { base.AddElementName = value; }
    }

    public new string RemoveElementName {
      get { return base.RemoveElementName; }
    }

    public new int Count {
      get { return base.Count; }
    }

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
      return type.ToString.Split('`')(0);
    }

    public new T this[int index] {
      get { return (T)BaseGet(index); }
      set {
        if (!(BaseGet(index) == null)) {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public new T this[string typeKey] {
      get { return (T)BaseGet(typeKey); }
    }

    public new T this[Type type] {
      get { return (T)BaseGet(GetTypeKey(type)); }
    }

    public void Add(T element)
    {
      BaseAdd(element);
    }

    protected override void BaseAdd(System.Configuration.ConfigurationElement element)
    {
      base.BaseAdd(element, false);
    }

    public void Remove(T element)
    {
      if (BaseIndexOf(element) >= 0) {
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

