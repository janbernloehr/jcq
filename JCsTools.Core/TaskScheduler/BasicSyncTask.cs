using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core
{
  public abstract class BasicSyncTask : BaseTask
  {
    public override bool IsAsync {
      get { return false; }
    }

    protected override abstract void PerformOperation();

    public override void WaitCompleted()
    {
      throw new NotImplementedException("You cannot wait for a synchronous task.");
    }
  }
}

