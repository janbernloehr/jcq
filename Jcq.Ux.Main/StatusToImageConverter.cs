// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusToImageConverter.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using JCsTools.JCQ.IcqInterface;

namespace JCsTools.JCQ.Ux
{
    public class StatusToImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as IcqStatusCode;
            if (status == null)
                return null;

            if (ReferenceEquals(status, IcqStatusCodes.Online))
            {
                return Application.Current.Resources["vbrOnline"];
            }
            if (ReferenceEquals(status, IcqStatusCodes.Offline))
            {
                return Application.Current.Resources["vbrOffline"];
            }
            return Application.Current.Resources["vbrAway"];
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Fail("Unexpected call of ConvertBack");
            return null;
        }
    }
}