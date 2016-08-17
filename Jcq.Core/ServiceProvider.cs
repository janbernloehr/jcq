// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProvider.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.Threading;
using Jcq.Core.Contracts;

namespace Jcq.Core
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
            TC serviceImplementation = default(TC);
            Type serviceContract = typeof(TC);

            try
            {
                _lock.AcquireReaderLock(WaitTimeout);

                if (!_services.ContainsKey(serviceContract))
                {
                    Type serviceImplementationType = Kernel.Mapper.GetImplementationType<TC>();

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
                        LockCookie cookie = _lock.UpgradeToWriterLock(WaitTimeout);

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