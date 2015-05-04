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
namespace JCsTools.JCQ.Ux
{
  public class WindowStateToVisibilityConverter : IValueConverter
  {
    public object System.Windows.Data.IValueConverter.Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      WindowState x = (WindowState)value;

      if ((string)parameter == "restore") {
        if (x == WindowState.Maximized) {
          return Visibility.Visible;
        } else {
          return Visibility.Collapsed;
        }
      } else {
        if (x == WindowState.Normal) {
          return Visibility.Visible;
        } else {
          return Visibility.Collapsed;
        }
      }
    }

    public object System.Windows.Data.IValueConverter.ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class WindowTheme
  {
    private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed)
        wnd.DragMove();
    }

    private void OnWindowClose(object sender, RoutedEventArgs e)
    {
      Window wnd;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null)
        wnd.Close();
    }

    private void OnWindowRestore(object sender, RoutedEventArgs e)
    {
      Window wnd;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null)
        wnd.WindowState = WindowState.Normal;
    }

    private void OnWindowMaximize(object sender, RoutedEventArgs e)
    {
      Window wnd;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null)
        wnd.WindowState = WindowState.Maximized;
    }

    private void OnWindowMinimize(object sender, RoutedEventArgs e)
    {
      Window wnd;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null)
        wnd.WindowState = WindowState.Minimized;
    }


    private void OnMouseDownOnGrid(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;

      if (Input.Mouse.LeftButton == Input.MouseButtonState.Pressed) {
        wnd = ((FrameworkElement)sender).TemplatedParent as Window;

        if (wnd != null)
          wnd.DragMove();
      }
    }

    private void OnSizeNorth(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.North);
      }
    }

    private void OnSizeSouth(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.South);
      }
    }

    private void OnSizeEast(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.East);
      }
    }

    private void OnSizeWest(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.West);
      }
    }

    private void OnSizeNorthEast(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.NorthEast);
      }
    }

    private void OnSizeNorthWest(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.NorthWest);
      }
    }

    private void OnSizeSouthEast(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.SouthEast);
      }
    }

    private void OnSizeSouthWest(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Window wnd;
      JCsTools.Wpf.Extenders.WindowResizeExtender wse;

      wnd = ((FrameworkElement)sender).TemplatedParent as Window;

      if (wnd != null) {
        wse = JCsTools.Wpf.Extenders.WindowExtenderProvider.GetResizeExtender(wnd);

        wse.DragSize(JCsTools.Wpf.Extenders.SizingAction.SouthWest);
      }
    }
  }
}

