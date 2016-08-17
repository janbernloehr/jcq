// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactContextMenuService.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Jcq.Core;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.Ux.Main.Views;
using Jcq.Ux.ViewModel;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.Main.Services
{
    public class ContactContextMenuService : ContextService, IContactContextMenuService
    {
        public ContactContextMenuService(IContext context) : base(context)
        {
        }

        public IEnumerable<MenuItem> GetMenuItems(ContactViewModel contact)
        {
            // Send text-message
            // Add to contactlist
            // Remove from contactlist
            // View history
            // View info

            MenuItem item;
            List<MenuItem> items;

            IStorageService svStorage;

            svStorage = Context.GetService<IStorageService>();

            items = new List<MenuItem>();

            item = new MenuItem {Header = "Send text-message"};

            item.Click += OnSendTextMessageClick;

            items.Add(item);

            if (svStorage.IsContactStored(contact.Model))
            {
                item = new MenuItem {Header = "Remove from contactlist"};

                item.Click += OnRemoveContactClick;

                items.Add(item);
            }
            else
            {
                item = new MenuItem {Header = "Add to contactlist"};

                item.Click += OnAddContactClick;

                items.Add(item);
            }

            if (ApplicationService.Current.Context.GetService<IContactHistoryService>().GetHistory(contact, 1).Count > 0)
            {
                item = new MenuItem {Header = "View chat history"};

                item.Click += OnViewHistoryClick;

                items.Add(item);
            }

            item = new MenuItem {Header = "View contact details"};

            item.Click += OnViewDetailsClick;

            items.Add(item);

            return items;
        }

        #region  ContextMenu Item Click Handlers 

        private void OnSendTextMessageClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe;
            ContactViewModel contact;

            try
            {
                fe = (FrameworkElement) e.OriginalSource;
                contact = (ContactViewModel) fe.DataContext;

                ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .GetMessageWindowViewModel(contact)
                    .Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnViewHistoryClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe;
            ContactViewModel contact;

            HistoryWindow wnd;

            try
            {
                fe = (FrameworkElement) e.OriginalSource;
                contact = (ContactViewModel) fe.DataContext;

                wnd = new HistoryWindow(contact);
                wnd.Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnViewDetailsClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe;
            ContactViewModel contact;

            ContactInformationWindow wnd;

            try
            {
                fe = (FrameworkElement) e.OriginalSource;
                contact = (ContactViewModel) fe.DataContext;

                wnd = new ContactInformationWindow(contact);
                wnd.Show();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnRemoveContactClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe;
            ContactViewModel contact;

            IStorageService sv;

            try
            {
                fe = (FrameworkElement) e.OriginalSource;
                contact = (ContactViewModel) fe.DataContext;

                sv = ApplicationService.Current.Context.GetService<IStorageService>();
                sv.RemoveContact(contact.Model, contact.Group.Model);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnAddContactClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe;
            ContactViewModel contact;

            IStorageService svStorage;

            try
            {
                fe = (FrameworkElement) e.OriginalSource;
                contact = (ContactViewModel) fe.DataContext;

                svStorage = ApplicationService.Current.Context.GetService<IStorageService>();
                svStorage.AddContact(contact.Model, svStorage.MasterGroup.Groups.First());
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #endregion
    }
}