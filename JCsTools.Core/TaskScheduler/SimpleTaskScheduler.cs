using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core
{
  public class SimpleTaskScheduler : Core.Interfaces.ITaskScheduler
  {
    public void Interfaces.ITaskScheduler.RunAsync(Interfaces.ITask task)
    {
      Kernel.Logger.Log("TaskScheduler", TraceEventType.Information, "Running task {0} asynchronous.", task.Id);

      if (task.IsAsync) {
        // The task is implemented asynchronous so here is nothing to do.
        task.Run();
      } else {
        // The task is implemented synchronous so we have to make it async.
        Threading.ThreadPool.QueueUserWorkItem(new Threading.WaitCallback(RunAsyncHelper), task);
      }
    }

    /// <summary>
    /// This is a helper to allow asynchronous execution of a task. It casts the state object to ITask and executes the Run method.
    /// </summary>
    private void RunAsyncHelper(object state)
    {
      Interfaces.ITask task;

      try {
        task = (Interfaces.ITask)state;
        task.Run();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public void Interfaces.ITaskScheduler.RunSync(Interfaces.ITask task)
    {
      Kernel.Logger.Log("TaskScheduler", TraceEventType.Information, "Running task {0} synchronous.", task.Id);

      if (!task.IsAsync) {
        // The task is implemented synchronous so here is nothing to do.
        task.Run();
      } else {
        // The task is implemented asynchronous so we have to wait for it to complete.
        task.Run();
        task.WaitCompleted();
      }
    }
  }
}

