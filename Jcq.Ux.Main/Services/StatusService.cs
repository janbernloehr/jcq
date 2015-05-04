// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusService.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Diagnostics;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public class StatusService : ContextService, IStatusService
    {
        public StatusService(IContext context) : base(context)
        {
            AvailableStatuses = new List<IStatusCode>
            {
                IcqStatusCodes.Online,
                IcqStatusCodes.Away,
                IcqStatusCodes.DoNotDisturb,
                IcqStatusCodes.Free4Chat,
                IcqStatusCodes.Invisible,
                IcqStatusCodes.NotAvailable,
                IcqStatusCodes.Occupied
            };
        }

        public List<IStatusCode> AvailableStatuses { get; private set; }

        public void SetIdentityStatus(IStatusCode value)
        {
            Kernel.Logger.Log("StatusService", TraceEventType.Information, "Changing status to: {0}", value);

            Context.SetMyStatus(value);
        }
    }
}