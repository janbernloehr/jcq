//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface.DataTypes.My
{

  [System.Runtime.CompilerServices.CompilerGeneratedAttribute(), System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
  internal sealed partial class MySettings : global::System.Configuration.ApplicationSettingsBase
  {
    private static MySettings defaultInstance = (MySettings)global::System.Configuration.ApplicationSettingsBase.Synchronized(new MySettings());

#region My.Settings Auto-Save Functionality
#if (false)
    private static bool addedHandler;

    private static object addedHandlerLockObject = new object();

    [System.Diagnostics.DebuggerNonUserCodeAttribute(), System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    private static void AutoSaveSettings(global::System.Object sender, global::System.EventArgs e)
    {
      if (My.Application.SaveMySettingsOnExit) {
        My.Settings.Save();
      }
    }
#endif
#endregion

    public static MySettings Default {
      get {

#if (false)
        if (!addedHandler) {
          lock (addedHandlerLockObject) {
            if (!addedHandler) {
              My.Application.Shutdown += AutoSaveSettings;
              addedHandler = true;
            }
          }
        }
#endif
        return defaultInstance;
      }
    }
  }
}
namespace JCsTools.JCQ.IcqInterface.DataTypes.My
{

  [Microsoft.VisualBasic.HideModuleNameAttribute(), System.Diagnostics.DebuggerNonUserCodeAttribute(), System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  internal class MySettingsProperty
  {

    [System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")]
    internal global::JCsTools.JCQ.IcqInterface.DataTypes.My.MySettings Settings {
      get { return global::JCsTools.JCQ.IcqInterface.DataTypes.My.MySettings.Default; }
    }
  }
}

