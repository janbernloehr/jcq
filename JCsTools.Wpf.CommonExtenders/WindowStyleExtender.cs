//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Interop;
/// <summary>
/// Extends a Wpf Window to support manual resizing.
/// </summary>
/// <remarks>To use this functionality your application needs FullTrust privileges.</remarks>
namespace JCsTools.Wpf.Extenders
{
  public class WindowResizeExtender
  {
    private Window _Window;

    internal WindowResizeExtender()
    {

    }

    /// <summary>
    /// Gets or sets the window object to extend.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public Window Window {
      get { return _Window; }
      set {
        _Window = value;
        _Window.WindowStyle = WindowStyle.None;
        _Window.ResizeMode = ResizeMode.NoResize;
      }
    }

    /// <summary>
    /// Allows the window to be resized.
    /// </summary>
    /// <param name="sizingAction"></param>
    /// <remarks></remarks>
    public void DragSize(SizingAction sizingAction)
    {
      if (System.Windows.Input.Mouse.LeftButton == Input.MouseButtonState.Pressed) {
        Win32Methods.SendMessage(_Handle, WindowMessage.WM_SYSCOMMAND, new IntPtr(SysCommand.SC_SIZE + sizingAction), IntPtr.Zero);
        Win32Methods.SendMessage(_Handle, 514, IntPtr.Zero, IntPtr.Zero);
      }
    }
  }

  /// <summary>
  /// Maps Win32 Sizing Actions
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

