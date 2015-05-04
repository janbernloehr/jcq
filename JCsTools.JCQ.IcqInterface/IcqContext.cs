//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqContext : Core.Service, Interfaces.IContext
  {
    private IcqContact _Identity;

    public IcqContext(string uin)
    {
      _Identity = new IcqContact(uin, uin);

      GetService<Interfaces.IConnector>();

      GetService<Interfaces.IStorageService>();
      GetService<Interfaces.IMessageService>();
      GetService<Interfaces.IIconService>();

      GetService<Interfaces.IDataWarehouseService>();
    }

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

    private Dictionary<Type, Interfaces.IContextService> cachedBindings = new Dictionary<Type, Interfaces.IContextService>();
    private Dictionary<Type, Interfaces.IContextService> cachedInstances = new Dictionary<Type, Interfaces.IContextService>();

    public C Core.Interfaces.IServiceProvider<Interfaces.IContextService>.GetService<C>() where C : Interfaces.IContextService
    {
      if (!cachedBindings.ContainsKey(typeof(C))) {
        Type type = Core.Kernel.Mapper.GetImplementationType<C>();

        if (!cachedInstances.ContainsKey(type)) {
          cachedInstances.Add(type, (C)Activator.CreateInstance(type, this));
        }

        cachedBindings.Add(typeof(C), cachedInstances(type));
      }

      return (C)cachedBindings(typeof(C));
    }

    public Interfaces.IContact Interfaces.IContext.Identity {
      get { return _Identity; }
    }

    public void Interfaces.IContext.SetMyStatus(Interfaces.IStatusCode statusCode)
    {
      DataTypes.Snac011e dataOut;
      IcqInterface.DataTypes.UserStatus status;

      status = statusCode.Attributes("IcqUserStatus") == null ? DataTypes.UserStatus.Offline : (DataTypes.UserStatus)statusCode.Attributes("IcqUserStatus");

      dataOut = new DataTypes.Snac011e();
      dataOut.UserStatus.UserStatus = status;

      IIcqDataTranferService transfer = (IIcqDataTranferService)GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }
  }
}

