// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateIcqIdentityViewModel.cs" company="Jan-Cornelius Molnar">
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
using Jcq.Core;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.ViewModel
{
    /// <summary>
    ///     This ViewModel features identity creation.
    /// </summary>
    public class CreateIcqIdentityViewModel : ViewModelBase
    {
        public CreateIcqIdentityViewModel()
        {
            ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);
        }

        /// <summary>
        ///     Gets the ImageSelectorViewModel which allows to pick an identity image.
        /// </summary>
        public ImageSelectorViewModel ImageSelector { get; }

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

            string avatarPath = Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName,
                string.Format("{0}.[default].jpg", fullname));

            AvatarImageService.CreateAvatarImageFromFile(ImageSelector.SelectedImageFile, avatarPath);

            var id = new IcqIdentity(fullname)
            {
                Description = uin,
                ImageUrl = avatarPath,
                IcqUin = uin,
                IcqPassword = password,
                ImageOriginalFilePathAttribute = ImageSelector.SelectedImageFile
            };

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