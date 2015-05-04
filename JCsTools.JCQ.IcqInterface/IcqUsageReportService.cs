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
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqUsageReportService : ContextService, Interfaces.IUsageReportService
  {
    public IcqUsageReportService(Interfaces.IContext context) : base(context)
    {

      IcqConnector connector = context.GetService<Interfaces.IConnector>() as IcqConnector;

      if (connector == null)
        throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

      connector.RegisterSnacHandler(0xb, 0x2, new Action<DataTypes.Snac0B02>(AnalyseSnac0B02));
      connector.RegisterSnacHandler(0xb, 0x4, new Action<DataTypes.Snac0B04>(AnalyseSnac0B04));
    }

    public event MinimumUsageReportIntervallReceivedEventHandler MinimumUsageReportIntervallReceived;
    public delegate void MinimumUsageReportIntervallReceivedEventHandler(object sender, Interfaces.IntervallReceivedEventArgs e);

    private void AnalyseSnac0B02(DataTypes.Snac0B02 data)
    {
      try {
        Core.Kernel.Logger.Log("IcqUsageReportService", TraceEventType.Information, "Minimum report intervall: {0}", data.MinimumReportIntervall);

        if (MinimumUsageReportIntervallReceived != null) {
          MinimumUsageReportIntervallReceived(this, new Interfaces.IntervallReceivedEventArgs(data.MinimumReportIntervall));
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void AnalyseSnac0B04(DataTypes.Snac0B04 data)
    {
      try {
        Core.Kernel.Logger.Log("IcqUsageReportService", TraceEventType.Information, "Usage report accepted.");

        if (UsageReportAccepted != null) {
          UsageReportAccepted(this, EventArgs.Empty);
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public void Interfaces.IUsageReportService.SendUsageReport()
    {
      DataTypes.Snac0B03 dataOut;

      dataOut = new DataTypes.Snac0B03();

      dataOut.TlvUsageReport.ScreenName = Context.Identity.Identifier;

      // TODO: IcqUsageReportService
      // Implement proper plattform description
      dataOut.TlvUsageReport.OperatingSystem = Environment.OSVersion.Platform.ToString;
      dataOut.TlvUsageReport.OperatingSystemVersion = Environment.OSVersion.Version;
      dataOut.TlvUsageReport.ProcessorType = "Unknown";
      dataOut.TlvUsageReport.WinsockDllDescription = "Unknown";
      dataOut.TlvUsageReport.WinsockDllVersion = new Version(0, 0, 0, 0);

      IIcqDataTranferService transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }

    public event UsageReportAcceptedEventHandler UsageReportAccepted;
    public delegate void UsageReportAcceptedEventHandler(object sender, System.EventArgs e);
  }
}

