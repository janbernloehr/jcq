// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowTheme.xaml.cs" company="Jan-Cornelius Molnar">
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

using System.Windows;
using System.Windows.Input;
using JCsTools.Wpf.Extenders;

namespace JCsTools.JCQ.Ux
{
    public partial class WindowTheme
    {
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Window wnd;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
                wnd.DragMove();
        }

        private void OnWindowClose(object sender, RoutedEventArgs e)
        {
            Window wnd;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.Close();
        }

        private void OnWindowRestore(object sender, RoutedEventArgs e)
        {
            Window wnd;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.WindowState = WindowState.Normal;
        }

        private void OnWindowMaximize(object sender, RoutedEventArgs e)
        {
            Window wnd;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.WindowState = WindowState.Maximized;
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            Window wnd;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.WindowState = WindowState.Minimized;
        }

        private void OnMouseDownOnGrid(object sender, MouseButtonEventArgs e)
        {
            Window wnd;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                wnd = ((FrameworkElement) sender).TemplatedParent as Window;

                if (wnd != null)
                    wnd.DragMove();
            }
        }

        private void OnSizeNorth(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.North);
            }
        }

        private void OnSizeSouth(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.South);
            }
        }

        private void OnSizeEast(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.East);
            }
        }

        private void OnSizeWest(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.West);
            }
        }

        private void OnSizeNorthEast(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.NorthEast);
            }
        }

        private void OnSizeNorthWest(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.NorthWest);
            }
        }

        private void OnSizeSouthEast(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.SouthEast);
            }
        }

        private void OnSizeSouthWest(object sender, MouseButtonEventArgs e)
        {
            Window wnd;
            WindowResizeExtender wse;

            wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
            {
                wse = WindowExtenderProvider.GetResizeExtender(wnd);

                wse.DragSize(SizingAction.SouthWest);
            }
        }
    }
}