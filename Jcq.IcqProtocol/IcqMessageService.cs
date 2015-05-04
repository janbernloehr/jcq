// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqMessageService.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqMessageService : ContextService, IMessageService, IOfflineMessageService
    {
        public IcqMessageService(IContext context) : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            connector.RegisterSnacHandler(0x4, 0x7, new Action<Snac0407>(AnalyseSnac0407));
            connector.RegisterSnacHandler(0x15, 0x3, new Action<Snac1503>(AnalyseSnac1503));
        }

        void IMessageService.SendMessage(IMessage message)
        {
            Snac0406 dataOut;

            dataOut = new Snac0406();
            dataOut.Channel = MessageChannel.Channel1PlainText;
            dataOut.CookieID = Environment.TickCount;
            dataOut.RequestAnAckFromServer = false;
            dataOut.ScreenName = message.Recipient.Identifier;
            dataOut.StoreMessageIfRecipientIsOffline = true;

            dataOut.MessageData.MessageText = message.Text;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler<MessageEventArgs> MessageReceived;

        void IOfflineMessageService.DeleteOfflineMessages()
        {
            Snac1502 dataOut;

            dataOut = new Snac1502();

            DeleteOfflineMessagesRequest req;
            req = new DeleteOfflineMessagesRequest();
            req.ClientUin = long.Parse(Context.Identity.Identifier);

            dataOut.MetaData.MetaRequest = req;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        void IOfflineMessageService.RequestOfflineMessages()
        {
            Snac1502 dataOut;

            dataOut = new Snac1502();

            OfflineMessageRequest req;
            req = new OfflineMessageRequest();
            req.ClientUin = long.Parse(Context.Identity.Identifier);

            dataOut.MetaData.MetaRequest = req;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler AllOfflineMessagesReceived;

        private void AnalyseSnac0407(Snac0407 snac)
        {
            IcqMessage msg;

            string senderId;
            string messageText;

            IContact sender;
            IContact recipient;

            try
            {
                senderId = snac.ScreenName;
                messageText = snac.MessageData.MessageText;

                sender = Context.GetService<IStorageService>().GetContactByIdentifier(senderId);
                recipient = Context.Identity;

                msg = new IcqMessage(sender, recipient, messageText);

                if (MessageReceived != null)
                {
                    MessageReceived(this, new MessageEventArgs(msg));
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac1503(Snac1503 snac)
        {
            IcqOfflineMessage msg;

            string senderId;
            string messageText;

            IContact sender;
            IContact recipient;

            try
            {
                switch (snac.MetaData.MetaResponse.ResponseType)
                {
                    case MetaResponseType.OfflineMessageResponse:
                        var resp = (OfflineMessageResponse) snac.MetaData.MetaResponse;

                        senderId = resp.ClientUin.ToString();
                        messageText = resp.MessageText;

                        sender = Context.GetService<IStorageService>().GetContactByIdentifier(senderId);
                        recipient = Context.Identity;

                        msg = new IcqOfflineMessage(sender, recipient, messageText, resp.DateSent);

                        if (MessageReceived != null)
                        {
                            MessageReceived(this, new MessageEventArgs(msg));
                        }


                        break;
                    case MetaResponseType.EndOfOfflineMessageResponse:
                        if (AllOfflineMessagesReceived != null)
                        {
                            AllOfflineMessagesReceived(this, EventArgs.Empty);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}