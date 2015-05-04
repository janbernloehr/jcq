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
// Interaction logic for MessageWindow.xaml
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Ux
{
  public partial class MessageWindow : System.Windows.Window
  {
    private MessageWindowViewModel _ViewModel;

    public MessageWindow(ContactViewModel contact)
    {
      _ViewModel = new MessageWindowViewModel(contact);

      _ViewModel.ShowRequested += OnShowRequested;
      _ViewModel.ConatctInformationWindowRequested += OnConatctInformationWindowRequested;
      _ViewModel.ContactHistoryWindowRequested += OnContactHistoryWindowRequested;
      _ViewModel.DocumentScrollRequired += OnDocumentScrollRequired;

      // This call is required by the Windows Form Designer.
      InitializeComponent();

      App.DefaultWindowStyle.Attach(this);

      txtMessage.Focus();
    }

    public MessageWindowViewModel ViewModel {
      get { return _ViewModel; }
    }

    private void OnShowRequested(object sender, EventArgs e)
    {
      try {
        this.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnConatctInformationWindowRequested(object sender, EventArgs e)
    {
      ContactInformationWindow wnd;

      try {
        wnd = new ContactInformationWindow(ViewModel.Contact);
        wnd.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnContactHistoryWindowRequested(object sender, EventArgs e)
    {
      HistoryWindow wnd;

      try {
        wnd = new HistoryWindow(ViewModel.Contact);
        wnd.Show();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnShowContactInformationClick(object sender, RoutedEventArgs e)
    {
      try {
        ViewModel.RequestContactInformationWindow();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnShowChatHistoryClick(object sender, RoutedEventArgs e)
    {
      try {
        ViewModel.RequestChatHistoryWindow();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnSendMessageClick(object sender, System.Windows.RoutedEventArgs e)
    {
      string messageText;

      try {
        messageText = txtMessage.Text;

        ViewModel.SendMessage(messageText);

        this.txtMessage.Clear();
        this.txtMessage.Focus();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void  // ERROR: Handles clauses are not supported in C#
MessageWindow_Activated(object sender, System.EventArgs e)
    {
      try {
        txtMessage.Focus();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void  // ERROR: Handles clauses are not supported in C#
MessageWindow_Closed(object sender, System.EventArgs e)
    {
      try {
        ViewModel.Close();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnDocumentScrollRequired(object sender, EventArgs e)
    {
      Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, new Action(MessagesRichTextBox.ScrollToEnd));
    }

    private void  // ERROR: Handles clauses are not supported in C#
txtMessage_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
      try {
        ViewModel.SendTypingNotification(string.IsNullOrEmpty(txtMessage.Text) ? TextChangedAction.Cleared : TextChangedAction.Changed);
        Debug.WriteLine("Text changed to : {0}", txtMessage.Text);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }
  }
}

