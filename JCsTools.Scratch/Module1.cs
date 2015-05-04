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
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using JCsTools.JCQ.IcqInterface;
using System.Xml;
namespace JCsTools.Scratch
{
  class Module1
  {
    //Sub Main()
    //    Dim fiContactListData As System.IO.FileInfo
    //    Dim formatter As JCsTools.Xml.Formatter.XmlSerializer
    //    Dim dataReader As XmlTextReader
    //    Dim ctx = New JCsTools.JCQ.IcqInterface.IcqContext("1234567")
    //    fiContactListData = New System.IO.FileInfo("C:\Users\Jan-Cornelius Molnar\AppData\Roaming\jcq\contactlistdata.xml")
    //    If Not fiContactListData.Exists Then Return
    //    formatter = New JCsTools.Xml.Formatter.XmlSerializer
    //    formatter.RegisterReferenceFormatter(GetType(IcqContact), New BaseStorageItemFormatter(ctx, formatter, GetType(IcqContact)))
    //    formatter.RegisterReferenceFormatter(GetType(Core.KeyedNotifiyingCollection(Of String, Interfaces.IContact)), New ContactKeyedNotifiyingCollectionFormatter(formatter))
    //    Dim items As IEnumerable(Of Interfaces.IContact)
    //    Using fs = fiContactListData.OpenRead
    //        dataReader = New System.Xml.XmlTextReader(fs)
    //        dataReader.WhitespaceHandling = WhitespaceHandling.None
    //        items = DirectCast(formatter.Deserialize(dataReader), IEnumerable(Of Interfaces.IContact))
    //    End Using
    //    Console.WriteLine(items.Count)
    //    Console.ReadLine()
    //End Sub
    //Dim coll As New System.Collections.ObjectModel.ObservableCollection(Of Core.Interfaces.IActivity)
    public void Main()
    {
      //Dim bind As New Core.NotifyingCollectionBinding(Of Core.Interfaces.IActivity)(Core.Kernel.ActivityManager.Activities, coll)

      //AddHandler coll.CollectionChanged, AddressOf OnCollectionChanged

      //Dim acc = Core.Kernel.ActivityManager.CreateActivity(Of MyCoolActivity)()

      //acc.Run()

      //Console.ReadLine()
    }

    //Private Sub OnCollectionChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
    //    Console.WriteLine(e.Action.ToString)
    //End Sub

  }

  //Public Class MyCoolActivity
  //    Inherits Core.Activty

  //    Public Sub New(ByVal id As Integer)
  //        MyBase.New(id)
  //    End Sub

  //    Public Overrides ReadOnly Property AutoComplete() As Boolean
  //        Get
  //            Return True
  //        End Get
  //    End Property

  //    Public Overrides ReadOnly Property Description() As String
  //        Get

  //        End Get
  //    End Property

  //    Public Overrides ReadOnly Property Name() As String
  //        Get
  //            Return "MyCool"
  //        End Get
  //    End Property

  //    Protected Overrides Sub RunActivity()
  //        Console.WriteLine("running activity ...")
  //    End Sub
  //End Class
}

