// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowStyleExtender.Win32.cs" company="Jan-Cornelius Molnar">
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
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace JCsTools.Wpf.Extenders
{
    public partial class WindowResizeExtender
    {
        // This Partial Class contains all API related code.
        private IntPtr _handle;
        private WindowInteropHelper _interopHelper;
        private HwndSource _source;

        protected void WindowSourceInitialized(object sender, EventArgs e)
        {
            var wnd = (Window) sender;

            // Create a helper object to gain access to the Win32 handle
            _interopHelper = new WindowInteropHelper(wnd);

            _handle = _interopHelper.Handle;
            _source = HwndSource.FromHwnd(_handle);

            // Add a hook to process window messages
            _source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var wmsg = (WindowMessage) msg;

            switch (wmsg)
            {
                case WindowMessage.WM_GETMINMAXINFO:
                    // Enable Max/Min Size support.

                    var minmax = new MinMaxInfo();
                    Matrix trans;
                    Point maxSize;
                    Point minSize;
                    Point workArea;

                    trans = _source.CompositionTarget.TransformToDevice;

                    Marshal.PtrToStructure(lParam, minmax);

                    workArea =
                        trans.Transform(new Point(SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height));

                    minmax.ptMaxSize.x = (int) workArea.X;
                    minmax.ptMaxSize.y = (int) workArea.Y;

                    maxSize = trans.Transform(new Point(_window.MaxWidth, _window.MaxHeight));

                    if (_window.MaxWidth < double.PositiveInfinity)
                        minmax.ptMaxTrackSize.x = (int) maxSize.X;
                    if (_window.MaxHeight < double.PositiveInfinity)
                        minmax.ptMaxTrackSize.y = (int) maxSize.Y;

                    minSize = trans.Transform(new Point(_window.MinWidth, _window.MinHeight));

                    if (_window.MinWidth > 0)
                        minmax.ptMinTrackSize.x = (int) minSize.X;
                    if (_window.MinHeight > 0)
                        minmax.ptMinTrackSize.y = (int) minSize.Y;

                    Marshal.StructureToPtr(minmax, lParam, true);

                    handled = true;
                    break;
                case WindowMessage.WM_SIZING:
                    var r = new Win32Rect();

                    Marshal.PtrToStructure(lParam, r);

                    handled = WmValidateSize(msg, r);

                    Marshal.StructureToPtr(r, lParam, true);

                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        private static bool WmValidateSize(int sizeMode, Win32Rect rectangle)
        {
            var cx = rectangle.right - rectangle.left;
            var cy = rectangle.bottom - rectangle.top;

            if (cx > SystemParameters.WorkArea.Width)
                rectangle.right = (int) (rectangle.left + SystemParameters.WorkArea.Width);
            if (cy > SystemParameters.WorkArea.Height)
                rectangle.bottom = (int) (rectangle.top + SystemParameters.WorkArea.Height);

            return true;
        }
    }
}