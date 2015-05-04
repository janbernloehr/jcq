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
namespace JCsTools.JCQ.Ux
{
  public class NavigationService : Core.Service, INavigationService
  {
    public void ViewModel.INavigationService.NavigateToContactsPage()
    {
      Application.Current.MainWindow.Content = new ContactsPage();

      object w1 = new PrivacyWindow();
      w1.Show();
    }

    public void ViewModel.INavigationService.NavigateToCreateIdentityPage()
    {
      Application.Current.MainWindow.Content = new CreateIdentityPage();
    }

    public void ViewModel.INavigationService.NavigateToSignInPage()
    {
      Application.Current.MainWindow.Content = new SignInPage();
    }

    public void ViewModel.INavigationService.NavigateToEditIdentityPage(IdentityManager.IIdentity identity)
    {
      Application.Current.MainWindow.Content = new EditIdentityPage(identity);
    }

    public void ViewModel.INavigationService.NavigateToSigningInPage()
    {
      Application.Current.MainWindow.Content = new SigningInPage();
    }
  }
}

