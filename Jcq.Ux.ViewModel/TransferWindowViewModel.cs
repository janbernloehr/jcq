// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferWindowViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using Jcq.IcqProtocol.Internal;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class TransferWindowViewModel : INotifyPropertyChanged
    {
        private string _message;

        public TransferWindowViewModel()
        {
            var svConnector = ApplicationService.Current.Context.GetService<IConnector>();
            var svTransfer = (IIcqDataTranferService) svConnector;

            svConnector.Disconnected += OnSignOut;
            svTransfer.FlapReceived += OnFlapReceived;
            svTransfer.FlapSent += OnFlapSent;
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;

                OnPropertyChanged("Message");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnSignOut(object sender, DisconnectedEventArgs e)
        {
            try
            {
                Message += string.Format("{0}: *** DISCONNECTED (Exp: {1}) *** {2}\n",
                    DateTime.Now.ToLongTimeString(), e.IsExpected, e.Message);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnFlapReceived(object sender, FlapTransportEventArgs e)
        {
            try
            {
                var infos = e.Flap.DataItems.Select(x => x.ToString()).ToList();

                if (infos.Count > 0)
                {
                    Message += string.Format("{0}: << Seq: {1} Channel: {2} Items: {3}\n",
                        DateTime.Now.ToLongTimeString(), e.Flap.DatagramSequenceNumber, e.Flap.Channel,
                        string.Join(", ", infos.ToArray()));
                }
                else
                {
                    Message += string.Format("{0}: << Seq: {1} Channel: {2}\n",
                        DateTime.Now.ToLongTimeString(), e.Flap.DatagramSequenceNumber, e.Flap.Channel);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnFlapSent(object sender, FlapTransportEventArgs e)
        {
            try
            {
                var infos = e.Flap.DataItems.Select(x => x.ToString()).ToList();

                Message += string.Format("{0}: >> Seq: {1} Channel: {2} Items: {3}",
                    DateTime.Now.ToLongTimeString(), e.Flap.DatagramSequenceNumber, e.Flap.Channel,
                    string.Join(", ", infos.ToArray()));
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}