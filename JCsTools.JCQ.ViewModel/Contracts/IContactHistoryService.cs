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
  public interface IContactHistoryService : IcqInterface.Interfaces.IContextService
  {
    void Load();
    void Save();
    void AppendMessage(ContactViewModel contact, MessageViewModel message);
    List<MessageViewModel> GetHistory(ContactViewModel contact);
    List<MessageViewModel> GetHistory(ContactViewModel contact, int maxItems);
  }

  public class MessageViewModel
  {
    public MessageViewModel(System.DateTime created, ContactViewModel recipient, Brush foreground)
    {
      _DateCreated = created;
      _Recipient = recipient;
      _Foreground = foreground;
    }

    private readonly DateTime _DateCreated;
    public DateTime DateCreated {
      get { return _DateCreated; }
    }

    private readonly ContactViewModel _Recipient;
    public ContactViewModel Recipient {
      get { return _Recipient; }
    }

    private System.Windows.Media.Brush _Foreground;
    public System.Windows.Media.Brush Foreground {
      get { return _Foreground; }
    }
  }

  public class StatusChangedMessageViewModel : MessageViewModel
  {
    private readonly ContactViewModel _Sender;
    private readonly IcqInterface.Interfaces.IStatusCode _Status;

    public StatusChangedMessageViewModel(System.DateTime created, ContactViewModel sender, ContactViewModel recipient, IcqInterface.Interfaces.IStatusCode status, Brush foreground) : base(created, recipient, foreground)
    {

      _Sender = sender;
      _Status = status;
    }

    public ContactViewModel Sender {
      get { return _Sender; }
    }

    public IcqInterface.Interfaces.IStatusCode Status {
      get { return _Status; }
    }
  }

  public class TextMessageViewModel : MessageViewModel
  {
    public TextMessageViewModel(IcqInterface.Interfaces.IMessage message, Brush foreground) : this(System.DateTime.Now, ContactViewModelCache.GetViewModel(message.Sender), ContactViewModelCache.GetViewModel(message.Recipient), message.Text, foreground)
    {
    }

    public TextMessageViewModel(System.DateTime created, ContactViewModel sender, ContactViewModel recipient, string message, Brush foreground) : base(created, recipient, Foreground)
    {

      _Sender = sender;
      _Message = message;
    }

    private readonly ContactViewModel _Sender;
    public ContactViewModel Sender {
      get { return _Sender; }
    }

    private readonly string _Message;
    public string Message {
      get { return _Message; }
    }
  }

  public class OfflineTextMessageViewModel : TextMessageViewModel
  {
    public OfflineTextMessageViewModel(IcqInterface.IcqOfflineMessage message, Brush foreground) : this(System.DateTime.Now, ContactViewModelCache.GetViewModel(message.Recipient), ContactViewModelCache.GetViewModel(message.Sender), message.Text, message.OfflineSentDate, Foreground)
    {
    }

    public OfflineTextMessageViewModel(System.DateTime received, ContactViewModel sender, ContactViewModel recipient, string message, System.DateTime sent, Brush foreground) : base(received, sender, recipient, message, Foreground)
    {

      _DateSent = sent;
    }

    private readonly DateTime _DateSent;
    public DateTime DateSent {
      get { return _DateSent; }
    }
  }
}

