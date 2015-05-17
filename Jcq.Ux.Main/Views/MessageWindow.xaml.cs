// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Jcq.Core;
using Jcq.Ux.ViewModel;
using JCsTools.JCQ.Ux;

namespace Jcq.Ux.Main.Views
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