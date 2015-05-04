/// <summary>
/// Provides a base Class for task implementations.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core
{
  public abstract class BaseTask : Interfaces.ITask
  {
    private Guid _Id;
    private bool _IsCompleted;
    private Exception _Exception;

    public event CompletedEventHandler Completed;
    public delegate void CompletedEventHandler(object sender, System.EventArgs e);

    public BaseTask()
    {
      _Id = Guid.NewGuid;
    }

    public System.Guid Interfaces.ITask.Id {
      get { return _Id; }
    }

    public bool Interfaces.ITask.IsCompleted {
      get { return _IsCompleted; }
    }

    protected Exception Exception {
      get { return _Exception; }
    }

    public virtual void Interfaces.ITask.Run()
    {
      if (IsAsync) {
        try {
          PerformOperation();
        } catch (Exception ex) {
          SetException(ex);
          SetCompleted();
        }
      } else {
        PerformOperation();
        SetCompleted();
      }
    }

    public abstract void Interfaces.ITask.WaitCompleted();

    public abstract bool Interfaces.ITask.IsAsync { get; }
    protected abstract void PerformOperation();

    /// <summary>
    /// Sets an exception on the task. This method should be called when an exception
    /// occures during task execution.
    /// </summary>
    protected virtual void SetException(Exception ex)
    {
      _Exception = ex;
    }

    /// <summary>
    /// Sets the task to completed.
    /// </summary>
    protected virtual void SetCompleted()
    {
      _IsCompleted = true;
      Kernel.Logger.Log("TaskScheduler", TraceEventType.Information, "Task {0} completed.", Id);
    }
  }
}

