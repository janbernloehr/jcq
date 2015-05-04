using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace JCsTools.Core
{
  public class DebugTraceListener : TraceListener
  {
    public override void Write(string message)
    {
      Debug.Write(message);
    }

    public override void WriteLine(string message)
    {
      Debug.WriteLine(message);
    }
  }
}

