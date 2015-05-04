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
namespace JCsTools.JCQ.IcqInterface.Interfaces
{
  public interface INotificationService : IContextService
  {
    event TypingNotificationEventArgsEventHandler TypingNotificationEventArgs;
    delegate void TypingNotificationEventArgsEventHandler(object sender, TypingNotificationEventArgs e);
    void SendNotification(IContact receiver, NotificationType type);
  }

  public class TypingNotificationEventArgs : EventArgs
  {
    public TypingNotificationEventArgs(Interfaces.IContact contact, NotificationType type)
    {
      _Contact = contact;
      _NotificationType = type;
    }

    private NotificationType _NotificationType;
    public NotificationType NotificationType {
      get { return _NotificationType; }
      set { _NotificationType = value; }
    }

    private Interfaces.IContact _Contact;
    public Interfaces.IContact Contact {
      get { return _Contact; }
      set { _Contact = value; }
    }
  }

  public enum NotificationType
  {
    TypingCanceled,
    TypingStarted,
    TypingFinished
  }
}

