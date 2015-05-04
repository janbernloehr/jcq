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
  public sealed class Kernel
  {
    private static Interfaces.IMapper _Mapper = new Mapper();
    private static Interfaces.IServiceProvider<Interfaces.IService> _ServiceProvider = new ServiceProvider<Interfaces.IService>();
    private static Interfaces.ILoggingService _Logger = Services.GetService<Interfaces.ILoggingService>();
    private static Interfaces.ITaskScheduler _TaskScheduler = new SimpleTaskScheduler();

    private Kernel()
    {

    }

    /// <summary>
    /// Provides services derived from IService.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public static Interfaces.IServiceProvider<Interfaces.IService> Services {
      get { return _ServiceProvider; }
    }

    public static Interfaces.ITaskScheduler TaskScheduler {
      get { return _TaskScheduler; }
    }

    /// <summary>
    /// Provides exception handling.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public static Interfaces.Exceptions.IExceptionService Exceptions {
      get { return Services.GetService<Interfaces.Exceptions.IExceptionService>(); }
    }

    public static Interfaces.ILoggingService Logger {
      get { return _Logger; }
    }

    public static Interfaces.IMapper Mapper {
      get { return _Mapper; }
    }
  }
}

