// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowStyleExtender.Win32.cs" company="Jan-Cornelius Molnar">
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
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Jcq.Wpf.CommonExtenders
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
            if (_source != null) _source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var wmsg = (WindowMessage) msg;

            switch (wmsg)
            {
                case WindowMessage.WM_GETMINMAXINFO:
                    // Enable Max/Min Size support.

                    var minmax = new MinMaxInfo();

                    if (_source == null)
                    {
                        handled = true;
                        break;
                    }

                    Matrix trans = _source.CompositionTarget.TransformToDevice;

                    Marshal.PtrToStructure(lParam, minmax);

                    Point workArea =
                        trans.Transform(new Point(SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height));

                    minmax.ptMaxSize.x = (int) workArea.X;
                    minmax.ptMaxSize.y = (int) workArea.Y;

                    Point maxSize = trans.Transform(new Point(_window.MaxWidth, _window.MaxHeight));

                    if (_window.MaxWidth < double.PositiveInfinity)
                        minmax.ptMaxTrackSize.x = (int) maxSize.X;
                    if (_window.MaxHeight < double.PositiveInfinity)
                        minmax.ptMaxTrackSize.y = (int) maxSize.Y;

                    Point minSize = trans.Transform(new Point(_window.MinWidth, _window.MinHeight));

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
            int cx = rectangle.right - rectangle.left;
            int cy = rectangle.bottom - rectangle.top;

            if (cx > SystemParameters.WorkArea.Width)
                rectangle.right = (int) (rectangle.left + SystemParameters.WorkArea.Width);
            if (cy > SystemParameters.WorkArea.Height)
                rectangle.bottom = (int) (rectangle.top + SystemParameters.WorkArea.Height);

            return true;
        }
    }
}