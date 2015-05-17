// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionService.cs" company="Jan-Cornelius Molnar">
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