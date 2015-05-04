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
namespace JCsTools.JCQ.ViewModel
{
  public class TransferWindowViewModel : System.ComponentModel.INotifyPropertyChanged
  {
    public TransferWindowViewModel()
    {
      IcqInterface.Interfaces.IConnector svConnector;
      IcqInterface.IIcqDataTranferService svTransfer;

      svConnector = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IConnector>();
      svTransfer = (IcqInterface.IIcqDataTranferService)svConnector;

      svConnector.Disconnected += OnSignOut;
      svTransfer.FlapReceived += OnFlapReceived;
      svTransfer.FlapSent += OnFlapSent;
    }

    private void OnSignOut(object sender, IcqInterface.Interfaces.DisconnectedEventArgs e)
    {
      try {
        Message += string.Format("{0}: *** DISCONNECTED (Exp: {1}) *** {2}", System.DateTime.Now.ToLongTimeString, e.IsExpected, e.Message) + System.Environment.NewLine;
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnFlapReceived(object sender, IcqInterface.FlapTransportEventArgs e)
    {
      List<string> infos;

      try {
        infos = new List<string>();

        foreach (IcqInterface.DataTypes.ISerializable x in e.Flap.DataItems) {
          infos.Add(((object)x).ToString);
        }

        if (infos.Count > 0) {
          Message += string.Format("{0}: << Seq: {1} Channel: {2} Items: {3}", System.DateTime.Now.ToLongTimeString, e.Flap.DatagramSequenceNumber, e.Flap.Channel, string.Join(", ", infos.ToArray)) + System.Environment.NewLine;
        } else {
          Message += string.Format("{0}: << Seq: {1} Channel: {2}", System.DateTime.Now.ToLongTimeString, e.Flap.DatagramSequenceNumber, e.Flap.Channel) + System.Environment.NewLine;
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnFlapSent(object sender, IcqInterface.FlapTransportEventArgs e)
    {
      List<string> infos;

      try {
        infos = new List<string>();

        foreach (IcqInterface.DataTypes.ISerializable x in e.Flap.DataItems) {
          infos.Add(((object)x).ToString);
        }

        Message += string.Format("{0}: >> Seq: {1} Channel: {2} Items: {3}", System.DateTime.Now.ToLongTimeString, e.Flap.DatagramSequenceNumber, e.Flap.Channel, string.Join(", ", infos.ToArray)) + System.Environment.NewLine;
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private string _Message;
    public string Message {
      get { return _Message; }
      set {
        _Message = value;

        OnPropertyChanged("Message");
      }
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
  }
}

