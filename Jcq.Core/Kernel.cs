// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Kernel.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.Core.Interfaces;
using JCsTools.Core.Interfaces.Exceptions;

namespace JCsTools.Core
{
    public sealed class Kernel
    {
        private static readonly IMapper _Mapper = new Mapper();
        private static readonly IServiceProvider<IService> _ServiceProvider = new ServiceProvider<IService>();
        private static readonly ILoggingService _Logger = Services.GetService<ILoggingService>();
        private static readonly ITaskScheduler _TaskScheduler = new SimpleTaskScheduler();

        private Kernel()
        {
        }

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

        public static ITaskScheduler TaskScheduler
        {
            get { return _TaskScheduler; }
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