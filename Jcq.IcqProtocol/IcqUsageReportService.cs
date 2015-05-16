// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqUsageReportService.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqUsageReportService : ContextService, IUsageReportService
    {
        public IcqUsageReportService(IContext context) : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            connector.RegisterSnacHandler(0xb, 0x2, new Action<Snac0B02>(AnalyseSnac0B02));
            connector.RegisterSnacHandler(0xb, 0x4, new Action<Snac0B04>(AnalyseSnac0B04));
        }

        void IUsageReportService.SendUsageReport()
        {
            var dataOut = new Snac0B03();

            dataOut.TlvUsageReport.ScreenName = Context.Identity.Identifier;

            // TODO: IcqUsageReportService
            // Implement proper plattform description
            dataOut.TlvUsageReport.OperatingSystem = Environment.OSVersion.Platform.ToString();
            dataOut.TlvUsageReport.OperatingSystemVersion = Environment.OSVersion.Version;
            dataOut.TlvUsageReport.ProcessorType = "Unknown";
            dataOut.TlvUsageReport.WinsockDllDescription = "Unknown";
            dataOut.TlvUsageReport.WinsockDllVersion = new Version(0, 0, 0, 0);

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler<IntervallReceivedEventArgs> MinimumUsageReportIntervallReceived;
        public event EventHandler UsageReportAccepted;

        private void AnalyseSnac0B02(Snac0B02 data)
        {
            try
            {
                Kernel.Logger.Log("IcqUsageReportService", TraceEventType.Information, "Minimum report intervall: {0}",
                    data.MinimumReportIntervall);

                if (MinimumUsageReportIntervallReceived != null)
                {
                    MinimumUsageReportIntervallReceived(this,
                        new IntervallReceivedEventArgs(data.MinimumReportIntervall));
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0B04(Snac0B04 data)
        {
            try
            {
                Kernel.Logger.Log("IcqUsageReportService", TraceEventType.Information, "Usage report accepted.");

                if (UsageReportAccepted != null)
                {
                    UsageReportAccepted(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}