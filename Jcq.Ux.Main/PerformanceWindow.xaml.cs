// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.Windows.Threading;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class PerformanceWindow
    {
        private readonly Process _process;
        private readonly DispatcherTimer _timer;

        public PerformanceWindow()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);

            _timer.Tick += OnTimerTick;

            _process = Process.GetCurrentProcess();
            _timer.Start();

            App.DefaultWindowStyle.Attach(this);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            long memoryLoad;
            double cpuTime;
            int threadCount;
            long bytesSent = 0;
            long bytesReceived = 0;

            memoryLoad = _process.WorkingSet64;
            cpuTime = _process.TotalProcessorTime.TotalMilliseconds;
            threadCount = _process.Threads.Count;

            if (ApplicationService.Current.Context != null)
            {
                var svConnect = (IcqConnector) ApplicationService.Current.Context.GetService<IConnector>();

                if (svConnect.TcpContext != null)
                {
                    bytesSent = svConnect.TcpContext.BytesSent;
                    bytesReceived = svConnect.TcpContext.BytesReceived;    
                }
            }

            MemoryUsage.Text = string.Format("{0:0.0}MB", memoryLoad/1024/1024);
            CpuTime.Text = string.Format("{0:#,##0}ms", cpuTime);
            CurrentThreads.Text = string.Format("{0}", threadCount);
            BytesSent.Text = string.Format("{0:#,##0}", bytesSent);
            BytesReceived.Text = string.Format("{0:#,##0}", bytesReceived);
        }
    }
}