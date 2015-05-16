// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProvider.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Collections.Generic;
using System.Threading;
using JCsTools.Core.Interfaces;

namespace JCsTools.Core
{
    public class ServiceProvider<TS> : Service, IServiceProvider<TS> where TS : IService
    {
        private const int WaitTimeout = 20000;
        private readonly ReaderWriterLock _lock;
        private readonly Dictionary<Type, TS> _services;

        public ServiceProvider()
        {
            _lock = new ReaderWriterLock();
            _services = new Dictionary<Type, TS>();
        }

        public TC GetService<TC>()
            where TC : TS
        {
            var serviceImplementation = default(TC);
            var serviceContract = typeof (TC);

            try
            {
                _lock.AcquireReaderLock(WaitTimeout);

                if (!_services.ContainsKey(serviceContract))
                {
                    var serviceImplementationType = Kernel.Mapper.GetImplementationType<TC>();

                    foreach (var pair in _services)
                    {
                        if (pair.GetType() == serviceImplementationType ||
                            pair.GetType().IsSubclassOf(serviceImplementationType) ||
                            serviceImplementationType.IsSubclassOf(pair.GetType()))
                        {
                            serviceImplementation = (TC) pair.Value;
                        }
                    }

                    if (serviceImplementation == null)
                        serviceImplementation = Kernel.Mapper.CreateImplementation<TC>();

                    if (serviceImplementation != null)
                    {
                        var cookie = _lock.UpgradeToWriterLock(WaitTimeout);

                        try
                        {
                            _services.Add(serviceContract, serviceImplementation);
                        }
                        finally
                        {
                            _lock.DowngradeFromWriterLock(ref cookie);
                        }
                    }
                }
                else
                {
                    serviceImplementation = (TC) _services[serviceContract];
                }
            }
            catch (Exception ex)
            {
                if (ex is ServiceNotFoundException)
                {
                    throw;
                }
                else
                {
                    throw new ServiceNotFoundException(serviceContract, ex);
                }
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }

            return serviceImplementation;
        }
    }
}