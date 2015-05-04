// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTaskScheduler.cs" company="Jan-Cornelius Molnar">
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
using System.Threading;
using JCsTools.Core.Interfaces;

namespace JCsTools.Core
{
    public class SimpleTaskScheduler : ITaskScheduler
    {
        void ITaskScheduler.RunAsync(ITask task)
        {
            Kernel.Logger.Log("TaskScheduler", TraceEventType.Information, "Running task {0} asynchronous.", task.Id);

            if (task.IsAsync)
            {
                // The task is implemented asynchronous so here is nothing to do.
                task.Run();
            }
            else
            {
                // The task is implemented synchronous so we have to make it async.
                ThreadPool.QueueUserWorkItem(RunAsyncHelper, task);
            }
        }

        public void RunSync(ITask task)
        {
            Kernel.Logger.Log("TaskScheduler", TraceEventType.Information, "Running task {0} synchronous.", task.Id);

            if (!task.IsAsync)
            {
                // The task is implemented synchronous so here is nothing to do.
                task.Run();
            }
            else
            {
                // The task is implemented asynchronous so we have to wait for it to complete.
                task.Run();
                task.WaitCompleted();
            }
        }

        /// <summary>
        ///     This is a helper to allow asynchronous execution of a task. It casts the state object to ITask and executes the Run
        ///     method.
        /// </summary>
        private void RunAsyncHelper(object state)
        {
            ITask task;

            try
            {
                task = (ITask) state;
                task.Run();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}