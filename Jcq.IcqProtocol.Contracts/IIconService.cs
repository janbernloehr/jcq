// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAvatarService.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.Contracts
{
    /// <summary>
    ///     Defines a service contract to request a users's contact icon and upload an own contact icon.
    /// </summary>
    public interface IIconService : IContextService
    {
        /// <summary>
        ///     Requests a download of the ContactIcon of the given contact.
        /// </summary>
        /// <param name="contact">The contact whose icon should be downloaded.</param>
        void RequestContactIcon(IContact contact);

        /// <summary>
        ///     Uploads a new icon for the IContextService.Identity.
        /// </summary>
        /// <param name="icon">A byte representation of the icon to upload.</param>
        void UploadIcon(byte[] icon);
    }
}