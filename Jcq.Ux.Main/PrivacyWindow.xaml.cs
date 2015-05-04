// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivacyWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using JCsTools.Core;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class PrivacyWindow
    {
        public PrivacyWindow()
        {
            ViewModel = new PrivacyWindowViewModel();

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            App.DefaultWindowStyle.Attach(this);
        }

        public PrivacyWindowViewModel ViewModel { get; private set; }

        protected void OnAddVisibleContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var identifier = NewVisibleContact.Text;

                ViewModel.AddVisibleContact(identifier);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnRemoveVisibleContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var view = CollectionViewSource.GetDefaultView(ViewModel.VisibleContacts);
                var contact = (ContactViewModel) view.CurrentItem;

                ViewModel.RemoveVisibleContact(contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnAddInvisibleContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var identifier = NewInvisibleContact.Text;

                ViewModel.AddInvisibleContact(identifier);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnRemoveInvisibleContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var view = CollectionViewSource.GetDefaultView(ViewModel.InvisibleContacts);
                var contact = (ContactViewModel) view.CurrentItem;

                ViewModel.RemoveInvisibleContact(contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnAddIgnoreContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var identifier = NewIgnoreContact.Text;

                ViewModel.AddIgnoreContact(identifier);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnRemoveIgnoreContactClick(object sender, RoutedEventArgs e)
        {
            ICollectionView view;
            ContactViewModel contact;

            try
            {
                view = CollectionViewSource.GetDefaultView(ViewModel.IgnoredContacts);
                contact = (ContactViewModel) view.CurrentItem;

                ViewModel.RemoveIgnoreContact(contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}