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
namespace JCsTools.JCQ.ViewModel
{
  public class ExceptionWindowViewModel : System.ComponentModel.INotifyPropertyChanged
  {
    public ExceptionWindowViewModel()
    {
      JCsTools.Core.Kernel.Exceptions.ExceptionPublished += OnException;
    }

    private void OnException(object sender, Core.Interfaces.ExceptionEventArgs e)
    {
      Message += string.Format("{0}: {1}", System.DateTime.Now.ToLongTimeString, e.ExceptionInfo.Exception.ToString + System.Environment.NewLine + System.Environment.NewLine);
    }

    private string _Message;
    public string Message {
      get { return _Message; }
      set {
        _Message = value;

        OnPropertyChanged("Message");
      }
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
  }
}

