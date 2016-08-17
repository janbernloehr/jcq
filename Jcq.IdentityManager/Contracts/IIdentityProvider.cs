// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentityProvider.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Jcq.Core.Contracts.Collections;

namespace Jcq.IdentityManager.Contracts
{
    /// <summary>
    ///     Defines a service contract to create, delete, and store identities.
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        ///     Gets a list of identities.
        /// </summary>
        INotifyingCollection<IIdentity> Identities { get; }

        /// <summary>
        ///     Returns the identity with the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Returns the identity with the given identifier. If none exists, null is returned.</returns>
        IIdentity GetIdentityByIdentifier(string identifier);

        /// <summary>
        ///     Creates the given identity in the store.
        /// </summary>
        /// <param name="identity">The identity.</param>
        void CreateIdentity(IIdentity identity);

        /// <summary>
        ///     Deletes the given identity from the store.
        /// </summary>
        /// <param name="identity">The identity.</param>
        void DeleteIdentity(IIdentity identity);
    }
}