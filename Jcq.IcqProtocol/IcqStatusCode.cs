// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqStatusCode.cs" company="Jan-Cornelius Molnar">
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

using System.Collections;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqStatusCode : IStatusCode
    {
        private readonly int _sortIndex;

        public IcqStatusCode(string name, UserStatus status, int sortIndex)
        {
            DisplayName = name;
            UserStatus = status;
            _sortIndex = sortIndex;
        }

        public UserStatus UserStatus { get; private set; }
        public string DisplayName { get; private set; }

        public int CompareTo(object obj)
        {
            var x = obj as IcqStatusCode;

            return x != null ? Comparer.Default.Compare(_sortIndex, x._sortIndex) : 0;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}