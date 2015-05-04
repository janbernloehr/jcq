// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowStyle.cs" company="Jan-Cornelius Molnar">
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

using System.Windows;
using JCsTools.Wpf.Extenders;

namespace JCsTools.JCQ.Ux
{
    public class WindowStyle
    {
        public WindowStyle()
        {
            Opacity = 1;
        }

        public bool AllowTransparency { get; set; }
        public double Opacity { get; set; }

        public void Attach(Window wnd)
        {
            wnd.Opacity = Opacity;
            wnd.AllowsTransparency = AllowTransparency;
            wnd.WindowStyle = System.Windows.WindowStyle.None;
            wnd.Style = (Style) Application.Current.FindResource("SimpleWindow");

            object extender = WindowExtenderProvider.AttachResizeExtender(wnd);

            wnd.Resources["WindowExtender"] = extender;

            //Style="{StaticResource SimpleWindow}" WindowStyle="None"
        }
    }
}