// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Kernel.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.Core.Interfaces;
using JCsTools.Core.Interfaces.Exceptions;

namespace JCsTools.Core
{
    public static class Kernel
    {
        private static readonly IMapper _Mapper = new Mapper();
        private static readonly IServiceProvider<IService> _ServiceProvider = new ServiceProvider<IService>();
        private static readonly ILoggingService _Logger = Services.GetService<ILoggingService>();

        /// <summary>
        ///     Provides services derived from IService.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IServiceProvider<IService> Services
        {
            get { return _ServiceProvider; }
        }

        /// <summary>
        ///     Provides exception handling.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IExceptionService Exceptions
        {
            get { return Services.GetService<IExceptionService>(); }
        }

        public static ILoggingService Logger
        {
            get { return _Logger; }
        }

        public static IMapper Mapper
        {
            get { return _Mapper; }
        }
    }
}