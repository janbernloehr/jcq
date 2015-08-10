// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqUsageReportService.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using Jcq.Core;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
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

                MinimumUsageReportIntervallReceived?.Invoke(this,
                    new IntervallReceivedEventArgs(data.MinimumReportIntervall));
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

                UsageReportAccepted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}