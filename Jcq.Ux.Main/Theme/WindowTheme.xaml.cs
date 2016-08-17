// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowTheme.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Windows.Input;
using Jcq.Wpf.CommonExtenders;

namespace Jcq.Ux.Main.Theme
{
    public partial class WindowTheme
    {
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.DragMove();
        }

        private void OnWindowClose(object sender, RoutedEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.Close();
        }

        private void OnWindowRestore(object sender, RoutedEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.WindowState = WindowState.Normal;
        }

        private void OnWindowMaximize(object sender, RoutedEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.WindowState = WindowState.Maximized;
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.WindowState = WindowState.Minimized;
        }

        private void OnMouseDownOnGrid(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd != null)
                wnd.DragMove();
        }

        private void OnSizeNorth(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.North);
        }

        private void OnSizeSouth(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.South);
        }

        private void OnSizeEast(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.East);
        }

        private void OnSizeWest(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.West);
        }

        private void OnSizeNorthEast(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.NorthEast);
        }

        private void OnSizeNorthWest(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.NorthWest);
        }

        private void OnSizeSouthEast(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.SouthEast);
        }

        private void OnSizeSouthWest(object sender, MouseButtonEventArgs e)
        {
            var wnd = ((FrameworkElement) sender).TemplatedParent as Window;

            if (wnd == null) return;

            WindowResizeExtender wse = WindowExtenderProvider.GetResizeExtender(wnd);

            wse.DragSize(SizingAction.SouthWest);
        }
    }
}