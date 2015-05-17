// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignInPage.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Windows;
using System.Windows.Controls;
using Jcq.Ux.ViewModel;
using JCsTools.Core;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            ViewModel = new SignInPageViewModel();

            DataContext = ViewModel;

            InitializeComponent();
        }

        public SignInPageViewModel ViewModel { get; private set; }

        private async void OnIdentityDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var identity = (IcqIdentity) ViewModel.Identities.CurrentItem;

                // Check whether there is an identity selected.
                if (identity == null)
                    return;

                var t = ViewModel.SignIn(identity);

                App.ShowStatusWindows();

                await t;
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private async void OnSignInClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var identity = (IcqIdentity) ViewModel.Identities.CurrentItem;

                // Check whether there is an identity selected.
                if (identity == null)
                    return;

                var t = ViewModel.SignIn(identity);

                App.ShowStatusWindows();

                await t;
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnAddNewClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.NavigateToCreateIdentityPage();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var identity = (IcqIdentity) ViewModel.Identities.CurrentItem;

                // Check whether there is an identity selected.
                if (identity == null)
                    return;

                ViewModel.NavigateToEditIdentityPage(identity);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}