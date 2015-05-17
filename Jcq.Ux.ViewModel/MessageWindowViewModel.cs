// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageWindowViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Jcq.Core;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.ViewModel
{
    public class MessageWindowViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<MessageViewModel> _messages;
        private readonly CountDownTimer _typingNotificationTimer;
        private string _statusText;

        public MessageWindowViewModel(ContactViewModel contact)
        {
            Contact = contact;

            ApplicationService.Current.Context.GetService<IUserInformationService>().RequestShortUserInfo(contact.Model);

            _messages = new ObservableCollection<MessageViewModel>();
            Messages = new ReadOnlyObservableCollection<MessageViewModel>(_messages);

            ApplicationService.Current.Context.GetService<IIconService>().RequestContactIcon(contact.Model);

            _typingNotificationTimer = new CountDownTimer(TimeSpan.FromSeconds(6));
            _typingNotificationTimer.Tick += OnTypingNotificationTimer_Tick;
        }

        public ContactViewModel Contact { get; private set; }
        public ReadOnlyObservableCollection<MessageViewModel> Messages { get; private set; }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ConatctInformationWindowRequested;
        public event EventHandler ContactHistoryWindowRequested;
        public event EventHandler ShowRequested;

        public static void RegisterEventHandlers()
        {
            var svMessaging = ApplicationService.Current.Context.GetService<IMessageService>();
            var svNotification = ApplicationService.Current.Context.GetService<INotificationService>();
            var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();

            svMessaging.MessageReceived += OnMessageReceived;
            svNotification.TypingNotification += OnTypingNotification;
            svStorage.ContactStatusChanged += OnContactStatusChanged;
        }

        public void RequestContactInformationWindow()
        {
            if (ConatctInformationWindowRequested != null)
            {
                ConatctInformationWindowRequested(this, EventArgs.Empty);
            }
        }

        public void RequestChatHistoryWindow()
        {
            if (ContactHistoryWindowRequested != null)
            {
                ContactHistoryWindowRequested(this, EventArgs.Empty);
            }
        }

        public void SendMessage(string text)
        {
            var msg = new IcqMessage
            {
                Sender = ApplicationService.Current.Context.Identity,
                Recipient = Contact.Model,
                Text = text
            };

            ApplicationService.Current.Context.GetService<IMessageService>().SendMessage(msg);

            var message = new TextMessageViewModel(msg, MessageColors.IdentityColor);

            DisplayMessage(message);
        }

        public void Show()
        {
            if (ShowRequested != null)
            {
                ShowRequested(this, EventArgs.Empty);
            }
        }

        public void Close()
        {
            ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                .RemoveMessageWindowViewModel(
                    Contact);
        }

        private void DisplayTypingNotification(NotificationType status)
        {
            switch (status)
            {
                case NotificationType.TypingFinished:
                    StatusText = string.Format("{0} finished typing a message.", Contact.Name);
                    break;
                case NotificationType.TypingStarted:
                    StatusText = string.Format("{0} is typing a message.", Contact.Name);
                    break;
                case NotificationType.TypingCanceled:
                    StatusText = string.Format("{0} cleared the entered text.", Contact.Name);
                    break;
            }
        }


        private void DisplayMessage(TextMessageViewModel message)
        {
            ApplicationService.Current.Context.GetService<IContactHistoryService>().AppendMessage(Contact, message);

            if (message.Sender == Contact)
            {
                StatusText = string.Format("Last message received on {0}.", message.DateCreated.ToShortTimeString());
            }
            else
            {
                StatusText = string.Format("Last message sent on {0}.", message.DateCreated.ToShortTimeString());
            }

            _messages.Add(message);

            if (DocumentScrollRequired != null)
            {
                DocumentScrollRequired(this, EventArgs.Empty);
            }
        }

        private void DisplayStatus(StatusChangedMessageViewModel message)
        {
            _messages.Add(message);
        }

        public void SendTypingNotification(TextChangedAction action)
        {
            switch (action)
            {
                case TextChangedAction.Changed:
                    if (_typingNotificationTimer.IsRunning)
                    {
                        _typingNotificationTimer.Reset();
                    }
                    else
                    {
                        _typingNotificationTimer.Start();
                        SendTypingNotification(NotificationType.TypingStarted);
                    }
                    break;
                case TextChangedAction.Cleared:
                    _typingNotificationTimer.Stop();
                    SendTypingNotification(NotificationType.TypingCanceled);
                    break;
            }
        }

        /// <summary>
        ///     Sends a notification that the user is typing something, has finished typing and cleared his text.
        /// </summary>
        private void SendTypingNotification(NotificationType type)
        {
            var svNotification = ApplicationService.Current.Context.GetService<INotificationService>();

            svNotification.SendNotification(Contact.Model, type);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event EventHandler DocumentScrollRequired;

        private void OnTypingNotificationTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                SendTypingNotification(NotificationType.TypingFinished);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #region  Shared Event Handlers 

        private static void OnMessageReceived(object sender, MessageEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action<IMessage>(ProcessMessageReceived), e.Message);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private static void OnContactStatusChanged(object sender, StatusChangedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action<StatusChangedEventArgs>(ProcessStatusChanged), e);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private static void OnTypingNotification(object sender, TypingNotificationEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action<TypingNotificationEventArgs>(ProcessTypingNotification), e);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #endregion

        #region  Shared Event Processors 

        private static void ProcessMessageReceived(IMessage msg)
        {
            try
            {
                var contact = ContactViewModelCache.GetViewModel(msg.Sender);

                var vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .GetMessageWindowViewModel(contact);

                var offlineMsg = msg as IcqOfflineMessage;

                TextMessageViewModel message;

                if (offlineMsg != null)
                {
                    message = new OfflineTextMessageViewModel(offlineMsg, MessageColors.Contact1Color);
                }
                else
                {
                    message = new TextMessageViewModel(msg, MessageColors.Contact1Color);
                }

                vm.DisplayMessage(message);
                vm.Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private static void ProcessStatusChanged(StatusChangedEventArgs e)
        {
            try
            {
                var contact = ContactViewModelCache.GetViewModel(e.Contact);

                if (!ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .IsMessageWindowViewModelAvailable(contact)) return;

                var vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .GetMessageWindowViewModel(contact);

                var message = new StatusChangedMessageViewModel(DateTime.Now, contact, vm.Contact, e.Status,
                    MessageColors.SystemColor);

                vm.DisplayStatus(message);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private static void ProcessTypingNotification(TypingNotificationEventArgs e)
        {
            try
            {
                var contact = ContactViewModelCache.GetViewModel(e.Contact);

                if (!ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .IsMessageWindowViewModelAvailable(contact)) return;

                var vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .GetMessageWindowViewModel(contact);
                vm.DisplayTypingNotification(e.NotificationType);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #endregion
    }
}