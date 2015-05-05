// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqIconService.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Jcq.IcqProtocol.Internal;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.IcqInterface.Internal;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqIconService : BaseConnector, IIconService
    {
        private int _IconId;
        private UploadIconRequest _UploadIconRequest;

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

            RequestAvatarAction action;

            if ((from x in _Actions
                let y = x as RequestAvatarAction
                where y != null && y.Contact.Identifier == contact.Identifier
                select y).Any())
                return;

            action = new RequestAvatarAction(this, contact);

            AddAction(action);
        }

        public void UploadIcon(byte[] avatar)
        {
            var newIcon = default(Snac1308);
            var editIcon = default(Snac1309);

            if (_UploadIconRequest != null && !_UploadIconRequest.IsCompleted)
                return;

            _UploadIconRequest = new UploadIconRequest(avatar);

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

            if (_IconId > 0)
            {
                editIcon = new Snac1309();
                editIcon.BuddyIcon = GetSSIBudyIcon(_UploadIconRequest);
                transfer.Send(editIcon);
                _UploadIconRequest.RequestId = editIcon.RequestId;
            }
            else
            {
                newIcon = new Snac1308();
                newIcon.BuddyIcon = GetSSIBudyIcon(_UploadIconRequest);
                transfer.Send(newIcon);
                _UploadIconRequest.RequestId = newIcon.RequestId;
            }
        }

        private void AnalyseSnac0105(Snac0105 dataIn)
        {
            // Server accepts the connection request for the "Icon Server".

            try
            {
                if (IsConnected)
                    return;

                var parts = dataIn.ServerAddress.ServerAddress.Split(':');

                var ip = IPAddress.Parse(parts[0]);
                var port = 0;
                var endpoint = default(IPEndPoint);

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

                var flap = default(FlapSendSignInCookie);

                flap = new FlapSendSignInCookie();
                flap.AuthorizationCookie.AuthorizationCookie.AddRange(dataIn.AuthorizationCookie.AuthorizationCookie);

                Send(flap);

                _IsRequestingConnection = false;
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0103(Snac0103 dataIn)
        {
            // Server aks to accept service family versions

            var requiredVersions = default(List<int>);
            var notSupported = default(List<int>);

            try
            {
                requiredVersions = new List<int>(new[]
                {
                    0x1,
                    0x10
                });
                notSupported = new List<int>();

                foreach (var x in requiredVersions)
                {
                    if (!dataIn.ServerSupportedFamilyIds.Contains(x))
                    {
                        notSupported.Add(x);
                    }
                }

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

                if (_UploadIconRequest == null || _UploadIconRequest.IsCompleted)
                    return;

                if (dataIn.Notification.Type == ExtendedStatusNotificationType.UploadIconRequest)
                {
                    var notification = default(UploadIconNotification);

                    notification = (UploadIconNotification) dataIn.Notification;

                    if (notification.IconFlag == UploadIconFlag.FirstUpload)
                    {
                        Debug.WriteLine("Icon upload requested.", "IcqIconService");

                        var action = default(UploadAvatarAction);

                        action = new UploadAvatarAction(this, _UploadIconRequest.IconData);

                        AddAction(action);
                    }
                    else
                    {
                        Debug.WriteLine("Icon available.", "IcqIconService");

                        _UploadIconRequest.IsCompleted = true;

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
                _UploadIconRequest.IsCompleted = true;

                if (dataIn.IconHash.Count > 0)
                {
                    Debug.WriteLine("Icon upload succeeded.", "IcqIconService");

                    if (_UploadIconRequest != null)
                    {
                        Context.Identity.SetIconHash(new List<byte>(_UploadIconRequest.IconMd5));
                    }
                }
                else
                {
                    Debug.WriteLine("Icon upload failed.", "IcqIconService");
                }

                if (_UploadIconRequest != null)
                    _UploadIconRequest.IsCompleted = true;
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0118(Snac0118 dataIn)
        {
            var dataOut = default(Snac0106);

            try
            {
                dataOut = new Snac0106();

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

            var serverRateGroupIds = default(List<int>);
            var dataOut = default(Snac0108);

            try
            {
                serverRateGroupIds = new List<int>();

                foreach (var x in dataIn.RateGroups)
                {
                    serverRateGroupIds.Add(x.GroupId);
                }

                dataOut = new Snac0108();

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

            var c = default(IContact);

            try
            {
                c = Context.GetService<IStorageService>().GetContactByIdentifier(dataIn.Uin);

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
                    _IconId = dataIn.BuddyIcon.ItemId;
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
                if (_UploadIconRequest == null || _UploadIconRequest.IsCompleted)
                    return;
                if (_UploadIconRequest.RequestId != dataIn.RequestId)
                    return;

                var code = dataIn.ActionResultCodes.FirstOrDefault();

                if (code == SSIActionResultCode.Success)
                {
                    Debug.WriteLine("Icon upload request accepted.", "IcqIconService");
                    _UploadIconRequest.IsAccepted = true;
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

        private SSIBuddyIcon GetSSIBudyIcon(UploadIconRequest request)
        {
            var icon = new SSIBuddyIcon();
            icon.ItemId = _IconId;
            icon.BuddyIcon.IconHash.AddRange(request.IconMd5);

            return icon;
        }

        #region " Internal Action Processing "

        private bool _IsProcessing;

        private readonly Queue<IAvatarServiceAction> _Actions = new Queue<IAvatarServiceAction>();
        protected event EventHandler ServiceAvailable;

        protected void AddAction(IAvatarServiceAction action)
        {
            Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "Adding Action {0}", action);

            lock (_Actions)
            {
                _Actions.Enqueue(action);
            }

            if (!IsConnected & !_IsRequestingConnection)
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
            var action = default(IAvatarServiceAction);

            if (_IsProcessing)
                return;
            _IsProcessing = true;

            try
            {
                Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "Processing {0} Actions {1}",
                    _Actions.Count, string.Join(";", (from x in _Actions select Convert.ToString(x)).ToArray()));

                do
                {
                    lock (_Actions)
                    {
                        if (_Actions.Count > 0)
                            action = _Actions.Dequeue();
                        else
                            action = null;
                    }

                    if (action != null)
                        action.Execute();
                } while (action != null);
            }
            finally
            {
                _IsProcessing = false;
            }
        }

        #endregion

        #region " Low Level I/O "

        private bool _IsRequestingConnection;

        private void RequestConnection()
        {
            _IsRequestingConnection = true;

            var iconServiceActivation = default(Snac0104);

            iconServiceActivation = new Snac0104();
            iconServiceActivation.ServiceFamilyId = 0x10;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

            transfer.Send(iconServiceActivation);
        }

        private void OnFlapReceived(object sender, FlapTransportEventArgs e)
        {
            try
            {
                var infos = new List<string>();

                foreach (var x in e.Flap.DataItems)
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

                foreach (var x in e.Flap.DataItems)
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


    //public class IcqIconService : BaseConnector, IIconService
    //{
    //    private int _IconId;
    //    private UploadIconRequest _UploadIconRequest;

    //    public IcqIconService(IcqContext context) : base(context)
    //    {
    //        var connector = context.GetService<IConnector>() as IcqConnector;

    //        if (connector == null)
    //            throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

    //        connector.RegisterSnacHandler(0x1, 0x5, new Action<Snac0105>(AnalyseSnac0105));
    //        connector.RegisterSnacHandler(0x1, 0x21, new Action<Snac0121>(AnalyseSnac0121));
    //        connector.RegisterSnacHandler(0x13, 0x6, new Action<Snac1306>(AnalyseSnac1306));
    //        connector.RegisterSnacHandler(0x13, 0xe, new Action<Snac130E>(AnalyseSnac130E));
    //    }

    //    void IIconService.RequestContactIcon(IContact contact)
    //    {
    //        if (contact.IconHash == null)
    //            return;

    //        RequestAvatarAction action;

    //        if ((from x in _Actionsx as RequestAvatarActionwhere 
    //        y != null && y.Contact.Identifier == contact.Identifier).
    //        Any)
    //        return;

    //        action = new RequestAvatarAction(this, contact);

    //        AddAction(action);
    //    }

    //    void IIconService.UploadIcon(byte[] avatar)
    //    {
    //        Snac1308 newIcon;
    //        Snac1309 editIcon;

    //        if (_UploadIconRequest != null && !_UploadIconRequest.IsCompleted)
    //            return;

    //        _UploadIconRequest = new UploadIconRequest(avatar);

    //        IIcqDataTranferService transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

    //        if (_IconId > 0)
    //        {
    //            editIcon = new Snac1309();
    //            editIcon.BuddyIcon = GetSSIBudyIcon(_UploadIconRequest);
    //            transfer.Send(editIcon);
    //            _UploadIconRequest.RequestId = editIcon.RequestId;
    //        }
    //        else
    //        {
    //            newIcon = new Snac1308();
    //            newIcon.BuddyIcon = GetSSIBudyIcon(_UploadIconRequest);
    //            transfer.Send(newIcon);
    //            _UploadIconRequest.RequestId = newIcon.RequestId;
    //        }
    //    }

    //    private void AnalyseSnac0105(Snac0105 dataIn)
    //    {
    //        // Server accepts the connection request for the "Icon Server".

    //        try
    //        {
    //            if (IsConnected)
    //                return;

    //            var parts = dataIn.ServerAddress.ServerAddress.Split(':');

    //            var ip = IPAddress.Parse(parts(0));
    //            int port;
    //            IPEndPoint endpoint;

    //            if (parts.Length > 1)
    //            {
    //                port = int.Parse(parts(1));
    //            }
    //            else
    //            {
    //                port = 5190;
    //            }

    //            endpoint = new IPEndPoint(ip, port);

    //            InnerConnect(endpoint);

    //            this.RegisterSnacHandler<Snac0103>(0x1, 0x3, new Action<Snac0103>(AnalyseSnac0103));
    //            this.RegisterSnacHandler<Snac0118>(0x1, 0x18, new Action<Snac0118>(AnalyseSnac0118));
    //            this.RegisterSnacHandler<Snac0113>(0x1, 0x13, new Action<Snac0113>(AnalyseSnac0113));
    //            this.RegisterSnacHandler<Snac0107>(0x1, 0x7, new Action<Snac0107>(AnalyseSnac0107));
    //            this.RegisterSnacHandler(0x10, 0x3, new Action<Snac1003>(AnalyseSnac1003));
    //            this.RegisterSnacHandler<Snac1005>(0x10, 0x5, new Action<Snac1005>(AnalyseSnac1005));

    //            FlapSendSignInCookie flap;

    //            flap = new FlapSendSignInCookie();
    //            flap.AuthorizationCookie.AuthorizationCookie.AddRange(dataIn.AuthorizationCookie.AuthorizationCookie);

    //            Send(flap);

    //            _IsRequestingConnection = false;
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac0103(Snac0103 dataIn)
    //    {
    //        // Server aks to accept service family versions

    //        List<int> requiredVersions;
    //        List<int> notSupported;

    //        try
    //        {
    //            requiredVersions = new List<int>(new[]
    //            {
    //                0x1,
    //                0x10
    //            });
    //            notSupported = new List<int>();

    //            foreach (int x in requiredVersions)
    //            {
    //                if (!dataIn.ServerSupportedFamilyIds.Contains(x))
    //                {
    //                    notSupported.Add(x);
    //                }
    //            }

    //            if (notSupported.Count == 0)
    //            {
    //                var dataOut = new Snac0117();

    //                dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x1, 0x4));
    //                dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x10, 0x1));

    //                Send(dataOut);
    //            }
    //            else
    //            {
    //                throw new NotSupportedException("This server does not support all required features!");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac0121(Snac0121 dataIn)
    //    {
    //        // Server sends an extended status request. If Type = 0x01 server requests an icon upload.

    //        try
    //        {
    //            Debug.WriteLine(string.Format("Extended Status Request: {0}", dataIn.Notification.Type),
    //                "IcqIconService");

    //            if (_UploadIconRequest == null || _UploadIconRequest.IsCompleted)
    //                return;

    //            if (dataIn.Notification.Type == ExtendedStatusNotificationType.UploadIconRequest)
    //            {
    //                UploadIconNotification notification;

    //                notification = (UploadIconNotification) dataIn.Notification;

    //                if (notification.IconFlag == UploadIconFlag.FirstUpload)
    //                {
    //                    Debug.WriteLine("Icon upload requested.", "IcqIconService");

    //                    UploadAvatarAction action;

    //                    action = new UploadAvatarAction(this, _UploadIconRequest.IconData);

    //                    AddAction(action);
    //                }
    //                else
    //                {
    //                    Debug.WriteLine("Icon available.", "IcqIconService");

    //                    _UploadIconRequest.IsCompleted = true;

    //                    Context.Identity.SetIconHash(notification.IconHash);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac1003(Snac1003 dataIn)
    //    {
    //        // server acknowledges icon upload.

    //        try
    //        {
    //            _UploadIconRequest.IsCompleted = true;

    //            if (dataIn.IconHash.Count > 0)
    //            {
    //                Debug.WriteLine("Icon upload succeeded.", "IcqIconService");

    //                if (_UploadIconRequest != null)
    //                {
    //                    Context.Identity.SetIconHash(new List<byte>(_UploadIconRequest.IconMd5));
    //                }
    //            }
    //            else
    //            {
    //                Debug.WriteLine("Icon upload failed.", "IcqIconService");
    //            }

    //            if (_UploadIconRequest != null)
    //                _UploadIconRequest.IsCompleted = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac0118(Snac0118 dataIn)
    //    {
    //        Snac0106 dataOut;

    //        try
    //        {
    //            dataOut = new Snac0106();

    //            Send(dataOut);
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac0113(Snac0113 dataIn)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private void AnalyseSnac0107(Snac0107 dataIn)
    //    {
    //        // Server accepted the rate configuration.
    //        // The connection now can be used.

    //        List<int> serverRateGroupIds;
    //        Snac0108 dataOut;

    //        try
    //        {
    //            serverRateGroupIds = new List<int>();

    //            foreach (var x in dataIn.RateGroups)
    //            {
    //                serverRateGroupIds.Add(x.GroupId);
    //            }

    //            dataOut = new Snac0108();

    //            dataOut.RateGroupIds.AddRange(serverRateGroupIds);

    //            Send(dataOut);

    //            if (ServiceAvailable != null)
    //            {
    //                ServiceAvailable(this, EventArgs.Empty);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac1005(Snac1005 dataIn)
    //    {
    //        // Received Icon data.

    //        IContact c;

    //        try
    //        {
    //            c = Context.GetService<IStorageService>.GetContactByIdentifier(dataIn.Uin);

    //            if (dataIn.IconData.Count > 0)
    //            {
    //                Debug.WriteLine(string.Format("Received Icon for {0}.", c.Identifier), "IcqIconService");

    //                c.SetIconData(dataIn.IconData);
    //            }
    //            else
    //            {
    //                Debug.WriteLine(string.Format("Receive Icon for {0} failed.", c.Identifier), "IcqIconService");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac1306(Snac1306 dataIn)
    //    {
    //        // The Server sent the buddy list.
    //        // Grab the Icon id to allow updates.

    //        try
    //        {
    //            if (dataIn.BuddyIcon != null)
    //            {
    //                _IconId = dataIn.BuddyIcon.ItemId;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void AnalyseSnac130E(Snac130E dataIn)
    //    {
    //        // Server akknowledges the icon upload request.
    //        // It will ask the client to upload the icon with Snac 01,21.

    //        try
    //        {
    //            if (_UploadIconRequest == null || _UploadIconRequest.IsCompleted)
    //                return;
    //            if (_UploadIconRequest.RequestId != dataIn.RequestId)
    //                return;

    //            object code = dataIn.ActionResultCodes.FirstOrDefault;

    //            if (code == SSIActionResultCode.Success)
    //            {
    //                Debug.WriteLine("Icon upload request accepted.", "IcqIconService");
    //                _UploadIconRequest.IsAccepted = true;
    //            }
    //            else
    //            {
    //                Debug.WriteLine(string.Format("Icon upload request rejected: {0}.", code), "IcqIconService");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private SSIBuddyIcon GetSSIBudyIcon(UploadIconRequest request)
    //    {
    //        SSIBuddyIcon icon;

    //        icon = new SSIBuddyIcon();
    //        icon.ItemId = _IconId;
    //        icon.BuddyIcon.IconHash.AddRange(request.IconMd5);

    //        return icon;
    //    }

    //    #region  Internal Action Processing 

    //    private bool _IsProcessing;

    //    private readonly Queue<IAvatarServiceAction> _Actions = new Queue<IAvatarServiceAction>();

    //    protected event EventHandler ServiceAvailable;

    //    protected void AddAction(IAvatarServiceAction action)
    //    {
    //        Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "Adding Action {0}", action);

    //        lock (_Actions)
    //        {
    //            _Actions.Enqueue(action);
    //        }

    //        if (!IsConnected & !_IsRequestingConnection)
    //        {
    //            RequestConnection();
    //        }
    //        else
    //        {
    //            ProcessActions();
    //        }
    //    }

    //    protected bool IsAvailable { get; private set; }

    //    private void // ERROR: Handles clauses are not supported in C#
    //        OnServiceAvailable(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            IsAvailable = true;
    //            TcpContext.SetCloseUnexpected();

    //            ProcessActions();

    //            TcpContext.SetCloseExpected();
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    protected void ProcessActions()
    //    {
    //        IAvatarServiceAction action;

    //        if (_IsProcessing)
    //            return;
    //        _IsProcessing = true;

    //        try
    //        {
    //            Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "Processing {0} Actions {1}",
    //                _Actions.Count, string.Join(';', (from x in _ActionsConvert.ToString(x) ).ToArray));

    //            do
    //            {
    //                lock (_Actions)
    //                {
    //                    if (_Actions.Count > 0)
    //                        action = _Actions.Dequeue;
    //                    else
    //                        action = null;
    //                }

    //                if (action != null)
    //                    action.Execute();
    //            } while (action != null);
    //        }
    //        finally
    //        {
    //            _IsProcessing = false;
    //        }
    //    }

    //    #endregion

    //    #region  Low Level I/O 

    //    private bool _IsRequestingConnection;

    //    private void RequestConnection()
    //    {
    //        _IsRequestingConnection = true;

    //        Snac0104 iconServiceActivation;

    //        iconServiceActivation = new Snac0104();
    //        iconServiceActivation.ServiceFamilyId = 0x10;

    //        IIcqDataTranferService transfer = (IIcqDataTranferService) Context.GetService<IConnector>();

    //        transfer.Send(iconServiceActivation);
    //    }

    //    private void // ERROR: Handles clauses are not supported in C#
    //        OnFlapReceived(object sender, FlapTransportEventArgs e)
    //    {
    //        try
    //        {
    //            List<string> infos = new List<string>();

    //            foreach (var x in e.Flap.DataItems)
    //            {
    //                infos.Add(x.ToString());
    //            }

    //            Kernel.Logger.Log("IcqIconService", TraceEventType.Information, "<< Seq: {0} Channel: {1} Items: {2}",
    //                e.Flap.DatagramSequenceNumber, e.Flap.Channel, string.Join(", ", infos.ToArray()));
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    private void // ERROR: Handles clauses are not supported in C#
    //        OnFlapSent(object sender, IcqInterface.FlapTransportEventArgs e)
    //    {
    //        try
    //        {
    //            List<string> infos = new List<string>();

    //            foreach (ISerializable x in e.Flap.DataItems)
    //            {
    //                infos.Add(((object) x).ToString);
    //            }

    //            Kernel.Logger.Log("IcqIconService", TraceEventType.Information, ">> Seq: {0} Channel: {1} Items: {2}",
    //                e.Flap.DatagramSequenceNumber, e.Flap.Channel, string.Join(", ", infos.ToArray));
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    #endregion
    //}
}