// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqIconService.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Jcq.Core;
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Internal;

namespace Jcq.IcqProtocol
{
    public class IcqIconService : BaseConnector, IIconService
    {
        private int _iconId;
        private UploadIconRequest _uploadIconRequest;

        public IcqIconService(IcqContext context)
            : base(context)
        {
            FlapSent += OnFlapSent;
            FlapReceived += OnFlapReceived;
            ServiceAvailable += OnServiceAvailable;

            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            connector.RegisterSnacHandler(0x1, 0x5, new Action<Snac0105>(AnalyseSnac0105));
            connector.RegisterSnacHandler(0x1, 0x21, new Action<Snac0121>(AnalyseSnac0121));
            connector.RegisterSnacHandler(0x13, 0x6, new Action<Snac1306>(AnalyseSnac1306));
            connector.RegisterSnacHandler(0x13, 0xe, new Action<Snac130E>(AnalyseSnac130E));
        }

        public void RequestContactIcon(IContact contact)
        {
            if (contact.IconHash == null)
                return;

            if ((from x in _actions
                let y = x as RequestAvatarAction
                where y != null && y.Contact.Identifier == contact.Identifier
                select y).Any())
                return;

            var action = new RequestAvatarAction(this, contact);

            AddAction(action);
        }

        public void UploadIcon(byte[] avatar)
        {
            return;

            Snac1308 newIcon = default(Snac1308);
            Snac1309 editIcon = default(Snac1309);

            if (_uploadIconRequest != null && !_uploadIconRequest.IsCompleted)
                return;

            _uploadIconRequest = new UploadIconRequest(avatar);

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

            if (_iconId > 0)
            {
                editIcon = new Snac1309();
                editIcon.BuddyIcon = GetSsiBudyIcon(_uploadIconRequest);
                transfer.Send(editIcon);
                _uploadIconRequest.RequestId = editIcon.RequestId;
            }
            newIcon = new Snac1308();
            newIcon.BuddyIcon = GetSsiBudyIcon(_uploadIconRequest);
            transfer.Send(newIcon);
            _uploadIconRequest.RequestId = newIcon.RequestId;
        }

        private void AnalyseSnac0105(Snac0105 dataIn)
        {
            // Server accepts the connection request for the "Icon Server".

            try
            {
                if (IsConnected)
                    return;

                var parts = dataIn.ServerAddress.ServerAddress.Split(':');

                IPAddress ip = IPAddress.Parse(parts[0]);
                int port = 0;
                IPEndPoint endpoint = default(IPEndPoint);

                if (parts.Length > 1)
                {
                    port = int.Parse(parts[1]);
                }
                else
                {
                    port = 5190;
                }

                endpoint = new IPEndPoint(ip, port);

                InnerConnect(endpoint);

                RegisterSnacHandler(0x1, 0x3, new Action<Snac0103>(AnalyseSnac0103));
                RegisterSnacHandler(0x1, 0x18, new Action<Snac0118>(AnalyseSnac0118));
                RegisterSnacHandler(0x1, 0x13, new Action<Snac0113>(AnalyseSnac0113));
                RegisterSnacHandler(0x1, 0x7, new Action<Snac0107>(AnalyseSnac0107));
                RegisterSnacHandler(0x10, 0x3, new Action<Snac1003>(AnalyseSnac1003));
                RegisterSnacHandler(0x10, 0x5, new Action<Snac1005>(AnalyseSnac1005));

                FlapSendSignInCookie flap = default(FlapSendSignInCookie);

                flap = new FlapSendSignInCookie();
                flap.AuthorizationCookie.AuthorizationCookie.AddRange(dataIn.AuthorizationCookie.AuthorizationCookie);

                Send(flap);

                _isRequestingConnection = false;
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0103(Snac0103 dataIn)
        {
            // Server aks to accept service family versions

            try
            {
                var requiredVersions = new List<int>(new[]
                {
                    0x1,
                    0x10
                });

                var notSupported = requiredVersions.Except(dataIn.ServerSupportedFamilyIds).ToList();

                if (notSupported.Count == 0)
                {
                    var dataOut = new Snac0117();

                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x1, 0x4));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x10, 0x1));

                    Send(dataOut);
                }
                else
                {
                    throw new NotSupportedException("This server does not support all required features!");
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0121(Snac0121 dataIn)
        {
            // Server sends an extended status request. If Type = 0x01 server requests an icon upload.

            try
            {
                Debug.WriteLine(string.Format("Extended Status Request: {0}", dataIn.Notification.Type),
                    "IcqIconService");

                if (_uploadIconRequest == null || _uploadIconRequest.IsCompleted)
                    return;

                if (dataIn.Notification.Type == ExtendedStatusNotificationType.UploadIconRequest)
                {
                    UploadIconNotification notification = default(UploadIconNotification);

                    notification = (UploadIconNotification) dataIn.Notification;

                    if (notification.IconFlag == UploadIconFlag.FirstUpload)
                    {
                        Debug.WriteLine("Icon upload requested.", "IcqIconService");

                        UploadAvatarAction action = default(UploadAvatarAction);

                        action = new UploadAvatarAction(this, _uploadIconRequest.IconData);

                        AddAction(action);
                    }
                    else
                    {
                        Debug.WriteLine("Icon available.", "IcqIconService");

                        _uploadIconRequest.IsCompleted = true;

                        Context.Identity.SetIconHash(notification.IconHash);
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac1003(Snac1003 dataIn)
        {
            // server acknowledges icon upload.

            try
            {
                _uploadIconRequest.IsCompleted = true;

                if (dataIn.IconHash.Count > 0)
                {
                    Debug.WriteLine("Icon upload succeeded.", "IcqIconService");

                    if (_uploadIconRequest != null)
                    {
                        Context.Identity.SetIconHash(new List<byte>(_uploadIconRequest.IconMd5));
                    }
                }
                else
                {
                    Debug.WriteLine("Icon upload failed.", "IcqIconService");
                }

                if (_uploadIconRequest != null)
                    _uploadIconRequest.IsCompleted = true;
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0118(Snac0118 dataIn)
        {
            try
            {
                var dataOut = new Snac0106();

                Send(dataOut);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0113(Snac0113 dataIn)
        {
            throw new NotImplementedException();
        }

        private void AnalyseSnac0107(Snac0107 dataIn)
        {
            // Server accepted the rate configuration.
            // The connection now can be used.

            try
            {
                var serverRateGroupIds = dataIn.RateGroups.Select(x => x.GroupId).ToList();

                var dataOut = new Snac0108();

                dataOut.RateGroupIds.AddRange(serverRateGroupIds);

                Send(dataOut);

                if (ServiceAvailable != null)
                {
                    ServiceAvailable(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac1005(Snac1005 dataIn)
        {
            // Received Icon data.

            try
            {
                IContact c = Context.GetService<IStorageService>().GetContactByIdentifier(dataIn.Uin);

                if (dataIn.IconData.Count > 0)
                {
                    Debug.WriteLine(string.Format("Received Icon for {0}.", c.Identifier), "IcqIconService");

                    c.SetIconData(dataIn.IconData);
                }
                else
                {
                    Debug.WriteLine(string.Format("Receive Icon for {0} failed.", c.Identifier), "IcqIconService");
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac1306(Snac1306 dataIn)
        {
            // The Server sent the buddy list.
            // Grab the Icon id to allow updates.

            try
            {
                if (dataIn.BuddyIcon != null)
                {
                    _iconId = dataIn.BuddyIcon.ItemId;
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac130E(Snac130E dataIn)
        {
            // Server akknowledges the icon upload request.
            // It will ask the client to upload the icon with Snac 01,21.

            try
            {
                if (_uploadIconRequest == null || _uploadIconRequest.IsCompleted)
                    return;
                if (_uploadIconRequest.RequestId != dataIn.RequestId)
                    return;

                SSIActionResultCode code = dataIn.ActionResultCodes.FirstOrDefault();

                if (code == SSIActionResultCode.Success)
                {
                    Debug.WriteLine("Icon upload request accepted.", "IcqIconService");
                    _uploadIconRequest.IsAccepted = true;
                }
                else
                {
                    Debug.WriteLine(string.Format("Icon upload request rejected: {0}.", code), "IcqIconService");
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private SSIBuddyIcon GetSsiBudyIcon(UploadIconRequest request)
        {
            var icon = new SSIBuddyIcon
            {
                ItemId = _iconId
            };

            icon.BuddyIcon.IconHash.AddRange(request.IconMd5);

            return icon;
        }

        #region " Internal Action Processing "

        private bool _isProcessing;

        private readonly Queue<IAvatarServiceAction> _actions = new Queue<IAvatarServiceAction>();
        protected event EventHandler ServiceAvailable;

        protected void AddAction(IAvatarServiceAction action)
        {
            Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "Adding Action {0}", action);

            lock (_actions)
            {
                _actions.Enqueue(action);
            }

            if (!IsConnected & !_isRequestingConnection)
            {
                RequestConnection();
            }
            else
            {
                ProcessActions();
            }
        }

        protected bool IsAvailable { get; private set; }

        private void OnServiceAvailable(object sender, EventArgs e)
        {
            try
            {
                IsAvailable = true;
                TcpContext.SetCloseUnexpected();

                ProcessActions();

                TcpContext.SetCloseExpected();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void ProcessActions()
        {
            if (_isProcessing)
                return;
            _isProcessing = true;

            try
            {
                Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "Processing {0} Actions {1}",
                    _actions.Count, string.Join(";", (from x in _actions select Convert.ToString(x)).ToArray()));

                IAvatarServiceAction action;

                do
                {
                    lock (_actions)
                    {
                        action = _actions.Count > 0 ? _actions.Dequeue() : null;
                    }

                    if (action != null)
                        action.Execute();
                } while (action != null);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        #endregion

        #region " Low Level I/O "

        private bool _isRequestingConnection;

        private void RequestConnection()
        {
            _isRequestingConnection = true;

            var iconServiceActivation = new Snac0104 {ServiceFamilyId = 0x10};

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

            transfer.Send(iconServiceActivation);
        }

        private void OnFlapReceived(object sender, FlapTransportEventArgs e)
        {
            try
            {
                var infos = new List<string>();

                foreach (ISerializable x in e.Flap.DataItems)
                {
                    infos.Add(x.ToString());
                }

                Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "<< Seq: {0} Channel: {1} Items: {2}",
                    e.Flap.DatagramSequenceNumber, e.Flap.Channel, string.Join(", ", infos.ToArray()));
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
                var infos = new List<string>();

                foreach (ISerializable x in e.Flap.DataItems)
                {
                    infos.Add(x.ToString());
                }

                Kernel.Logger.Log("IcqIconService", TraceEventType.Information, ">> Seq: {0} Channel: {1} Items: {2}",
                    e.Flap.DatagramSequenceNumber, e.Flap.Channel, string.Join(", ", infos.ToArray()));
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        #endregion
    }
}