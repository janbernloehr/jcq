// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivacyWindow.xaml.cs" company="Jan-Cornelius Molnar">
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