// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountDownTimer.cs" company="Jan-Cornelius Molnar">
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
using System.Threading;

namespace Jcq.Ux.ViewModel
{
    public class CountDownTimer : IDisposable
    {
        private readonly TimeSpan _countDownDue;
        private readonly Timer _timer;

        public CountDownTimer(TimeSpan countDownDue)
        {
            _countDownDue = countDownDue;

            _timer = new Timer(OnTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        public bool IsRunning { get; private set; }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        public event EventHandler Tick;

        private void OnTimerCallback(object state)
        {
            Stop();
            if (Tick != null)
            {
                Tick(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            _timer.Change(Convert.ToInt64(_countDownDue.TotalMilliseconds), Timeout.Infinite);
            IsRunning = true;
        }

        public void Reset()
        {
            _timer.Change(Convert.ToInt64(_countDownDue.TotalMilliseconds), Timeout.Infinite);
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            IsRunning = false;
        }
    }
}