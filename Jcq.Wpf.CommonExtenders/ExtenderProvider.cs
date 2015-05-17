// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtenderProvider.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.Windows;

namespace JCsTools.Wpf.Extenders
{
    public class WindowExtenderProvider
    {
        private static readonly object _lock = new object();

        private static readonly Dictionary<Window, WindowResizeExtender> _ResizeExtenders =
            new Dictionary<Window, WindowResizeExtender>();

        public static WindowResizeExtender AttachResizeExtender(Window wnd)
        {
            var extender = new WindowResizeExtender();
            extender.Window = wnd;

            lock (_lock)
            {
                _ResizeExtenders.Add(wnd, extender);
            }

            wnd.Closed += OnWindowClosed;

            return extender;
        }

        private static void OnWindowClosed(object sender, EventArgs e)
        {
            var wnd = (Window) sender;

            lock (_lock)
            {
                if (_ResizeExtenders.ContainsKey(wnd))
                {
                    _ResizeExtenders.Remove(wnd);
                }
            }
        }

        public static WindowResizeExtender GetResizeExtender(Window wnd)
        {
            lock (_lock)
            {
                return _ResizeExtenders.ContainsKey(wnd) ? _ResizeExtenders[wnd] : null;
            }
        }
    }
}