// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferWindow.xaml.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Jcq.Ux.ViewModel;
using JCsTools.JCQ.Ux;

namespace Jcq.Ux.Main.Views
{
    public partial class TransferWindow : Window
    {
        public TransferWindow()
        {
            ViewModel = new TransferWindowViewModel();

            ViewModel.PropertyChanged += OnViewModelPropertyChanged;

            DataContext = ViewModel;

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