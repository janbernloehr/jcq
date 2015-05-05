// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseTask.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core.Interfaces;

namespace JCsTools.Core
{
    public abstract class BaseTask : ITask
    {
        protected BaseTask()
        {
            Id = Guid.NewGuid();
        }

        protected Exception Exception { get; private set; }
        public Guid Id { get; private set; }
        public bool IsCompleted { get; private set; }

        public virtual void Run()
        {
            if (IsAsync)
            {
                try
                {
                    PerformOperation();
                }
                catch (Exception ex)
                {
                    SetException(ex);
                    SetCompleted();
                }
            }
            else
            {
                PerformOperation();
                SetCompleted();
            }
        }

        public abstract void WaitCompleted();
        public abstract bool IsAsync { get; }
        public event EventHandler Completed;
        protected abstract void PerformOperation();

        /// <summary>
        ///     Sets an exception on the task. This method should be called when an exception
        ///     occures during task execution.
        /// </summary>
        protected virtual void SetException(Exception ex)
        {
            Exception = ex;
        }

        /// <summary>
        ///     Sets the task to completed.
        /// </summary>
        protected virtual void SetCompleted()
        {
            IsCompleted = true;
            Kernel.Logger.Log("TaskScheduler", TraceEventType.Information, "Task {0} completed.", Id);
        }
    }
}