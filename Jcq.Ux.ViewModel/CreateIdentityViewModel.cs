// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateIdentityViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.IO;
using JCsTools.Core;
using JCsTools.IdentityManager;

namespace JCsTools.JCQ.ViewModel
{
    /// <summary>
    ///     This ViewModel features identity creation.
    /// </summary>
    public class CreateIdentityViewModel
    {
        public CreateIdentityViewModel()
        {
            ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);
        }

        /// <summary>
        ///     Gets the ImageSelectorViewModel which allows to pick an identity image.
        /// </summary>
        public ImageSelectorViewModel ImageSelector { get; private set; }

        /// <summary>
        ///     Creates an Identity with the specified FullName, Uin, Password and the selected image. All four properties
        ///     are mandatory.
        /// </summary>
        public void CreateIdentity(string fullname, string uin, string password)
        {
            if (string.IsNullOrEmpty(fullname))
                throw new ArgumentNullException("fullname");
            if (string.IsNullOrEmpty(uin))
                throw new ArgumentNullException("uin");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var avatarPath = Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName,
                string.Format("{0}.[default].jpg", fullname));

            AvatarImageService.CreateAvatarImageFromFile(ImageSelector.SelectedImageFile, avatarPath);

            var id = new Identity(fullname) {Description = uin, ImageUrl = avatarPath};

            id.SetAttribute(IdentityAttributes.UinAttribute, uin);
            id.SetAttribute(IdentityAttributes.PasswordAttribute, password);
            id.SetAttribute(IdentityAttributes.ImageOriginalFilePathAttribute, ImageSelector.SelectedImageFile);

            ApplicationService.Current.IdentityProvider.CreateIdentity(id);
        }

        /// <summary>
        ///     Navigates to the SignIn page.
        /// </summary>
        public void NavigateToSignInPage()
        {
            Kernel.Services.GetService<INavigationService>().NavigateToSignInPage();
        }
    }
}