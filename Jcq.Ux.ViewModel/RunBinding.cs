// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunBinding.cs" company="Jan-Cornelius Molnar">
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
using System.Windows.Documents;

namespace JCsTools.JCQ.ViewModel
{
    public class RunBinding : DependencyObject
    {
        protected static void OnThisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;

            var text = (string) e.NewValue;
            var run = (Run) d;

            run.Text = text;
        }

        public static string GetText(DependencyObject obj)
        {
            return (string) obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text",
            typeof (string), typeof (RunBinding), new FrameworkPropertyMetadata(null, OnThisPropertyChanged));
    }
}