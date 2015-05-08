// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingService.cs" company="Jan-Cornelius Molnar">
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

using System.Diagnostics;
using JCsTools.Core.Interfaces;

namespace JCsTools.Core
{
    public class LoggingService : Service, ILoggingService
    {
        private static readonly TraceSource JcqTraceSource;

        static LoggingService()
        {
            JcqTraceSource = new TraceSource("jcq");
            JcqTraceSource.Switch = new SourceSwitch("MySwitch", "Verbose");


        }

        int ILoggingService.DefaultEventId
        {
            get { return 100; }
        }

        int ILoggingService.DefaultPriority
        {
            get { return 100; }
        }

        void ILoggingService.Log(string category, TraceEventType severity, string message)
        {
            //TODO: Logger
            //Logger.Write(message, category, DefaultPriority, DefaultEventId, severity);
            JcqTraceSource.TraceInformation(message);
        }

        void ILoggingService.Log(string category, TraceEventType severity, string message, params object[] args)
        {
            //TODO: Logger
            //Logger.Write(string.Format(message, args), category, DefaultPriority, DefaultEventId, severity);
            JcqTraceSource.TraceInformation(message, args);
        }
    }
}