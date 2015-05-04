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
namespace JCsTools.IdentityManager
{
  public class Identity : IIdentity
  {
    private Dictionary<string, object> values = new Dictionary<string, object>();
    private string _Description;
    private string _Identifier;
    private string _ImageUrl;

    public Identity(string id)
    {
      _Identifier = id;
    }

    public string IIdentity.Identifier {
      get { return _Identifier; }
      set { _Identifier = value; }
    }

    public string IIdentity.Description {
      get { return _Description; }
      set { _Description = value; }
    }

    public string IIdentity.ImageUrl {
      get { return _ImageUrl; }
      set { _ImageUrl = value; }
    }

    public T IIdentity.GetAttribute<T>(IIdentityAttribute<T> key)
    {
      return (T)GetAttribute(key.AttributeName);
    }

    public string[] IIdentity.GetAttributeNames()
    {
      return values.Keys.ToArray;
    }

    public void IIdentity.SetAttribute<T>(IIdentityAttribute<T> key, T value)
    {
      SetAttribute(key.AttributeName, value);
    }

    public void IIdentity.SetAttribute(string key, object value)
    {
      if (values.ContainsKey(key)) {
        values(key) = value;
      } else {
        values.Add(key, value);
      }
    }

    public object IIdentity.GetAttribute(string key)
    {
      return values(key);
    }

    public bool IIdentity.HasAttribute(string key)
    {
      return values.ContainsKey(key);
    }

    public bool IIdentity.HasAttribute<T>(IIdentityAttribute<T> key)
    {
      return HasAttribute(key.AttributeName);
    }
  }
}

