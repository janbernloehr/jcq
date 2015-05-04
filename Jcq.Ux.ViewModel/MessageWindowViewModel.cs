// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageWindowViewModel.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class MessageWindowViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<MessageViewModel> _messages;
        private readonly CountDownTimer _typingNotificationTimer;
        //private ImageSource _ContactImage;
        //private bool _ContactImageCreated;
        private string _statusText;
        //private DispatcherTimer _textChangedTimer;

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

        //private MessageSenderRole GetSenderRole(IMessage message)
        //{
        //    if (message.Sender.Identifier == ApplicationService.Current.Context.Identity.Identifier)
        //    {
        //        return MessageSenderRole.ContextIdentity;
        //    }
        //    return MessageSenderRole.FirstAtt;
        //}

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