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
/// This ViewModel features identity creation.
/// </summary>
namespace JCsTools.JCQ.ViewModel
{
  public class CreateIdentityViewModel
  {
    private ImageSelectorViewModel _ImageSelector;

    public CreateIdentityViewModel()
    {
      _ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);
    }

    /// <summary>
    /// Gets the ImageSelectorViewModel which allows to pick an identity image.
    /// </summary>
    public ImageSelectorViewModel ImageSelector {
      get { return _ImageSelector; }
    }

    /// <summary>
    /// Creates an Identity with the specified FullName, Uin, Password and the selected image. All four properties
    /// are mandatory.
    /// </summary>
    public void CreateIdentity(string fullname, string uin, string password)
    {
      if (string.IsNullOrEmpty(fullname))
        throw new ArgumentNullException("fullname");
      if (string.IsNullOrEmpty(uin))
        throw new ArgumentNullException("uin");
      if (string.IsNullOrEmpty(password))
        throw new ArgumentNullException("password");

      IdentityManager.Identity id;

      string avatarPath = System.IO.Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName, string.Format("{0}.[default].jpg", fullname));

      AvatarImageService.CreateAvatarImageFromFile(ImageSelector.SelectedImageFile, avatarPath);

      id = new IdentityManager.Identity(fullname);
      id.Description = uin;
      id.ImageUrl = avatarPath;
      id.SetAttribute(IdentityAttributes.UinAttribute, uin);
      id.SetAttribute(IdentityAttributes.PasswordAttribute, password);
      id.SetAttribute(IdentityAttributes.ImageOriginalFilePathAttribute, ImageSelector.SelectedImageFile);

      ApplicationService.Current.IdentityProvider.CreateIdentity(id);
    }

    /// <summary>
    /// Navigates to the SignIn page.
    /// </summary>
    public void NavigateToSignInPage()
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToSignInPage();
    }
  }

  public sealed class IdentityAttributes
  {
    private static IdentityUinAttribute _UinAttribute = new IdentityUinAttribute();
    private static IdentityPasswordAttribute _PasswordAttribute = new IdentityPasswordAttribute();
    private static IdentityImageOriginalFilePathAttribute _ImageOriginalFilePathAttribute = new IdentityImageOriginalFilePathAttribute();

    public static IdentityUinAttribute UinAttribute {
      get { return _UinAttribute; }
    }

    public static IdentityPasswordAttribute PasswordAttribute {
      get { return _PasswordAttribute; }
    }

    public static IdentityImageOriginalFilePathAttribute ImageOriginalFilePathAttribute {
      get { return _ImageOriginalFilePathAttribute; }
    }
  }

  /// <summary>
  /// Identity Attribute representing the Identity's UIN.
  /// </summary>
  public class IdentityUinAttribute : IdentityManager.IIdentityAttribute<string>
  {
    public string IdentityManager.IIdentityAttribute.AttributeName {
      get { return "Uin"; }
    }
  }

  /// <summary>
  /// Identity Attribute representing the Identity's password.
  /// </summary>
  public class IdentityPasswordAttribute : IdentityManager.IIdentityAttribute<string>
  {
    public string IdentityManager.IIdentityAttribute.AttributeName {
      get { return "Password"; }
    }
  }

  /// <summary>
  /// Identity Attribute representing the Identity's avatar origin.
  /// </summary>
  public class IdentityImageOriginalFilePathAttribute : IdentityManager.IIdentityAttribute<string>
  {
    public string IdentityManager.IIdentityAttribute.AttributeName {
      get { return "ImageOriginalFilePath"; }
    }
  }
}

