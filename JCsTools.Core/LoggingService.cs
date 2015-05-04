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
using Logger = Microsoft.Practices.EnterpriseLibrary.Logging.Logger;
namespace JCsTools.Core
{
  public class LoggingService : Service, Interfaces.ILoggingService
  {
    public int Interfaces.ILoggingService.DefaultEventId {
      get { return 100; }
    }

    public int Interfaces.ILoggingService.DefaultPriority {
      get { return 100; }
    }

    public void Interfaces.ILoggingService.Log(string category, System.Diagnostics.TraceEventType severity, string message)
    {
      Logger.Write(message, category, DefaultPriority, DefaultEventId, severity);
    }

    public void Interfaces.ILoggingService.Log(string category, System.Diagnostics.TraceEventType severity, string message, params object[] args)
    {
      Logger.Write(string.Format(message, args), category, DefaultPriority, DefaultEventId, severity);
    }
  }
}

