// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Jan-Cornelius Molnar">
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

using System.ComponentModel;
using System.Windows;
using Jcq.Ux.Main;

namespace JCsTools.JCQ.Ux
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Width = WindowSettings.Default.MainWindowWidth;
            Height = WindowSettings.Default.MainWindowHeight;
            Top = WindowSettings.Default.MainWindowTop;
            Left = WindowSettings.Default.MainWindowLeft;

            App.DefaultWindowStyle.Attach(this);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            WindowSettings.Default.MainWindowWidth = Width;
            WindowSettings.Default.MainWindowHeight = Height;
            WindowSettings.Default.MainWindowTop = Top;
            WindowSettings.Default.MainWindowLeft = Left;

            WindowSettings.Default.Save();
        }
    }
}