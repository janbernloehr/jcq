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
namespace JCsTools.JCQ.Ux
{
  public partial class PerformanceWindow
  {
    private System.Windows.Threading.DispatcherTimer _timer;
    private System.Diagnostics.Process _process;

    public PerformanceWindow()
    {

      // This call is required by the Windows Form Designer.
      InitializeComponent();

      // Add any initialization after the InitializeComponent() call.
      _timer = new System.Windows.Threading.DispatcherTimer();
      _timer.Interval = TimeSpan.FromSeconds(1);

      _timer.Tick += OnTimerTick;

      _process = System.Diagnostics.Process.GetCurrentProcess;
      _timer.Start();

      App.DefaultWindowStyle.Attach(this);
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
      long memoryLoad;
      double cpuLoad;
      int threadCount;
      long bytesSent;
      long bytesReceived;

      memoryLoad = _process.WorkingSet64;
      cpuLoad = _process.TotalProcessorTime.TotalMilliseconds;
      threadCount = _process.Threads.Count;

      if (ApplicationService.Current.Context != null) {
        object svConnect = (IcqInterface.IcqConnector)ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IConnector>();

        bytesSent = svConnect.TcpContext.BytesSent;
        bytesReceived = svConnect.TcpContext.BytesReceived;
      }

      this.MemoryUsage.Text = string.Format("{0:#,##0}", memoryLoad);
      this.CpuUsage.Text = string.Format("{0:#,##0}", cpuLoad);
      this.CurrentThreads.Text = string.Format("{0}", threadCount);
      this.BytesSent.Text = string.Format("{0:#,##0}", bytesSent);
      this.BytesReceived.Text = string.Format("{0:#,##0}", bytesReceived);
    }

  }
}

