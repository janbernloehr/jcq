// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotifyingCollection.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace Jcq.Core.Contracts.Collections
{
    /// <summary>
    ///     Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole
    ///     list is refreshed
    /// </summary>
    /// <typeparam name="T">The type of object in the collection.</typeparam>
    public interface INotifyingCollection<T> : IReadOnlyNotifyingCollection<T>, ICollection<T>
    {
        /// <summary>
        ///     Move an element within the collection.
        /// </summary>
        /// <param name="oldIndex">The current index of the element.</param>
        /// <param name="newIndex">The index the element will be moved to.</param>
        void Move(int oldIndex, int newIndex);
    }
}