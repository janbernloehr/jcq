// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqContext.cs" company="Jan-Cornelius Molnar">
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
using System.Threading.Tasks;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqContext : Service, IContext
    {
        private readonly IcqContact _identity;
        //Private _Connector As IcqConnector
        //Public ReadOnly Property Connector() As IcqConnector
        //    Get
        //        Return _Connector
        //    End Get
        //End Property

        //Private _MessageService As IcqMessageService
        //Public ReadOnly Property MessageService() As IcqMessageService
        //    Get
        //        Return _MessageService
        //    End Get
        //End Property

        //Private _StorageService As IcqStorageService
        //Public ReadOnly Property StorageService() As IcqStorageService
        //    Get
        //        Return _StorageService
        //    End Get
        //End Property

        //Private _IconService As IconService
        //Public ReadOnly Property IconService() As IconService
        //    Get
        //        Return _IconService
        //    End Get
        //End Property

        //Private _DataWarehouse As IcqDataWarehouse
        //Public ReadOnly Property DataWarehouse() As IcqDataWarehouse
        //    Get
        //        Return _DataWarehouse
        //    End Get
        //End Property

        private readonly Dictionary<Type, IContextService> _cachedBindings = new Dictionary<Type, IContextService>();
        private readonly Dictionary<Type, IContextService> _cachedInstances = new Dictionary<Type, IContextService>();

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