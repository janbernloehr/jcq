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
namespace JCsTools.Core
{
  public class ServiceProvider<S> : Service, Interfaces.IServiceProvider<S> where S : Interfaces.IService
  {
    private System.Collections.Generic.Dictionary<System.Type, S> mServices;
    private System.Threading.ReaderWriterLock _lock;
    private const int WAIT_TIMEOUT = 20000;

    public ServiceProvider()
    {
      _lock = new System.Threading.ReaderWriterLock();

      mServices = new System.Collections.Generic.Dictionary<System.Type, S>();
    }

    public C Interfaces.IServiceProvider<S>.GetService<C>() where C : S
    {
      System.Type serviceContract;
      C serviceImplementation;
      System.Type serviceImplementationType;
      System.Threading.LockCookie cookie;

      serviceContract = typeof(C);

      try {
        _lock.AcquireReaderLock(WAIT_TIMEOUT);

        if (!mServices.ContainsKey(serviceContract)) {
          serviceImplementationType = Core.Kernel.Mapper.GetImplementationType<C>();

          foreach (KeyValuePair<Type, S> pair in mServices) {
            if (object.ReferenceEquals(pair.GetType, serviceImplementationType) || pair.GetType.IsSubclassOf(serviceImplementationType) || serviceImplementationType.IsSubclassOf(pair.GetType)) {
              serviceImplementation = (C)pair.Value;
            }
          }

          if (serviceImplementation == null)
            serviceImplementation = Core.Kernel.Mapper.CreateImplementation<C>();

          if (!serviceImplementation == null) {
            cookie = _lock.UpgradeToWriterLock(WAIT_TIMEOUT);

            try {
              mServices.Add(serviceContract, serviceImplementation);
            } finally {
              _lock.DowngradeFromWriterLock(cookie);
            }
          }
        } else {
          serviceImplementation = (C)mServices(serviceContract);
        }
      } catch (Exception ex) {
        if (ex is Interfaces.ServiceNotFoundException) {
          throw;
        } else {
          throw new Interfaces.ServiceNotFoundException(serviceContract, ex);
        }
      } finally {
        _lock.ReleaseReaderLock();
      }

      return serviceImplementation;
    }
  }
}

