// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditIcqIdentityViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Windows;
using Jcq.Ux.ViewModel;
using JCsTools.Core;

namespace JCsTools.JCQ.ViewModel
{
    /// <summary>
    ///     This ViewModel features identity editing.
    /// </summary>
    public class EditIcqIdentityViewModel
    {
        public EditIcqIdentityViewModel(IcqIdentity identity)
        {
            Identity = identity;

            ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);

            //TODO: Make this work.
            //if (Identity.HasAttribute(IdentityAttributes.ImageOriginalFilePathAttribute))
            //{
            //    ImageSelector.SelectedImageFile =
            //        Identity.GetAttribute(IdentityAttributes.ImageOriginalFilePathAttribute);
            //}
        }

        /// <summary>
        ///     Gets the ImageSelectorViewModel which allows to pick an identity image.
        /// </summary>
        public ImageSelectorViewModel ImageSelector { get; private set; }

        /// <summary>
        ///     Gets the Identity which is edited.
        /// </summary>
        public IcqIdentity Identity { get; private set; }

        /// <summary>
        ///     Updates the Identity with the specified FullName, Uin, Password and the selected image. FullName, Uin and Image
        ///     are mandatory.
        /// </summary>
        public void UpdateIdentity(string fullname, string uin, string password)
        {
            if (string.IsNullOrEmpty(fullname))
                throw new ArgumentNullException("fullname");
            if (string.IsNullOrEmpty(uin))
                throw new ArgumentNullException("uin");

            var avatarPath = Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName,
                string.Format("{0}.[default].jpg", fullname));

            AvatarImageService.CreateAvatarImageFromFile(ImageSelector.SelectedImageFile, avatarPath);

            Identity.Identifier = fullname;
            Identity.Description = uin;
            Identity.ImageUrl = avatarPath;
            Identity.IcqUin = uin;

            if (!string.IsNullOrEmpty(password))
                Identity.IcqPassword = password;
        }

        /// <summary>
        ///     Deletes the Identity if the user agrees.
        /// </summary>
        public void DeleteIdentity()
        {
            if (MessageBox.Show(string.Format("Do you really want to delete Identity \"{0}\"?", Identity.Identifier),
                null, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            ApplicationService.Current.IdentityProvider.DeleteIdentity(Identity);
            NavigateToSignInPage();
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