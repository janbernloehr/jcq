// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowStyleExtender.cs" company="Jan-Cornelius Molnar">
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
using System.Windows.Input;

namespace Jcq.Wpf.CommonExtenders
{
    /// <summary>
    ///     Extends a Wpf Window to support manual resizing.
    /// </summary>
    /// <remarks>To use this functionality your application needs FullTrust privileges.</remarks>
    public partial class WindowResizeExtender
    {
        private Window _window;

        internal WindowResizeExtender()
        {
        }

        /// <summary>
        ///     Gets or sets the window object to extend.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Window Window
        {
            get { return _window; }
            set
            {
                if (_window != null)
                    _window.SourceInitialized -= WindowSourceInitialized;

                _window = value;
                _window.WindowStyle = WindowStyle.None;
                _window.ResizeMode = ResizeMode.NoResize;
                _window.SourceInitialized += WindowSourceInitialized;
            }
        }

        /// <summary>
        ///     Allows the window to be resized.
        /// </summary>
        /// <param name="sizingAction"></param>
        /// <remarks></remarks>
        public void DragSize(SizingAction sizingAction)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            Win32Methods.SendMessage(_handle, (uint) WindowMessage.WM_SYSCOMMAND,
                new IntPtr((int) SysCommand.SC_SIZE + (int) sizingAction), IntPtr.Zero);
            Win32Methods.SendMessage(_handle, 514, IntPtr.Zero, IntPtr.Zero);
        }
    }

    /// <summary>
    ///     Maps Win32 Sizing Actions
    /// </summary>
    /// <remarks></remarks>
    public enum SizingAction
    {
        North = 3,
        South = 6,
        East = 2,
        West = 1,
        NorthEast = 5,
        NorthWest = 4,
        SouthEast = 8,
        SouthWest = 7
    }
}