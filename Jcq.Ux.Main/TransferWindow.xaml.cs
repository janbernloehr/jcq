// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public partial class TransferWindow : Window
    {
        public TransferWindow()
        {
            ViewModel = new TransferWindowViewModel();

            ViewModel.PropertyChanged += OnViewModelPropertyChanged;

            InitializeComponent();

            App.DefaultWindowStyle.Attach(this);
        }

        public TransferWindowViewModel ViewModel { get; private set; }

        protected void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Message")
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(MessageTextBox.ScrollToEnd));
            }
        }
    }

    //Public Class TransferTracer
    //    Implements System.ComponentModel.INotifyPropertyChanged

    //    Public Sub New()
    //        Dim transfer As IcqInterface.IIcqDataTranferService = DirectCast(App.[Default].Context.GetService(Of IcqInterface.Interfaces.IConnector)(), IcqInterface.IIcqDataTranferService)

    //        AddHandler transfer.FlapReceived, AddressOf OnFlapReceived
    //        AddHandler transfer.FlapSent, AddressOf OnFlapSent
    //    End Sub

    //    Private Sub OnFlapReceived(ByVal sender As Object, ByVal e As IcqInterface.FlapTransportEventArgs)
    //        Try
    //            Dim infos As New List(Of String)

    //            For Each x As IcqInterface.DataTypes.ISerializable In e.Flap.DataItems
    //                infos.Add(CObj(x).ToString)
    //            Next

    //            Message += String.Format("{0}: << Seq: {1} Channel: {2} Items: {3}", Date.Now.ToLongTimeString, e.Flap.DatagramSequenceNumber, e.Flap.Channel, String.Join(", ", infos.ToArray)) & System.Environment.NewLine
    //        Catch ex As Exception
    //            Core.Kernel.Exceptions.PublishException(ex)
    //        End Try
    //    End Sub

    //    Private Sub OnFlapSent(ByVal sender As Object, ByVal e As IcqInterface.FlapTransportEventArgs)
    //        Try
    //            Dim infos As New List(Of String)

    //            For Each x As IcqInterface.DataTypes.ISerializable In e.Flap.DataItems
    //                infos.Add(CObj(x).ToString)
    //            Next

    //            Message += String.Format("{0}: >> Seq: {1} Channel: {2} Items: {3}", Date.Now.ToLongTimeString, e.Flap.DatagramSequenceNumber, e.Flap.Channel, String.Join(", ", infos.ToArray)) & System.Environment.NewLine
    //        Catch ex As Exception
    //            Core.Kernel.Exceptions.PublishException(ex)
    //        End Try
    //    End Sub

    //    Private _Message As String
    //    Public Property Message() As String
    //        Get
    //            Return _Message
    //        End Get
    //        Set(ByVal value As String)
    //            _Message = value

    //            OnPropertyChanged("Message")
    //        End Set
    //    End Property

    //    Private Sub OnPropertyChanged(ByVal propertyName As String)
    //        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
    //    End Sub

    //    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    //End Class
}