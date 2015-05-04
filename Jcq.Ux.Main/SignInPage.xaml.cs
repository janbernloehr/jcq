// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignInPage.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Windows;
using System.Windows.Controls;
using JCsTools.Core;
using JCsTools.IdentityManager;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            ViewModel = new SignInPageViewModel();

            InitializeComponent();
        }

        public SignInPageViewModel ViewModel { get; private set; }

        private void OnIdentityDoubleClick(object sender, RoutedEventArgs e)
        {
            IIdentity identity;

            try
            {
                identity = (IIdentity) ViewModel.Identities.CurrentItem;

                // Check whether there is an identity selected.
                if (identity == null)
                    return;

                ViewModel.SignIn(identity);

                App.ShowStatusWindows();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnSignInClick(object sender, RoutedEventArgs e)
        {
            IIdentity identity;

            try
            {
                identity = (IIdentity) ViewModel.Identities.CurrentItem;

                // Check whether there is an identity selected.
                if (identity == null)
                    return;

                ViewModel.SignIn(identity);

                App.ShowStatusWindows();
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
            IIdentity identity;

            try
            {
                identity = (IIdentity) ViewModel.Identities.CurrentItem;

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