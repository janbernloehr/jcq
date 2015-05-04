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
namespace JCsTools.Core
{
  public class ReadOnlyNotifyingCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, Interfaces.IReadOnlyNotifyingCollection<T>
  {
    public ReadOnlyNotifyingCollection(NotifyingCollection<T> list) : base(list)
    {

      list.CollectionChanged += HandleCollectionChanged;
      list.PropertyChanged += HandlePropertyChanged;
    }

    protected void HandleCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      OnCollectionChanged(e);
    }

    protected void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      OnPropertyChanged(e);
    }

    protected void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (CollectionChanged != null) {
        CollectionChanged(this, e);
      }
    }

    protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, e);
      }
    }

    public event CollectionChangedEventHandler CollectionChanged;
    public delegate void CollectionChangedEventHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);

  }
}

