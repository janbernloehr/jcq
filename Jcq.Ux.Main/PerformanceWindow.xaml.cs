// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceWindow.xaml.cs" company="Jan-Cornelius Molnar">
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