// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImplementationNotFoundException.cs" company="Jan-Cornelius Molnar">
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
using System.Globalization;
using System.Runtime.Serialization;
using Jcq.Core.Contracts;

namespace JCsTools.Core.Interfaces
{
    /// <summary>
    ///     The exception that is thrown when an attempt to find an implementation for a contract which does not exists fails.
    /// </summary>
    [Serializable]
    public class ImplementationNotFoundException : ArgumentException
    {
        public ImplementationNotFoundException() : base(Strings.ImplementationNotFound_Generic)
        {
        }

        public ImplementationNotFoundException(string message) : base(message)
        {
        }

        public ImplementationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ImplementationNotFoundException(Type typeInterface)
            : base(
                string.Format(CultureInfo.InvariantCulture, Strings.ImplementationNotFound_WithTypeName,
                    typeInterface.FullName))
        {
        }

        public ImplementationNotFoundException(Type typeInterface, Exception innerException)
            : base(
                string.Format(CultureInfo.InvariantCulture, Strings.ImplementationNotFound_WithTypeName,
                    typeInterface.FullName), innerException)
        {
        }

        protected ImplementationNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}