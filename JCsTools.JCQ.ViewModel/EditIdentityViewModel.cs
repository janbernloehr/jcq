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
using System.IO;
/// <summary>
/// This ViewModel features identity editing.
/// </summary>
namespace JCsTools.JCQ.ViewModel
{
  public class EditIdentityViewModel
  {
    private ImageSelectorViewModel _ImageSelector;
    private IdentityManager.IIdentity _Identity;

    public EditIdentityViewModel(IdentityManager.IIdentity identity)
    {
      _Identity = identity;

      _ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);

      if (_Identity.HasAttribute(IdentityAttributes.ImageOriginalFilePathAttribute)) {
        _ImageSelector.SelectedImageFile = _Identity.GetAttribute(IdentityAttributes.ImageOriginalFilePathAttribute);
      }
    }

    /// <summary>
    /// Gets the ImageSelectorViewModel which allows to pick an identity image.
    /// </summary>
    public ImageSelectorViewModel ImageSelector {
      get { return _ImageSelector; }
    }

    /// <summary>
    /// Gets the Identity which is edited.
    /// </summary>
    public IdentityManager.IIdentity Identity {
      get { return _Identity; }
    }

    /// <summary>
    /// Updates the Identity with the specified FullName, Uin, Password and the selected image. FullName, Uin and Image
    /// are mandatory.
    /// </summary>
    public void UpdateIdentity(string fullname, string uin, string password)
    {
      if (string.IsNullOrEmpty(fullname))
        throw new ArgumentNullException("fullname");
      if (string.IsNullOrEmpty(uin))
        throw new ArgumentNullException("uin");

      string avatarPath = System.IO.Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName, string.Format("{0}.[default].jpg", fullname));

      AvatarImageService.CreateAvatarImageFromFile(ImageSelector.SelectedImageFile, avatarPath);

      Identity.Identifier = fullname;
      Identity.Description = uin;
      Identity.ImageUrl = avatarPath;
      Identity.SetAttribute(IdentityAttributes.UinAttribute, uin);

      if (!string.IsNullOrEmpty(password))
        Identity.SetAttribute(IdentityAttributes.PasswordAttribute, password);
    }

    /// <summary>
    /// Deletes the Identity if the user agrees.
    /// </summary>
    public void DeleteIdentity()
    {
      if (MessageBox.Show(string.Format("Do you really want to delete Identity \"{0}\"?", Identity.Identifier), null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
        ApplicationService.Current.IdentityProvider.DeleteIdentity(Identity);
        NavigateToSignInPage();
      }
    }

    /// <summary>
    /// Navigates to the SignIn page.
    /// </summary>
    public void NavigateToSignInPage()
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToSignInPage();
    }
  }
}

