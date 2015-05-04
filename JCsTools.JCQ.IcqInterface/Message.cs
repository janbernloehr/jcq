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
using JCsTools.JCQ.IcqInterface.Interfaces;
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqMessage : Interfaces.IMessage
  {
    private readonly Hashtable _Attributes = new Hashtable();

    public IcqMessage()
    {

    }

    public IcqMessage(IContact sender, IContact recipient, string text)
    {
      _Attributes("Sender") = sender;
      _Attributes("Recipient") = recipient;
      _Attributes("Text") = text;
    }

    public System.Collections.Hashtable Interfaces.IMessage.Attributes {
      get { return _Attributes; }
    }

    public IContact Interfaces.IMessage.Sender {
      get { return (IContact)_Attributes("Sender"); }
      set { _Attributes("Sender") = value; }
    }

    public IContact Interfaces.IMessage.Recipient {
      get { return (IContact)_Attributes("Recipient"); }
      set { _Attributes("Recipient") = value; }
    }

    public string Interfaces.IMessage.Text {
      get { return (string)_Attributes("Text"); }
      set { _Attributes("Text") = value; }
    }
  }

  public class IcqOfflineMessage : IcqMessage
  {
    public IcqOfflineMessage(IContact sender, IContact recipient, string text, DateTime dateSent) : base(sender, recipient, text)
    {
      Attributes("OfflineSentDate") = dateSent;
    }

    public System.DateTime OfflineSentDate {
      get { return (System.DateTime)Attributes("OfflineSentDate"); }
      set { Attributes("OfflineSentDate") = value; }
    }
  }
}

