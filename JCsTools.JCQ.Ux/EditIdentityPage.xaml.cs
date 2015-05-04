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
  public partial class EditIdentityPage
  {
    private EditIdentityViewModel _ViewModel;

    public EditIdentityPage(IdentityManager.IIdentity identity)
    {
      _ViewModel = new EditIdentityViewModel(identity);

      // This call is required by the Windows Form Designer.
      InitializeComponent();

      DisplayData();
    }

    public EditIdentityViewModel ViewModel {
      get { return _ViewModel; }
    }

    private void DisplayData()
    {
      txtName.Text = ViewModel.Identity.Identifier;
      txtUin.Text = ViewModel.Identity.GetAttribute(IdentityAttributes.UinAttribute);
    }

    private void OnCancelClick(object sender, System.Windows.RoutedEventArgs e)
    {
      try {
        ViewModel.NavigateToSignInPage();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnUpdateClick(object sender, System.Windows.RoutedEventArgs e)
    {
      string fullname;
      string uin;
      string password;

      try {
        fullname = txtName.Text;
        uin = txtUin.Text;
        password = txtPassword.Password;

        ViewModel.UpdateIdentity(fullname, uin, password);
        ViewModel.NavigateToSignInPage();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnDeleteIdentityClick(object sender, RoutedEventArgs e)
    {
      try {
        ViewModel.DeleteIdentity();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnAddNewImageClick(object sender, RoutedEventArgs e)
    {
      try {
        ViewModel.ImageSelector.AddImageFile();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }
  }
}

