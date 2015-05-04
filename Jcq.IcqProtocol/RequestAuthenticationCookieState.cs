// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestAuthenticationCookieState.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface
{
    /// <summary>
    ///     Provides the state for an authentication cookie request.
    /// </summary>
    public class RequestAuthenticationCookieState
    {
        /// <summary>
        ///     Gets or sets the Address of the Sign In Server.
        /// </summary>
        public string BosServerAddress { get; set; }

        /// <summary>
        ///     Gets or sets the Authentication Cookie.
        /// </summary>
        public List<byte> AuthCookie { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the sign in succeeded.
        /// </summary>
        public bool AuthenticationSucceeded { get; set; }

        /// <summary>
        ///     Gets or sets the authentication error.
        /// </summary>
        public AuthFailedCode AuthenticationError { get; set; }
    }
}