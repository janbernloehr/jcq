// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountDownTimer.cs" company="Jan-Cornelius Molnar">
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
using System.Threading;

namespace JCsTools.JCQ.ViewModel
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