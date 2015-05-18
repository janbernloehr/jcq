// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationService.cs" company="Jan-Cornelius Molnar">
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

using System.Windows;
using Jcq.Core;
using Jcq.Ux.Main.Views;
using Jcq.Ux.ViewModel;
using Jcq.Ux.ViewModel.Contracts;
using JCsTools.JCQ.Ux;

namespace Jcq.Ux.Main.Services
{
    public class NavigationService : Service, INavigationService
    {
        void INavigationService.NavigateToContactsPage()
        {
            Application.Current.MainWindow.Content = new ContactsPage();

            var w1 = new PrivacyWindow();
            w1.Show();

            var w2 = new RateLimitsWindow();
            w2.Show();
        }

        void INavigationService.NavigateToCreateIdentityPage()
        {
            Application.Current.MainWindow.Content = new CreateIdentityPage();
        }

        void INavigationService.NavigateToSignInPage()
        {
            Application.Current.Dispatcher.Invoke(() => { Application.Current.MainWindow.Content = new SignInPage(); });
        }

        void INavigationService.NavigateToEditIdentityPage(IcqIdentity identity)
        {
            Application.Current.MainWindow.Content = new EditIcqIdentityPage(identity);
        }

        void INavigationService.NavigateToSigningInPage()
        {
            Application.Current.MainWindow.Content = new SigningInPage();
        }
    }
}