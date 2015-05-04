// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsPage.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Windows;
using System.Windows.Controls;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class ContactsPage : Page
    {
        public ContactsPage()
        {
            ViewModel = new ContactsPageViewModel();

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