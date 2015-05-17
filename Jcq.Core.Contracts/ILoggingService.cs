// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoggingService.cs" company="Jan-Cornelius Molnar">
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

using System.Diagnostics;

namespace Jcq.Core.Contracts
{
    /// <summary>
    /// Defines a contract for a logging service.
    /// </summary>
    public interface ILoggingService : IService
    {
        /// <summary>
        /// Adds the given log entry to the log.
        /// </summary>
        /// <param name="category">The category of the entry.</param>
        /// <param name="severity">The severity of the entry.</param>
        /// <param name="message">The message of the entry.</param>
        void Log(string category, TraceEventType severity, string message);

        /// <summary>
        /// Adds the given log entry to the log.
        /// </summary>
        /// <param name="category">The category of the entry.</param>
        /// <param name="severity">The severity of the entry.</param>
        /// <param name="message">The message of the entry.</param>
        /// <param name="args">The formatting arguments for the message.</param>
        void Log(string category, TraceEventType severity, string message, params object[] args);
    }
}