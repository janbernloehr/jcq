//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// Interaction logic for App.xaml
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Ux
{
  public partial class App : System.Windows.Application
  {
    private static WindowStyle _Style = new WindowStyle {
      AllowTransparency = true,
      Opacity = 0.95
    };

    public static WindowStyle DefaultWindowStyle {
      get { return _Style; }
    }

    private void  // ERROR: Handles clauses are not supported in C#
App_Startup(object sender, System.Windows.StartupEventArgs e)
    {
      MainWindow hostWindow;
      ExceptionWindow exceptionWindow;
      PerformanceWindow performanceWindow;
      //Dim activitesWindow As ActivitiesWindow
      //Dim avatarSelWindow As TestAvatarSelector

      try {
        System.AppDomain.CurrentDomain.UnhandledException += OnAppDomainException;

        Core.Kernel.Logger.Log("Ux", TraceEventType.Start, "Starting up JCQ.");

        ApplicationService.Initialize(new System.IO.DirectoryInfo(DataStorageDirectoryPath));
        ApplicationService.Current.LoadServiceData();

        hostWindow = new MainWindow();

        hostWindow.Content = new SignInPage();
        hostWindow.Show();

        exceptionWindow = new ExceptionWindow();
        exceptionWindow.Show();

        performanceWindow = new PerformanceWindow();
        performanceWindow.Show();

        //activitesWindow = New ActivitiesWindow
        //activitesWindow.Show()

        //avatarSelWindow = New TestAvatarSelector
        //avatarSelWindow.Show()

        MainWindow = hostWindow;
      } catch (Exception ex) {
        MessageBox.Show(ex.ToString, "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void  // ERROR: Handles clauses are not supported in C#
App_Exit(object sender, System.Windows.ExitEventArgs e)
    {
      try {
        ApplicationService.Current.SaveServiceData();
      } catch (Exception ex) {
        MessageBox.Show(ex.ToString, "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

#region  Unhandled Exceptions 

    private void  // ERROR: Handles clauses are not supported in C#
App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      MessageBox.Show(e.Exception.ToString, "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
      e.Handled = true;
    }

    private void OnAppDomainException(object sender, UnhandledExceptionEventArgs e)
    {
      MessageBox.Show(e.ExceptionObject.ToString, "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

#endregion

    public static void ShowStatusWindows()
    {
      TransferWindow transferInfoWindow;

      transferInfoWindow = new TransferWindow();

      transferInfoWindow.Show();
    }

#region  Persistance 
    public static string DataStorageDirectoryPath {
      get { return System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "jcq\\"); }
    }

    //Private Sub LoadData()
    //    Dim datadirectory As System.IO.DirectoryInfo

    //    Try
    //        datadirectory = New System.IO.DirectoryInfo(DataStorageDirectoryPath)

    //        Debug.WriteLine(String.Format("DataWarehouse Directory: {0}", DataStorageDirectoryPath), "Ux")

    //        Context.GetService(Of IcqInterface.Interfaces.IDataWarehouseService).Load(datadirectory)
    //        _HistoryService.Load()
    //    Catch ex As Exception
    //        Core.Kernel.Exceptions.PublishException(ex)
    //    End Try
    //End Sub

    //Private Sub PersistDataStorage()
    //    Dim datadirectory As System.IO.DirectoryInfo

    //    If Context Is Nothing Then Return

    //    Try
    //        datadirectory = New System.IO.DirectoryInfo(DataStorageDirectoryPath)

    //        Context.GetService(Of IcqInterface.Interfaces.IDataWarehouseService).Save(datadirectory)
    //    Catch ex As Exception
    //        Core.Kernel.Exceptions.PublishException(ex)
    //    End Try
    //End Sub

    //Private Sub SaveHistory()
    //    If _HistoryService Is Nothing Then Return

    //    _HistoryService.Save()
    //End Sub

    //Private Sub SaveIdentities()
    //    _IdentityProvider.Save()
    //End Sub

    //Private Sub SaveData()
    //    PersistDataStorage()
    //    SaveHistory()
    //    SaveIdentities()
    //End Sub
#endregion

  }

  public class WindowStyle
  {
    public WindowStyle()
    {
      _Opacity = 1;
    }

    private bool _AllowTransparency;
    public bool AllowTransparency {
      get { return _AllowTransparency; }
      set { _AllowTransparency = value; }
    }

    private double _Opacity;
    public double Opacity {
      get { return _Opacity; }
      set { _Opacity = value; }
    }

    public void Attach(Window wnd)
    {
      wnd.Opacity = Opacity;
      wnd.AllowsTransparency = AllowTransparency;
      wnd.WindowStyle = Windows.WindowStyle.None;
      wnd.Style = (Style)App.Current.FindResource("SimpleWindow");

      object extender = JCsTools.Wpf.Extenders.WindowExtenderProvider.AttachResizeExtender(wnd);

      wnd.Resources("WindowExtender") = extender;

      //Style="{StaticResource SimpleWindow}" WindowStyle="None"
    }
  }
}

