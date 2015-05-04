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
using JCsTools.JCQ.ViewModel;
namespace JCsTools.JCQ.Ux
{
  public class ContactContextMenuService : IcqInterface.ContextService, IContactContextMenuService
  {
    public ContactContextMenuService(IcqInterface.Interfaces.IContext context) : base(context)
    {
    }

    public IEnumerable<MenuItem> IContactContextMenuService.GetContextMenuItems(ContactViewModel contact)
    {
      // Send text-message
      // Add to contactlist
      // Remove from contactlist
      // View history
      // View info

      MenuItem item;
      List<MenuItem> items;

      IcqInterface.Interfaces.IStorageService svStorage;

      svStorage = Context.GetService<IcqInterface.Interfaces.IStorageService>();

      items = new List<MenuItem>();

      item = new MenuItem();
      item.Header = "Send text-message";

      item.Click += OnSendTextmessageClick;

      items.Add(item);

      if (svStorage.IsContactStored(contact.Model)) {
        item = new MenuItem();
        item.Header = "Remove from contactlist";

        item.Click += OnRemoveContactClick;

        items.Add(item);
      } else {
        item = new MenuItem();
        item.Header = "Add to contactlist";

        item.Click += OnAddContactClick;

        items.Add(item);
      }

      if (ApplicationService.Current.Context.GetService<IContactHistoryService>.GetHistory(contact, 1).Count > 0) {
        item = new MenuItem();
        item.Header = "View chat history";

        item.Click += OnViewHistoryClick;

        items.Add(item);
      }

      item = new MenuItem();
      item.Header = "View contact details";

      item.Click += OnViewDetailsClick;

      items.Add(item);

      return items;
    }

#region  ContextMenu Item Click Handlers 

    private void OnSendTextMessageClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe;
      ContactViewModel contact;

      try {
        fe = (FrameworkElement)e.OriginalSource;
        contact = (ContactViewModel)fe.DataContext;

        ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.GetMessageWindowViewModel(contact).Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnViewHistoryClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe;
      ContactViewModel contact;

      HistoryWindow wnd;

      try {
        fe = (FrameworkElement)e.OriginalSource;
        contact = (ContactViewModel)fe.DataContext;

        wnd = new HistoryWindow(contact);
        wnd.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnViewDetailsClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe;
      ContactViewModel contact;

      ContactInformationWindow wnd;

      try {
        fe = (FrameworkElement)e.OriginalSource;
        contact = (ContactViewModel)fe.DataContext;

        wnd = new ContactInformationWindow(contact);
        wnd.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnRemoveContactClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe;
      ContactViewModel contact;

      IcqInterface.Interfaces.IStorageService sv;

      try {
        fe = (FrameworkElement)e.OriginalSource;
        contact = (ContactViewModel)fe.DataContext;

        sv = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();
        sv.RemoveContact(contact.Model, contact.Group.Model);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnAddContactClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe;
      ContactViewModel contact;

      IcqInterface.Interfaces.IStorageService svStorage;

      try {
        fe = (FrameworkElement)e.OriginalSource;
        contact = (ContactViewModel)fe.DataContext;

        svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();
        svStorage.AddContact(contact.Model, svStorage.MasterGroup.Groups(0));
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

#endregion

  }
}

