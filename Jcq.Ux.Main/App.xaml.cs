// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Jcq.Core;
using Jcq.Ux.Main.Views;
using Jcq.Ux.ViewModel;
using JCsTools.JCQ.Ux;
using WindowStyle = Jcq.Ux.Main.Code.WindowStyle;

namespace Jcq.Ux.Main
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly WindowStyle Style = new WindowStyle
        {
            AllowTransparency = true,
            Opacity = 0.95
        };

        public static WindowStyle DefaultWindowStyle
        {
            get { return Style; }
        }

        #region  Persistance

        public static string DataStorageDirectoryPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "jcq\\"); }
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

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += OnAppDomainException;

                Kernel.Logger.Log("Ux", TraceEventType.Start, "Starting up JCQ.");

                ApplicationService.Initialize(new DirectoryInfo(DataStorageDirectoryPath));
                ApplicationService.Current.LoadServiceData();

                var hostWindow = new MainWindow();

                hostWindow.Content = new SignInPage();
                hostWindow.Show();

                var exceptionWindow = new ExceptionWindow();
                exceptionWindow.Show();

                var performanceWindow = new PerformanceWindow();
                performanceWindow.Show();

                //activitesWindow = New ActivitiesWindow
                //activitesWindow.Show()

                //avatarSelWindow = New TestAvatarSelector
                //avatarSelWindow.Show()

                MainWindow = hostWindow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                ApplicationService.Current.SaveServiceData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ShowStatusWindows()
        {
            var transferInfoWindow = new TransferWindow();

            transferInfoWindow.Show();
        }

        #region  Unhandled Exceptions

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void OnAppDomainException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "JCQ Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}