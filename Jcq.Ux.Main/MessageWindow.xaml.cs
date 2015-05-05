// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using JCsTools.Core;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class MessageWindow : Window
    {
        public MessageWindow(ContactViewModel contact)
        {
            ViewModel = new MessageWindowViewModel(contact);

            ViewModel.ShowRequested += OnShowRequested;
            ViewModel.ConatctInformationWindowRequested += OnConatctInformationWindowRequested;
            ViewModel.ContactHistoryWindowRequested += OnContactHistoryWindowRequested;
            ViewModel.DocumentScrollRequired += OnDocumentScrollRequired;

            DataContext = ViewModel;

            InitializeComponent();

            App.DefaultWindowStyle.Attach(this);

            txtMessage.Focus();
        }

        public MessageWindowViewModel ViewModel { get; private set; }

        private void OnShowRequested(object sender, EventArgs e)
        {
            try
            {
                Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnConatctInformationWindowRequested(object sender, EventArgs e)
        {
            ContactInformationWindow wnd;

            try
            {
                wnd = new ContactInformationWindow(ViewModel.Contact);
                wnd.Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnContactHistoryWindowRequested(object sender, EventArgs e)
        {
            HistoryWindow wnd;

            try
            {
                wnd = new HistoryWindow(ViewModel.Contact);
                wnd.Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnShowContactInformationClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.RequestContactInformationWindow();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnShowChatHistoryClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.RequestChatHistoryWindow();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnSendMessageClick(object sender, RoutedEventArgs e)
        {
            string messageText;

            try
            {
                messageText = txtMessage.Text;

                ViewModel.SendMessage(messageText);

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void // ERROR: Handles clauses are not supported in C#
            MessageWindow_Activated(object sender, EventArgs e)
        {
            try
            {
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void // ERROR: Handles clauses are not supported in C#
            MessageWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                ViewModel.Close();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnDocumentScrollRequired(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(MessagesRichTextBox.ScrollToEnd));
        }

        private void // ERROR: Handles clauses are not supported in C#
            txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ViewModel.SendTypingNotification(string.IsNullOrEmpty(txtMessage.Text)
                    ? TextChangedAction.Cleared
                    : TextChangedAction.Changed);
                Debug.WriteLine("Text changed to : {0}", txtMessage.Text);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}