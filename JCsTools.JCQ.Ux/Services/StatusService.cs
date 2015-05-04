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
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
namespace JCsTools.JCQ.Ux
{
  public class StatusService : ContextService, IStatusService
  {
    public StatusService(Interfaces.IContext context) : base(context)
    {
    }

    private List<IStatusCode> _AvailableStatuses;

    public System.Collections.Generic.List<IcqInterface.Interfaces.IStatusCode> ViewModel.IStatusService.AvailableStatuses {
      get {
        if (_AvailableStatuses == null) {
          _AvailableStatuses = new List<IStatusCode>();

          _AvailableStatuses.Add(IcqStatusCodes.Online);
          _AvailableStatuses.Add(IcqStatusCodes.Away);
          _AvailableStatuses.Add(IcqStatusCodes.DoNotDisturb);
          _AvailableStatuses.Add(IcqStatusCodes.Free4Chat);
          _AvailableStatuses.Add(IcqStatusCodes.Invisible);
          _AvailableStatuses.Add(IcqStatusCodes.NotAvailable);
          _AvailableStatuses.Add(IcqStatusCodes.Occupied);
        }

        return _AvailableStatuses;
      }
    }

    public void ViewModel.IStatusService.SetIdentityStatus(IcqInterface.Interfaces.IStatusCode value)
    {
      Core.Kernel.Logger.Log("StatusService", TraceEventType.Information, "Changing status to: {0}", value);

      Context.SetMyStatus(value);
    }
  }
}

