// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqClientCapabilities.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public static class IcqClientCapabilities
    {
        private static readonly Guid _IcqFlag = new Guid("09461349-4C7F-11D1-8222-444553540000");
        private static readonly Guid _IcqRouteFinder = new Guid("09461344-4C7F-11D1-8222-444553540000");
        private static readonly Guid _RtfMessages = new Guid("97B12751-243C-4334-AD22-D6ABF73F1492");

        public static Guid IcqFlag
        {
            get { return _IcqFlag; }
        }

        public static Guid IcqRouteFinder
        {
            get { return _IcqRouteFinder; }
        }

        public static Guid RtfMessages
        {
            get { return _RtfMessages; }
        }
    }
}