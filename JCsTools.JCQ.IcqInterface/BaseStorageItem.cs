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
namespace JCsTools.JCQ.IcqInterface
{
  public abstract class BaseStorageItem : Interfaces.IStorageItem, System.ComponentModel.INotifyPropertyChanged
  {
    private readonly System.Collections.Hashtable _Attributes;
    private string _Identifier;
    private string _Name;

    public BaseStorageItem()
    {
      _Attributes = new System.Collections.Hashtable();
    }

    public BaseStorageItem(string id, string name)
    {
      _Attributes = new System.Collections.Hashtable();
      _Identifier = id;
      _Name = name;
    }

    public System.Collections.Hashtable Interfaces.IStorageItem.Attributes {
      get { return _Attributes; }
    }

    public string Interfaces.IStorageItem.Identifier {
      get { return _Identifier; }
      set {
        _Identifier = value;
        OnPropertyChanged("Identifier");
      }
    }

    public string Interfaces.IStorageItem.Name {
      get { return string.IsNullOrEmpty(_Name) ? _Identifier : _Name; }
      set {
        _Name = value;
        OnPropertyChanged("Name");
      }
    }

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
  }
}

