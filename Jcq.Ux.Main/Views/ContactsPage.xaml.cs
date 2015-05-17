// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsPage.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Windows;
using System.Windows.Controls;
using Jcq.Core;
using Jcq.IcqProtocol;
using Jcq.Ux.ViewModel;
using JCsTools.JCQ.Ux;

namespace Jcq.Ux.Main.Views
{
    public partial class ContactsPage : Page
    {
        public ContactsPage()
        {
            ViewModel = new ContactsPageViewModel();

            DataContext = ViewModel;

            InitializeComponent();
        }

        public ContactsPageViewModel ViewModel { get; private set; }

        private void OnSendMessageClick(object sender, RoutedEventArgs e)
        {
            ContactViewModel contact;

            try
            {
                contact = (ContactViewModel) ((FrameworkElement) e.OriginalSource).DataContext;

                ViewModel.StartChatSessionWithContact(contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnContactDoubleClick(object sender, RoutedEventArgs e)
        {
            ContactViewModel contact;

            try
            {
                contact = (ContactViewModel) ((FrameworkElement) e.OriginalSource).DataContext;

                ViewModel.StartChatSessionWithContact(contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnSubMenuOpened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu;
            ContactViewModel contact;

            try
            {
                menu = (ContextMenu) e.OriginalSource;
                contact = (ContactViewModel) menu.DataContext;

                ViewModel.CreateContextMenuForContact(menu, contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnSetStatusClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe;
            IcqStatusCode status;

            try
            {
                fe = (FrameworkElement) e.OriginalSource;
                status = (IcqStatusCode) fe.DataContext;

                ViewModel.ChangeStatus(status);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        //Private Sub OnStatusSelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
        //    Dim status As JCQ.IcqInterface.IcqStatusCode

        //    Try
        //        status = DirectCast(e.AddedItems(0), IcqInterface.IcqStatusCode)

        //        ViewModel.ChangeStatus(status)
        //    Catch ex As Exception
        //        Core.Kernel.Exceptions.PublishException(ex)
        //    End Try
        //End Sub

        private void OnAddNewContactClick(object sender, RoutedEventArgs e)
        {
            SearchWindow wnd;

            try
            {
                wnd = new SearchWindow();
                wnd.Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnDisconnectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.SignOut();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}