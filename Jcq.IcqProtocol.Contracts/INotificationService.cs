// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotificationService.cs" company="Jan-Cornelius Molnar">
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

using System;

namespace Jcq.IcqProtocol.Contracts
{
    /// <summary>
    /// Defines a service contract to send and receive typing notifications.
    /// </summary>
    public interface INotificationService : IContextService
    {
        /// <summary>
        /// Occurs when a TypingNotification is received.
        /// </summary>
        event EventHandler<TypingNotificationEventArgs> TypingNotification;

        /// <summary>
        /// Send a typing notification to a contact.
        /// </summary>
        /// <param name="receiver">The receiver of the typing notficiation.</param>
        /// <param name="type">The type of the typing notification.</param>
        void SendNotification(IContact receiver, NotificationType type);
    }
}