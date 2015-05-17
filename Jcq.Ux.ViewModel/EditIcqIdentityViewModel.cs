// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditIcqIdentityViewModel.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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