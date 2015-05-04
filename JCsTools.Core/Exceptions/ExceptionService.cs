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
namespace JCsTools.Core.Exceptions
{
  public class ExceptionService : Service, Interfaces.Exceptions.IExceptionService
  {
    private NotifyingCollection<Interfaces.Exceptions.IExceptionInformation> _Exceptions;
    private ReadOnlyNotifyingCollection<Interfaces.Exceptions.IExceptionInformation> _ReadOnlyExceptions;

    public ExceptionService()
    {
      _Exceptions = new NotifyingCollection<Interfaces.Exceptions.IExceptionInformation>();
      _ReadOnlyExceptions = new ReadOnlyNotifyingCollection<Interfaces.Exceptions.IExceptionInformation>(_Exceptions);
    }

    public System.Collections.ObjectModel.ReadOnlyCollection<Interfaces.Exceptions.IExceptionInformation> Interfaces.Exceptions.IExceptionService.Exceptions {
      get { return _ReadOnlyExceptions; }
    }

    public void Interfaces.Exceptions.IExceptionService.PublishException(System.Exception ex)
    {
      PublishException(ex, false, false);
    }

    public void Interfaces.Exceptions.IExceptionService.PublishException(System.Exception ex, bool handled)
    {
      PublishException(ex, handled, false);
    }

    public void Interfaces.Exceptions.IExceptionService.PublishException(System.Exception ex, bool handled, bool displayed)
    {
      ExceptionInformation info;

      info = new ExceptionInformation(ex, handled, displayed);

      lock (_Exceptions) {
        _Exceptions.Add(info);
      }

      Core.Kernel.Logger.Log("ExceptionService", TraceEventType.Error, ex.ToString);

      if (ExceptionPublished != null) {
        ExceptionPublished(this, new Interfaces.ExceptionEventArgs(info));
      }
    }

    public event ExceptionPublishedEventHandler ExceptionPublished;
    public delegate void ExceptionPublishedEventHandler(object sender, Interfaces.ExceptionEventArgs e);
  }
}

