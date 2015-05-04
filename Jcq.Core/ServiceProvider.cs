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
    public class ServiceProvider<S> : Service, IServiceProvider<S> where S : IService
    {
        private const int WAIT_TIMEOUT = 20000;
        private readonly ReaderWriterLock _lock;
        private readonly Dictionary<Type, S> mServices;

        public ServiceProvider()
        {
            _lock = new ReaderWriterLock();

            mServices = new Dictionary<Type, S>();
        }

        public C GetService<C>() where C : S
        {
            Type serviceContract;
            var serviceImplementation = default(C);
            Type serviceImplementationType;
            LockCookie cookie;

            serviceContract = typeof (C);

            try
            {
                _lock.AcquireReaderLock(WAIT_TIMEOUT);

                if (!mServices.ContainsKey(serviceContract))
                {
                    serviceImplementationType = Kernel.Mapper.GetImplementationType<C>();

                    foreach (var pair in mServices)
                    {
                        if (ReferenceEquals(pair.GetType(), serviceImplementationType) ||
                            pair.GetType().IsSubclassOf(serviceImplementationType) ||
                            serviceImplementationType.IsSubclassOf(pair.GetType()))
                        {
                            serviceImplementation = (C) pair.Value;
                        }
                    }

                    if (serviceImplementation == null)
                        serviceImplementation = Kernel.Mapper.CreateImplementation<C>();

                    if (serviceImplementation != null)
                    {
                        cookie = _lock.UpgradeToWriterLock(WAIT_TIMEOUT);

                        try
                        {
                            mServices.Add(serviceContract, serviceImplementation);
                        }
                        finally
                        {
                            _lock.DowngradeFromWriterLock(ref cookie);
                        }
                    }
                }
                else
                {
                    serviceImplementation = (C) mServices[serviceContract];
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