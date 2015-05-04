// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionService.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using JCsTools.Core.Interfaces;
using JCsTools.Core.Interfaces.Exceptions;

namespace JCsTools.Core.Exceptions
{
    public class ExceptionService : Service, IExceptionService
    {
        private readonly NotifyingCollection<IExceptionInformation> _Exceptions;
        private readonly ReadOnlyNotifyingCollection<IExceptionInformation> _ReadOnlyExceptions;

        public ExceptionService()
        {
            _Exceptions = new NotifyingCollection<IExceptionInformation>();
            _ReadOnlyExceptions = new ReadOnlyNotifyingCollection<IExceptionInformation>(_Exceptions);
        }

        public ReadOnlyCollection<IExceptionInformation> Exceptions
        {
            get { return _ReadOnlyExceptions; }
        }

        public void PublishException(Exception ex)
        {
            PublishException(ex, false, false);
        }

        public void PublishException(Exception ex, bool handled)
        {
            PublishException(ex, handled, false);
        }

        public void PublishException(Exception ex, bool handled, bool displayed)
        {
            ExceptionInformation info;

            info = new ExceptionInformation(ex, handled, displayed);

            lock (_Exceptions)
            {
                _Exceptions.Add(info);
            }

            Kernel.Logger.Log("ExceptionService", TraceEventType.Error, ex.ToString());

            if (ExceptionPublished != null)
            {
                ExceptionPublished(this, new ExceptionEventArgs(info));
            }
        }

        public event EventHandler<ExceptionEventArgs> ExceptionPublished;
    }
}