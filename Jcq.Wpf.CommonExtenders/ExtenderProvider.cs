// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtenderProvider.cs" company="Jan-Cornelius Molnar">
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