// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchWindow.xaml.cs" company="Jan-Cornelius Molnar">
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