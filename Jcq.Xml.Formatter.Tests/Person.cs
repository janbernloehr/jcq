// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace Jcq.Xml.Formatter.Tests
{
    public class Person
    {
        private readonly List<byte> _Data = new List<byte>();
        private readonly List<string> _Girlfriends = new List<string>();
        public string FirstName { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

        public List<string> Girlfriends
        {
            get { return _Girlfriends; }
        }

        public List<byte> Data
        {
            get { return _Data; }
        }
    }
}