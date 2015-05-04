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
// Interaction logic for SignInPage.xaml
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Ux
{
  public partial class SignInPage : System.Windows.Controls.Page
  {
    private SignInPageViewModel _viewModel;

    public SignInPageViewModel ViewModel {
      get { return _viewModel; }
    }

    public SignInPage()
    {
      _viewModel = new SignInPageViewModel();

      InitializeComponent();
    }

    private void OnIdentityDoubleClick(object sender, RoutedEventArgs e)
    {
      IdentityManager.IIdentity identity;

      try {
        identity = (IdentityManager.IIdentity)ViewModel.Identities.CurrentItem;

        // Check whether there is an identity selected.
        if (identity == null)
          return;

        ViewModel.SignIn(identity);

        App.ShowStatusWindows();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnSignInClick(object sender, System.Windows.RoutedEventArgs e)
    {
      IdentityManager.IIdentity identity;

      try {
        identity = (IdentityManager.IIdentity)ViewModel.Identities.CurrentItem;

        // Check whether there is an identity selected.
        if (identity == null)
          return;

        ViewModel.SignIn(identity);

        App.ShowStatusWindows();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnAddNewClick(object sender, System.Windows.RoutedEventArgs e)
    {
      try {
        ViewModel.NavigateToCreateIdentityPage();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnEditClick(object sender, RoutedEventArgs e)
    {
      IdentityManager.IIdentity identity;

      try {
        identity = (IdentityManager.IIdentity)ViewModel.Identities.CurrentItem;

        // Check whether there is an identity selected.
        if (identity == null)
          return;

        ViewModel.NavigateToEditIdentityPage(identity);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }
  }
}

