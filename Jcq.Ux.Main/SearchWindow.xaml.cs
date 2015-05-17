// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class SearchWindow
    {
        //Private wse As JCsTools.Wpf.Extenders.WindowResizeExtender

        public SearchWindow()
        {
            ViewModel = new SearchWindowViewModel();

            DataContext = ViewModel;

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            //' Add any initialization after the InitializeComponent() call.
            //wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.AttachResizeExtender(Me)

            App.DefaultWindowStyle.Attach(this);
        }

        public SearchWindowViewModel ViewModel { get; private set; }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Search(SearchText.Text);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnSubMenuOpened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu;
            ContactViewModel contact;

            try
            {
                menu = (ContextMenu) e.OriginalSource;
                contact = (ContactViewModel) menu.DataContext;

                ViewModel.BuildContactContextMenu(menu, contact);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}