// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferWindowViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using Jcq.Core;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.Internal;

namespace Jcq.Ux.ViewModel
{
    public class TransferWindowViewModel : ViewModelBase
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
                OnPropertyChanged();
            }
        }

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

                Message += string.Format("{0}: >> Seq: {1} Channel: {2} Items: {3}\n",
                    DateTime.Now.ToLongTimeString(), e.Flap.DatagramSequenceNumber, e.Flap.Channel,
                    string.Join(", ", infos.ToArray()));
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}