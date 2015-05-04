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
        private IntPtr _Handle;
        private WindowInteropHelper _InteropHelper;
        private HwndSource _Source;

        protected void // ERROR: Handles clauses are not supported in C#
            WindowSourceInitialized(object sender, EventArgs e)
        {
            Window wnd;

            wnd = (Window) sender;

            // Create a helper object to gain access to the Win32 handle
            _InteropHelper = new WindowInteropHelper(wnd);

            _Handle = _InteropHelper.Handle;
            _Source = HwndSource.FromHwnd(_Handle);

            // Add a hook to process window messages
            _Source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            WindowMessage wmsg;

            //TODO: process window messages here
            wmsg = (WindowMessage) msg;

            switch (wmsg)
            {
                case WindowMessage.WM_GETMINMAXINFO:
                    // Enable Max/Min Size support.

                    var minmax = new MinMaxInfo();
                    Matrix trans;
                    Point maxSize;
                    Point minSize;
                    Point workArea;

                    trans = _Source.CompositionTarget.TransformToDevice;

                    Marshal.PtrToStructure(lParam, minmax);

                    workArea =
                        trans.Transform(new Point(SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height));

                    minmax.ptMaxSize.x = (int) workArea.X;
                    minmax.ptMaxSize.y = (int) workArea.Y;

                    maxSize = trans.Transform(new Point(_Window.MaxWidth, _Window.MaxHeight));

                    if (_Window.MaxWidth < double.PositiveInfinity)
                        minmax.ptMaxTrackSize.x = (int) maxSize.X;
                    if (_Window.MaxHeight < double.PositiveInfinity)
                        minmax.ptMaxTrackSize.y = (int) maxSize.Y;

                    minSize = trans.Transform(new Point(_Window.MinWidth, _Window.MinHeight));

                    if (_Window.MinWidth > 0)
                        minmax.ptMinTrackSize.x = (int) minSize.X;
                    if (_Window.MinHeight > 0)
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

        private bool WmValidateSize(int sizeMode, Win32Rect rectangle)
        {
            int cx;
            int cy;

            cx = rectangle.right - rectangle.left;
            cy = rectangle.bottom - rectangle.top;

            if (cx > SystemParameters.WorkArea.Width)
                rectangle.right = (int) (rectangle.left + SystemParameters.WorkArea.Width);
            if (cy > SystemParameters.WorkArea.Height)
                rectangle.bottom = (int) (rectangle.top + SystemParameters.WorkArea.Height);

            return true;
        }
    }
}