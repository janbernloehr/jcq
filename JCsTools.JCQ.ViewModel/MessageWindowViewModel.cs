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
  public class MessageWindowViewModel : System.ComponentModel.INotifyPropertyChanged
  {
    private System.Collections.ObjectModel.ObservableCollection<MessageViewModel> _Messages;
    private System.Collections.ObjectModel.ReadOnlyObservableCollection<MessageViewModel> _ReadOnlyMessages;

    private ContactViewModel _Contact;

    private ImageSource _ContactImage;
    private bool _ContactImageCreated;

    private CountDownTimer _TypingNotificationTimer;

    public event EventHandler ConatctInformationWindowRequested;
    public event EventHandler ContactHistoryWindowRequested;
    public event EventHandler ShowRequested;

    public MessageWindowViewModel(ContactViewModel contact)
    {
      _Contact = contact;

      ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IUserInformationService>.RequestShortUserInfo(contact.Model);

      _Messages = new System.Collections.ObjectModel.ObservableCollection<MessageViewModel>();
      _ReadOnlyMessages = new System.Collections.ObjectModel.ReadOnlyObservableCollection<MessageViewModel>(_Messages);

      ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IIconService>.RequestContactIcon(contact.Model);

      _TypingNotificationTimer = new CountDownTimer(TimeSpan.FromSeconds(6));
    }

    public ContactViewModel Contact {
      get { return _Contact; }
    }

    public System.Collections.ObjectModel.ReadOnlyObservableCollection<MessageViewModel> Messages {
      get { return _ReadOnlyMessages; }
    }

    private string _StatusText;
    public string StatusText {
      get { return _StatusText; }
      set {
        _StatusText = value;
        OnPropertyChanged("StatusText");
      }
    }

    public static void RegisterEventHandlers()
    {
      IcqInterface.Interfaces.IMessageService svMessaging;
      IcqInterface.Interfaces.INotificationService svNotification;
      IcqInterface.Interfaces.IStorageService svStorage;

      svMessaging = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IMessageService>();
      svNotification = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.INotificationService>();
      svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();

      svMessaging.MessageReceived += OnMessageReceived;
      svNotification.TypingNotificationEventArgs += OnTypingNotification;
      svStorage.ContactStatusChanged += OnContactStatusChanged;
    }

#region  Shared Event Handlers 

    private static void OnMessageReceived(object sender, IcqInterface.Interfaces.MessageEventArgs e)
    {
      try {
        Application.Current.Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, new Action<IcqInterface.Interfaces.IMessage>(ProcessMessageReceived), e.Message);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private static void OnContactStatusChanged(object sender, IcqInterface.Interfaces.StatusChangedEventArgs e)
    {
      try {
        Application.Current.Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, new Action<IcqInterface.Interfaces.StatusChangedEventArgs>(ProcessStatusChanged), e);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private static void OnTypingNotification(object sender, IcqInterface.Interfaces.TypingNotificationEventArgs e)
    {
      try {
        Application.Current.Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, new Action<IcqInterface.Interfaces.TypingNotificationEventArgs>(ProcessTypingNotification), e);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

#endregion

#region  Shared Event Processors 

    private static void ProcessMessageReceived(IcqInterface.Interfaces.IMessage msg)
    {
      MessageWindowViewModel vm;
      ContactViewModel contact;

      IcqInterface.IcqOfflineMessage offlineMsg;
      TextMessageViewModel message;

      try {
        contact = ContactViewModelCache.GetViewModel(msg.Sender);

        vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.GetMessageWindowViewModel(contact);

        offlineMsg = msg as IcqInterface.IcqOfflineMessage;

        if (offlineMsg != null) {
          message = new OfflineTextMessageViewModel(offlineMsg, MessageColors.Contact1Color);
        } else {
          message = new TextMessageViewModel(msg, MessageColors.Contact1Color);
        }

        vm.DisplayMessage(message);
        vm.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private static void ProcessStatusChanged(IcqInterface.Interfaces.StatusChangedEventArgs e)
    {
      MessageWindowViewModel vm;
      ContactViewModel contact;
      StatusChangedMessageViewModel message;

      try {
        contact = ContactViewModelCache.GetViewModel(e.Contact);

        if (ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.IsMessageWindowViewModelAvailable(contact)) {
          vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.GetMessageWindowViewModel(contact);

          message = new StatusChangedMessageViewModel(System.DateTime.Now, contact, vm.Contact, e.Status, MessageColors.SystemColor);

          vm.DisplayStatus(message);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private static void ProcessTypingNotification(IcqInterface.Interfaces.TypingNotificationEventArgs e)
    {
      MessageWindowViewModel vm;
      ContactViewModel contact;

      try {
        contact = ContactViewModelCache.GetViewModel(e.Contact);

        if (ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.IsMessageWindowViewModelAvailable(contact)) {
          vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.GetMessageWindowViewModel(contact);
          vm.DisplayTypingNotification(e.NotificationType);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

#endregion

    public void RequestContactInformationWindow()
    {
      if (ConatctInformationWindowRequested != null) {
        ConatctInformationWindowRequested(this, EventArgs.Empty);
      }
    }

    public void RequestChatHistoryWindow()
    {
      if (ContactHistoryWindowRequested != null) {
        ContactHistoryWindowRequested(this, EventArgs.Empty);
      }
    }

    public void SendMessage(string text)
    {
      IcqInterface.IcqMessage msg;
      TextMessageViewModel message;

      msg = new IcqInterface.IcqMessage();
      msg.Sender = ApplicationService.Current.Context.Identity;
      msg.Recipient = Contact.Model;
      msg.Text = text;

      ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IMessageService>.SendMessage(msg);

      message = new TextMessageViewModel(msg, MessageColors.IdentityColor);

      DisplayMessage(message);
    }

    public void Show()
    {
      if (ShowRequested != null) {
        ShowRequested(this, EventArgs.Empty);
      }
    }

    public void Close()
    {
      ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.RemoveMessageWindowViewModel(Contact);
    }

    private void DisplayTypingNotification(IcqInterface.Interfaces.NotificationType status)
    {
      switch (status) {
        case IcqInterface.Interfaces.NotificationType.TypingFinished:
          StatusText = string.Format("{0} finished typing a message.", Contact.Name);
          break;
        case IcqInterface.Interfaces.NotificationType.TypingStarted:
          StatusText = string.Format("{0} is typing a message.", Contact.Name);
          break;
        case IcqInterface.Interfaces.NotificationType.TypingCanceled:
          StatusText = string.Format("{0} cleared the entered text.", Contact.Name);
          break;
      }
    }

    private MessageSenderRole GetSenderRole(IcqInterface.Interfaces.IMessage message)
    {
      if (message.Sender.Identifier == ApplicationService.Current.Context.Identity.Identifier) {
        return MessageSenderRole.ContextIdentity;
      } else {
        return MessageSenderRole.FirstAtt;
      }
    }

    private void DisplayMessage(TextMessageViewModel message)
    {
      //Paragraph = Core.Kernel.Services.GetService(Of IMessageFormattingService).FormatMessage(GetSenderRole(message), message)
      ApplicationService.Current.Context.GetService<IContactHistoryService>.AppendMessage(Contact, message);

      if (object.ReferenceEquals(message.Sender, Contact)) {
        StatusText = string.Format("Last message received on {0}.", message.DateCreated.ToShortTimeString);
      } else {
        StatusText = string.Format("Last message sent on {0}.", message.DateCreated.ToShortTimeString);
      }

      _Messages.Add(message);
      //Document.Blocks.Add(paragraph)

      if (DocumentScrollRequired != null) {
        DocumentScrollRequired(this, EventArgs.Empty);
      }
    }

    private void DisplayStatus(StatusChangedMessageViewModel message)
    {
      //Dim paragraph As Paragraph

      //paragraph = Core.Kernel.Services.GetService(Of IMessageFormattingService).FormatStatus(Contact, status)

      //Document.Blocks.Add(paragraph)

      _Messages.Add(message);
    }

    private System.Windows.Threading.DispatcherTimer textChangedTimer;

    public void SendTypingNotification(TextChangedAction action)
    {
      switch (action) {
        case TextChangedAction.Changed:
          if (_TypingNotificationTimer.IsRunning) {
            _TypingNotificationTimer.Reset();
          } else {
            _TypingNotificationTimer.Start();
            SendTypingNotification(IcqInterface.Interfaces.NotificationType.TypingStarted);
          }
          break;
        case TextChangedAction.Cleared:
          _TypingNotificationTimer.Stop();
          SendTypingNotification(IcqInterface.Interfaces.NotificationType.TypingCanceled);
          break;
      }
    }

    /// <summary>
    /// Sends a notification that the user is typing something, has finished typing and cleared his text.
    /// </summary>
    private void SendTypingNotification(IcqInterface.Interfaces.NotificationType type)
    {
      IcqInterface.Interfaces.INotificationService svNotification;

      svNotification = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.INotificationService>();

      svNotification.SendNotification(Contact.Model, type);
    }

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }

    public event EventHandler DocumentScrollRequired;

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);

    private void  // ERROR: Handles clauses are not supported in C#
OnTypingNotificationTimer_Tick(object sender, System.EventArgs e)
    {
      try {
        SendTypingNotification(IcqInterface.Interfaces.NotificationType.TypingFinished);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }
  }

  public class CountDownTimer : IDisposable
  {
    private System.Threading.Timer _timer;

    public event EventHandler Tick;

    public CountDownTimer(TimeSpan countDownDue)
    {
      double actualMilliseconds;

      actualMilliseconds = Math.Round(countDownDue.TotalMilliseconds);

      if (actualMilliseconds > Int32.MaxValue)
        throw new ArgumentOutOfRangeException("countDownDue", "No more than Int32.MaxValue milliseconds.");

      _CountDownDue = Convert.ToInt32(actualMilliseconds);

      _timer = new System.Threading.Timer(OnTimerCallback, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
    }

    private void OnTimerCallback(object state)
    {
      this.Stop();
      if (Tick != null) {
        Tick(this, EventArgs.Empty);
      }
    }

    private int _CountDownDue;

    private bool _IsRunning;
    public bool IsRunning {
      get { return _IsRunning; }
    }

    public void Start()
    {
      _timer.Change(_CountDownDue, System.Threading.Timeout.Infinite);
      _IsRunning = true;
    }

    public void Reset()
    {
      _timer.Change(_CountDownDue, System.Threading.Timeout.Infinite);
    }

    public void Stop()
    {
      _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
      _IsRunning = false;
    }

    private bool disposedValue = false;

    // IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue & disposing) {
        if (_timer != null)
          _timer.Dispose();
      }
      this.disposedValue = true;
    }

#region  IDisposable Support 
    // This code added by Visual Basic to correctly implement the disposable pattern.
    public void IDisposable.Dispose()
    {
      // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
      Dispose(true);
      GC.SuppressFinalize(this);
    }
#endregion

  }

  public enum TextChangedAction
  {
    Changed,
    Cleared
  }

  public class MessageColors
  {
    public static Brush IdentityColor {
      get { return Brushes.Red; }
    }

    public static Brush SystemColor {
      get { return Brushes.Gray; }
    }

    public static Brush HistoryColor {
      get { return Brushes.Gray; }
    }

    public static Brush Contact1Color {
      get { return Brushes.Blue; }
    }

    public static Brush Contact2Color {
      get { return Brushes.Green; }
    }

    public static Brush Contact3Color {
      get { return Brushes.Orange; }
    }

    public static Brush Contact4Color {
      get { return Brushes.Purple; }
    }
  }
}

