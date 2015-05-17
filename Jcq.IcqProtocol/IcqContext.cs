// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqContext.cs" company="Jan-Cornelius Molnar">
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
using System.Threading.Tasks;
using Jcq.Core;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    public class IcqContext : Service, IContext
    {
        private readonly Dictionary<Type, IContextService> _cachedBindings = new Dictionary<Type, IContextService>();
        private readonly Dictionary<Type, IContextService> _cachedInstances = new Dictionary<Type, IContextService>();
        private readonly IcqContact _identity;

        public IcqContext(string uin)
        {
            _identity = new IcqContact(uin, uin);

            GetService<IConnector>();

            GetService<IStorageService>();
            GetService<IMessageService>();
            GetService<IIconService>();

            GetService<IDataWarehouseService>();
        }

        public C GetService<C>() where C : IContextService
        {
            if (_cachedBindings.ContainsKey(typeof (C)))
                return (C) _cachedBindings[typeof (C)];

            var type = Kernel.Mapper.GetImplementationType<C>();

            if (!_cachedInstances.ContainsKey(type))
            {
                _cachedInstances.Add(type, (C) Activator.CreateInstance(type, this));
            }

            _cachedBindings.Add(typeof (C), _cachedInstances[type]);

            return (C) _cachedBindings[typeof (C)];
        }

        public IContact Identity
        {
            get { return _identity; }
        }

        public Task SetMyStatus(IStatusCode statusCode)
        {
            var icqStatus = statusCode as IcqStatusCode;

            if (icqStatus == null)
                throw new ArgumentException(@"IcqStatusCode required", "statusCode");

            var status = icqStatus.UserStatus;

            var dataOut = new Snac011e();
            dataOut.UserStatus.UserStatus = status;

            var transfer = (IIcqDataTranferService) GetService<IConnector>();
            return transfer.Send(dataOut);
        }
    }
}