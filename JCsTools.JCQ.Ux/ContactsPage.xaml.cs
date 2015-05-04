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
// Interaction logic for Contacts.xaml
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Ux
{
  public partial class ContactsPage : System.Windows.Controls.Page
  {
    private ContactsPageViewModel _ViewModel;

    public ContactsPageViewModel ViewModel {
      get { return _ViewModel; }
    }

    public ContactsPage()
    {
      _ViewModel = new ContactsPageViewModel();

      InitializeComponent();
    }

    private void OnSendMessageClick(object sender, RoutedEventArgs e)
    {
      ContactViewModel contact;

      try {
        contact = (ContactViewModel)((FrameworkElement)e.OriginalSource).DataContext;

        ViewModel.StartChatSessionWithContact(contact);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnContactDoubleClick(object sender, RoutedEventArgs e)
    {
      ContactViewModel contact;

      try {
        contact = (ContactViewModel)((FrameworkElement)e.OriginalSource).DataContext;

        ViewModel.StartChatSessionWithContact(contact);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnSubMenuOpened(object sender, RoutedEventArgs e)
    {
      ContextMenu menu;
      ContactViewModel contact;

      try {
        menu = (ContextMenu)e.OriginalSource;
        contact = (ContactViewModel)menu.DataContext;

        ViewModel.CreateContextMenuForContact(menu, contact);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnSetStatusClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe;
      IcqInterface.IcqStatusCode status;

      try {
        fe = (FrameworkElement)e.OriginalSource;
        status = (IcqInterface.IcqStatusCode)fe.DataContext;

        ViewModel.ChangeStatus(status);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
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

      try {
        wnd = new SearchWindow();
        wnd.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnDisconnectClick(object sender, RoutedEventArgs e)
    {
      try {
        ViewModel.SignOut();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }
  }

  public class StatusToImageConverter : System.Windows.Data.IValueConverter
  {
    public object System.Windows.Data.IValueConverter.Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      IcqInterface.IcqStatusCode status = value as IcqInterface.IcqStatusCode;
      if (status == null)
        return null;

      if (object.ReferenceEquals(status, IcqInterface.IcqStatusCodes.Online)) {
        return App.Current.Resources("vbrOnline");
      } else if (object.ReferenceEquals(status, IcqInterface.IcqStatusCodes.Offline)) {
        return App.Current.Resources("vbrOffline");
      } else {
        return App.Current.Resources("vbrAway");
      }
    }

    public object System.Windows.Data.IValueConverter.ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Debug.Fail("Unexpected call of ConvertBack");
      return null;
    }
  }
}

