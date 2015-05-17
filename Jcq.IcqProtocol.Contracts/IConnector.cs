// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnector.cs" company="Jan-Cornelius Molnar">
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
using System.Threading.Tasks;

namespace Jcq.IcqProtocol.Contracts
{
    /// <summary>
    /// Defines a service contract for establishing a connection to the network.
    /// </summary>
    public interface IConnector : IContextService
    {
        /// <summary>
        /// Occurs when the sign in to the network has been completed successfully.
        /// </summary>
        event EventHandler SignInCompleted;

        /// <summary>
        /// Occurs when the sign in to the network has failed.
        /// </summary>
        event EventHandler<SignInFailedEventArgs> SignInFailed;

        /// <summary>
        /// Occurs when the connection to the network was interrupted.
        /// </summary>
        event EventHandler<DisconnectedEventArgs> Disconnected;

        /// <summary>
        /// Sign out of the network.
        /// </summary>
        void SignOut();

        /// <summary>
        /// Sign in to the network.
        /// </summary>
        /// <param name="credential">The credentials used to sign in.</param>
        /// <returns></returns>
        Task<bool> SignInAsync(ICredential credential);
    }
}