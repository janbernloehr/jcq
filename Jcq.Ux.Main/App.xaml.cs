// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.IO;
using System.Windows;
using System.Windows.Threading;
using JCsTools.Core;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly WindowStyle _Style = new WindowStyle
        {
            AllowTransparency = true,
            Opacity = 0.95
        };

        public static WindowStyle DefaultWindowStyle
        {
            get { return _Style; }
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
            MainWindow hostWindow;
            ExceptionWindow exceptionWindow;
            PerformanceWindow performanceWindow;
            //Dim activitesWindow As ActivitiesWindow
            //Dim avatarSelWindow As TestAvatarSelector

            try
            {
                AppDomain.CurrentDomain.UnhandledException += OnAppDomainException;

                Kernel.Logger.Log("Ux", TraceEventType.Start, "Starting up JCQ.");

                ApplicationService.Initialize(new DirectoryInfo(DataStorageDirectoryPath));
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
            TransferWindow transferInfoWindow;

            transferInfoWindow = new TransferWindow();

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