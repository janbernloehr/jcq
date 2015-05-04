using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Controls
{
  public class CustomControl1 : Control
  {
    static CustomControl1()
    {
      //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
      //This style is defined in Themes\Generic.xaml
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
    }

  }
}

