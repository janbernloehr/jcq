// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPrivacyService.cs" company="Jan-Cornelius Molnar">
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

using System.Threading.Tasks;
using Jcq.Core.Contracts.Collections;

namespace Jcq.IcqProtocol.Contracts
{
    /// <summary>
    /// Defines a service contract to obtain and change the privacy configurations.
    /// </summary>
    public interface IPrivacyService : IContextService
    {
        /// <summary>
        /// Gets a list of contacts to which the identity is always visible.
        /// </summary>
        IReadOnlyNotifyingCollection<IContact> VisibleList { get; }

        /// <summary>
        /// Gets a list of contacts to which the identity is always invisible.
        /// </summary>
        IReadOnlyNotifyingCollection<IContact> InvisibleList { get; }

        /// <summary>
        /// Gets a list of contacts which are ignored by this identity.
        /// </summary>
        IReadOnlyNotifyingCollection<IContact> IgnoreList { get; }

        /// <summary>
        /// Adds the given contact to the visible list.
        /// </summary>
        /// <param name="contact">The contact to add.</param>
        /// <returns></returns>
        Task AddContactToVisibleList(IContact contact);

        /// <summary>
        /// Removes the given contact from the visible list.
        /// </summary>
        /// <param name="contact">The contact to remove.</param>
        /// <returns></returns>
        Task RemoveContactFromVisibleList(IContact contact);

        /// <summary>
        /// Adds the given contact to the invisible list.
        /// </summary>
        /// <param name="contact">The contact to add.</param>
        /// <returns></returns>
        Task AddContactToInvisibleList(IContact contact);

        /// <summary>
        /// Removes the given contact from the invisible list.
        /// </summary>
        /// <param name="contact">The contact to remove.</param>
        /// <returns></returns>
        Task RemoveContactFromInvisibleList(IContact contact);

        /// <summary>
        /// Adds the given contact to the ignore list.
        /// </summary>
        /// <param name="contact">The contact to add.</param>
        /// <returns></returns>
        Task AddContactToIgnoreList(IContact contact);

        /// <summary>
        /// Removes the given contact from the ignore list.
        /// </summary>
        /// <param name="contact">The contact to remove.</param>
        /// <returns></returns>
        Task RemoveContactFromIgnoreList(IContact contact);
    }
}