// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditIdentityPage.xaml.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core;
using JCsTools.IdentityManager;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class EditIdentityPage
    {
        public EditIdentityPage(IIdentity identity)
        {
            ViewModel = new EditIdentityViewModel(identity);

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            DisplayData();
        }

        public EditIdentityViewModel ViewModel { get; private set; }

        private void DisplayData()
        {
            txtName.Text = ViewModel.Identity.Identifier;
            txtUin.Text = ViewModel.Identity.GetAttribute(IdentityAttributes.UinAttribute);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.NavigateToSignInPage();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            string fullname;
            string uin;
            string password;

            try
            {
                fullname = txtName.Text;
                uin = txtUin.Text;
                password = txtPassword.Password;

                ViewModel.UpdateIdentity(fullname, uin, password);
                ViewModel.NavigateToSignInPage();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnDeleteIdentityClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.DeleteIdentity();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnAddNewImageClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.ImageSelector.AddImageFile();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}