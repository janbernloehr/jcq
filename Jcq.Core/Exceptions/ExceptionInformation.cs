// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionInformation.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core.Interfaces.Exceptions;

namespace JCsTools.Core.Exceptions
{
    public class ExceptionInformation : IExceptionInformation
    {
        public ExceptionInformation(Exception ex)
        {
            Exception = ex;
        }

        public ExceptionInformation(Exception ex, bool handled, bool displayed) : this(ex)
        {
            Displayed = displayed;
            Handled = handled;
        }

        public Exception Exception { get; private set; }
        public bool Displayed { get; set; }
        public bool Handled { get; set; }
    }
}