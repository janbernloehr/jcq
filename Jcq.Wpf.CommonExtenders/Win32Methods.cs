// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Win32Methods.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.Wpf.CommonExtenders
{
    internal class Win32Methods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class MinMaxInfo
    {
        internal Win32Point ptMaxPosition;
        internal Win32Point ptMaxSize;
        internal Win32Point ptMaxTrackSize;
        internal Win32Point ptMinTrackSize;
        internal Win32Point ptReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class Win32Point
    {
        // Fields
        internal int x;
        internal int y;

        internal Win32Point()
        {
        }

        internal Win32Point(int xval, int yval)
        {
            x = xval;
            y = yval;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class Win32Rect
    {
        internal int bottom;
        // Fields

        internal int left;
        internal int right;
        internal int top;
    }

    internal enum WindowMessage : uint
    {
        WM_NULL = 0x0,
        WM_CREATE = 0x1,
        WM_DESTROY = 0x2,
        WM_MOVE = 0x3,
        WM_SIZE = 0x5,
        WM_ACTIVATE = 0x6,
        WM_SETFOCUS = 0x7,
        WM_KILLFOCUS = 0x8,
        WM_ENABLE = 0xa,
        WM_SETREDRAW = 0xb,
        WM_SETTEXT = 0xc,
        WM_GETTEXT = 0xd,
        WM_GETTEXTLENGTH = 0xe,
        WM_PAINT = 0xf,
        WM_CLOSE = 0x10,
        WM_QUERYENDSESSION = 0x11,
        WM_QUERYOPEN = 0x13,
        WM_ENDSESSION = 0x16,
        WM_QUIT = 0x12,
        WM_ERASEBKGND = 0x14,
        WM_SYSCOLORCHANGE = 0x15,
        WM_SHOWWINDOW = 0x18,
        WM_WININICHANGE = 0x1a,
        WM_DEVMODECHANGE = 0x1b,
        WM_ACTIVATEAPP = 0x1c,
        WM_FONTCHANGE = 0x1d,
        WM_TIMECHANGE = 0x1e,
        WM_CANCELMODE = 0x1f,
        WM_SETCURSOR = 0x20,
        WM_MOUSEACTIVATE = 0x21,
        WM_CHILDACTIVATE = 0x22,
        WM_QUEUESYNC = 0x23,
        WM_GETMINMAXINFO = 0x24,
        WM_PAINTICON = 0x26,
        WM_ICONERASEBKGND = 0x27,
        WM_NEXTDLGCTL = 0x28,
        WM_SPOOLERSTATUS = 0x2a,
        WM_DRAWITEM = 0x2b,
        WM_MEASUREITEM = 0x2c,
        WM_DELETEITEM = 0x2d,
        WM_VKEYTOITEM = 0x2e,
        WM_CHARTOITEM = 0x2f,
        WM_SETFONT = 0x30,
        WM_GETFONT = 0x31,
        WM_SETHOTKEY = 0x32,
        WM_GETHOTKEY = 0x33,
        WM_QUERYDRAGICON = 0x37,
        WM_COMPAREITEM = 0x39,
        WM_GETOBJECT = 0x3d,
        WM_COMPACTING = 0x41,
        WM_COMMNOTIFY = 0x44,
        WM_WINDOWPOSCHANGING = 0x46,
        WM_WINDOWPOSCHANGED = 0x47,
        WM_POWER = 0x48,
        WM_COPYDATA = 0x4a,
        WM_CANCELJOURNAL = 0x4b,
        WM_NOTIFY = 0x4e,
        WM_INPUTLANGCHANGEREQUEST = 0x50,
        WM_INPUTLANGCHANGE = 0x51,
        WM_TCARD = 0x52,
        WM_HELP = 0x53,
        WM_USERCHANGED = 0x54,
        WM_NOTIFYFORMAT = 0x55,
        WM_CONTEXTMENU = 0x7b,
        WM_STYLECHANGING = 0x7c,
        WM_STYLECHANGED = 0x7d,
        WM_DISPLAYCHANGE = 0x7e,
        WM_GETICON = 0x7f,
        WM_SETICON = 0x80,
        WM_NCCREATE = 0x81,
        WM_NCDESTROY = 0x82,
        WM_NCCALCSIZE = 0x83,
        WM_NCHITTEST = 0x84,
        WM_NCPAINT = 0x85,
        WM_NCACTIVATE = 0x86,
        WM_GETDLGCODE = 0x87,
        WM_SYNCPAINT = 0x88,
        WM_NCMOUSEMOVE = 0xa0,
        WM_NCLBUTTONDOWN = 0xa1,
        WM_NCLBUTTONUP = 0xa2,
        WM_NCLBUTTONDBLCLK = 0xa3,
        WM_NCRBUTTONDOWN = 0xa4,
        WM_NCRBUTTONUP = 0xa5,
        WM_NCRBUTTONDBLCLK = 0xa6,
        WM_NCMBUTTONDOWN = 0xa7,
        WM_NCMBUTTONUP = 0xa8,
        WM_NCMBUTTONDBLCLK = 0xa9,
        WM_NCXBUTTONDOWN = 0xab,
        WM_NCXBUTTONUP = 0xac,
        WM_NCXBUTTONDBLCLK = 0xad,
        WM_INPUT_DEVICE_CHANGE = 0xfe,
        WM_INPUT = 0xff,
        WM_KEYFIRST = 0x100,
        WM_KEYDOWN = 0x100,
        WM_KEYUP = 0x101,
        WM_CHAR = 0x102,
        WM_DEADCHAR = 0x103,
        WM_SYSKEYDOWN = 0x104,
        WM_SYSKEYUP = 0x105,
        WM_SYSCHAR = 0x106,
        WM_SYSDEADCHAR = 0x107,
        WM_UNICHAR = 0x109,
        WM_KEYLAST = 0x109,
        WM_IME_STARTCOMPOSITION = 0x10d,
        WM_IME_ENDCOMPOSITION = 0x10e,
        WM_IME_COMPOSITION = 0x10f,
        WM_IME_KEYLAST = 0x10f,
        WM_INITDIALOG = 0x110,
        WM_COMMAND = 0x111,
        WM_SYSCOMMAND = 0x112,
        WM_TIMER = 0x113,
        WM_HSCROLL = 0x114,
        WM_VSCROLL = 0x115,
        WM_INITMENU = 0x116,
        WM_INITMENUPOPUP = 0x117,
        WM_MENUSELECT = 0x11f,
        WM_MENUCHAR = 0x120,
        WM_ENTERIDLE = 0x121,
        WM_MENURBUTTONUP = 0x122,
        WM_MENUDRAG = 0x123,
        WM_MENUGETOBJECT = 0x124,
        WM_UNINITMENUPOPUP = 0x125,
        WM_MENUCOMMAND = 0x126,
        WM_CHANGEUISTATE = 0x127,
        WM_UPDATEUISTATE = 0x128,
        WM_QUERYUISTATE = 0x129,
        WM_CTLCOLORMSGBOX = 0x132,
        WM_CTLCOLOREDIT = 0x133,
        WM_CTLCOLORLISTBOX = 0x134,
        WM_CTLCOLORBTN = 0x135,
        WM_CTLCOLORDLG = 0x136,
        WM_CTLCOLORSCROLLBAR = 0x137,
        WM_CTLCOLORSTATIC = 0x138,
        MN_GETHMENU = 0x1e1,
        WM_MOUSEFIRST = 0x200,
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_LBUTTONDBLCLK = 0x203,
        WM_RBUTTONDOWN = 0x204,
        WM_RBUTTONUP = 0x205,
        WM_RBUTTONDBLCLK = 0x206,
        WM_MBUTTONDOWN = 0x207,
        WM_MBUTTONUP = 0x208,
        WM_MBUTTONDBLCLK = 0x209,
        WM_MOUSEWHEEL = 0x20a,
        WM_XBUTTONDOWN = 0x20b,
        WM_XBUTTONUP = 0x20c,
        WM_XBUTTONDBLCLK = 0x20d,
        WM_MOUSELAST = 0x20d,
        WM_PARENTNOTIFY = 0x210,
        WM_ENTERMENULOOP = 0x211,
        WM_EXITMENULOOP = 0x212,
        WM_NEXTMENU = 0x213,
        WM_SIZING = 0x214,
        WM_CAPTURECHANGED = 0x215,
        WM_MOVING = 0x216,
        WM_POWERBROADCAST = 0x218,
        WM_DEVICECHANGE = 0x219,
        WM_MDICREATE = 0x220,
        WM_MDIDESTROY = 0x221,
        WM_MDIACTIVATE = 0x222,
        WM_MDIRESTORE = 0x223,
        WM_MDINEXT = 0x224,
        WM_MDIMAXIMIZE = 0x225,
        WM_MDITILE = 0x226,
        WM_MDICASCADE = 0x227,
        WM_MDIICONARRANGE = 0x228,
        WM_MDIGETACTIVE = 0x229,
        WM_MDISETMENU = 0x230,
        WM_ENTERSIZEMOVE = 0x231,
        WM_EXITSIZEMOVE = 0x232,
        WM_DROPFILES = 0x233,
        WM_MDIREFRESHMENU = 0x234,
        WM_IME_SETCONTEXT = 0x281,
        WM_IME_NOTIFY = 0x282,
        WM_IME_CONTROL = 0x283,
        WM_IME_COMPOSITIONFULL = 0x284,
        WM_IME_SELECT = 0x285,
        WM_IME_CHAR = 0x286,
        WM_IME_REQUEST = 0x288,
        WM_IME_KEYDOWN = 0x290,
        WM_IME_KEYUP = 0x291,
        WM_MOUSEHOVER = 0x2a1,
        WM_MOUSELEAVE = 0x2a3,
        WM_NCMOUSEHOVER = 0x2a0,
        WM_NCMOUSELEAVE = 0x2a2,
        WM_WTSSESSION_CHANGE = 0x2b1,
        WM_TABLET_FIRST = 0x2c0,
        WM_TABLET_LAST = 0x2df,
        WM_CUT = 0x300,
        WM_COPY = 0x301,
        WM_PASTE = 0x302,
        WM_CLEAR = 0x303,
        WM_UNDO = 0x304,
        WM_RENDERFORMAT = 0x305,
        WM_RENDERALLFORMATS = 0x306,
        WM_DESTROYCLIPBOARD = 0x307,
        WM_DRAWCLIPBOARD = 0x308,
        WM_PAINTCLIPBOARD = 0x309,
        WM_VSCROLLCLIPBOARD = 0x30a,
        WM_SIZECLIPBOARD = 0x30b,
        WM_ASKCBFORMATNAME = 0x30c,
        WM_CHANGECBCHAIN = 0x30d,
        WM_HSCROLLCLIPBOARD = 0x30e,
        WM_QUERYNEWPALETTE = 0x30f,
        WM_PALETTEISCHANGING = 0x310,
        WM_PALETTECHANGED = 0x311,
        WM_HOTKEY = 0x312,
        WM_PRINT = 0x317,
        WM_PRINTCLIENT = 0x318,
        WM_APPCOMMAND = 0x319,
        WM_THEMECHANGED = 0x31a,
        WM_CLIPBOARDUPDATE = 0x31d,
        WM_DWMCOMPOSITIONCHANGED = 0x31e,
        WM_DWMNCRENDERINGCHANGED = 0x31f,
        WM_DWMCOLORIZATIONCOLORCHANGED = 0x320,
        WM_DWMWINDOWMAXIMIZEDCHANGE = 0x321,
        WM_HANDHELDFIRST = 0x358,
        WM_HANDHELDLAST = 0x35f,
        WM_AFXFIRST = 0x360,
        WM_AFXLAST = 0x37f,
        WM_PENWINFIRST = 0x380,
        WM_PENWINLAST = 0x38f,
        WM_APP = 0x8000
    }

    internal enum SysCommand : uint
    {
        SC_SIZE = 0xf000,
        SC_MOVE = 0xf010,
        SC_MINIMIZE = 0xf020,
        SC_MAXIMIZE = 0xf030,
        SC_NEXTWINDOW = 0xf040,
        SC_PREVWINDOW = 0xf050,
        SC_CLOSE = 0xf060,
        SC_VSCROLL = 0xf070,
        SC_HSCROLL = 0xf080,
        SC_MOUSEMENU = 0xf090,
        SC_KEYMENU = 0xf100,
        SC_ARRANGE = 0xf110,
        SC_RESTORE = 0xf120,
        SC_TASKLIST = 0xf130,
        SC_SCREENSAVE = 0xf140,
        SC_HOTKEY = 0xf150,
        SC_DEFAULT = 0xf160,
        SC_MONITORPOWER = 0xf170,
        SC_CONTEXTHELP = 0xf180,
        SC_SEPARATOR = 0xf00f
    }

    internal enum Win32WindowStyle : uint
    {
        WS_POPUP = 0x80000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_OVERLAPPED = 0x0,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x8000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_MAXIMIZE = 0x1000000,
        WS_CAPTION = 0x0,
        WS_BORDER = 0x800000,
        WS_DLGFRAME = 0x400000,
        WS_VSCROLL = 0x200000,
        WS_HSCROLL = 0x100000,
        WS_SYSMENU = 0x80000,
        WS_THICKFRAME = 0x40000,
        WS_GROUP = 0x20000,
        WS_TABSTOP = 0x10000,
        WS_MINIMIZEBOX = 0x20000,
        WS_MAXIMIZEBOX = 0x10000
    }
}