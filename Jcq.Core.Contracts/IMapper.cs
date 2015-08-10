// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMapper.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.Core.Contracts
{
    /// <summary>
    ///     Defines a contract for an contract to implementation mapper.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        ///     Returns the type of the implementation registered for the given contract.
        /// </summary>
        /// <typeparam name="TC">The type of the contract.</typeparam>
        /// <returns>The type of the implementation.</returns>
        Type GetImplementationType<TC>();

        /// <summary>
        ///     Returns the type of the implementation registered for the given contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The type of the implementation.</returns>
        Type GetImplementationType(Type contractType);

        /// <summary>
        ///     Returns an instance of the implementation registered for the given contract.
        /// </summary>
        /// <typeparam name="TC">The type of the contract.</typeparam>
        /// <param name="args">The constructor arguments to pass to the implementation.</param>
        /// <returns>An implementation instance of the contract.</returns>
        TC CreateImplementation<TC>(params object[] args);

        /// <summary>
        ///     Returns an instance of the implementation registered for the given contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="args">The constructor arguments to pass to the implementation.</param>
        /// <returns>An implementation instance of the contract.</returns>
        object CreateImplementation(Type contractType, params object[] args);

        /// <summary>
        ///     Gets a value indicating whether an implementation has been registered for the given contract.
        /// </summary>
        /// <typeparam name="TC">The type of the contract.</typeparam>
        /// <returns>True if an implementation has been registered. Otherwise false.</returns>
        bool ExistsContractImplementation<TC>();

        /// <summary>
        ///     Gets a value indicating whether an implementation has been registered for the given contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>True if an implementation has been registered. Otherwise false.</returns>
        bool ExistsContractImplementation(Type contractType);

        /// <summary>
        ///     Registers the given type as an implementation for the given contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        void RegisterContractImplementation(Type contractType, Type implementationType);
    }
}