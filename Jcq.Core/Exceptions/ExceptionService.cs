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
        private readonly NotifyingCollection<IExceptionInformation> _exceptions;
        private readonly ReadOnlyNotifyingCollection<IExceptionInformation> _readOnlyExceptions;

        public ExceptionService()
        {
            _exceptions = new NotifyingCollection<IExceptionInformation>();
            _readOnlyExceptions = new ReadOnlyNotifyingCollection<IExceptionInformation>(_exceptions);
        }

        public ReadOnlyCollection<IExceptionInformation> Exceptions
        {
            get { return _readOnlyExceptions; }
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
            var info = new ExceptionInformation(ex, handled, displayed);

            lock (_exceptions)
            {
                _exceptions.Add(info);
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