using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core
{
  public abstract class BasicAsyncTask : BaseTask
  {
    private System.Threading.ManualResetEvent _WaitCompleted;

    public BasicAsyncTask()
    {
      _WaitCompleted = new Threading.ManualResetEvent(false);
    }

    public override bool IsAsync {
      get { return true; }
    }

    protected override void SetCompleted()
    {
      base.SetCompleted();

      _WaitCompleted.Set();
    }

    protected override abstract void PerformOperation();

    public override void WaitCompleted()
    {
      _WaitCompleted.WaitOne(Threading.Timeout.Infinite, true);

      if (Exception != null)
        throw Exception;
    }
  }
}

