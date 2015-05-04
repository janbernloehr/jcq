// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageColors.cs" company="Jan-Cornelius Molnar">
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

using System.Windows.Media;

namespace JCsTools.JCQ.ViewModel
{
    public class MessageColors
    {
        public static Brush IdentityColor
        {
            get { return Brushes.Red; }
        }

        public static Brush SystemColor
        {
            get { return Brushes.Gray; }
        }

        public static Brush HistoryColor
        {
            get { return Brushes.Gray; }
        }

        public static Brush Contact1Color
        {
            get { return Brushes.Blue; }
        }

        public static Brush Contact2Color
        {
            get { return Brushes.Green; }
        }

        public static Brush Contact3Color
        {
            get { return Brushes.Orange; }
        }

        public static Brush Contact4Color
        {
            get { return Brushes.Purple; }
        }
    }
}