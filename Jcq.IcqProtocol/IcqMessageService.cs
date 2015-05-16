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
            var dataOut = new Snac0406
            {
                Channel = MessageChannel.Channel1PlainText,
                CookieID = Environment.TickCount,
                RequestAnAckFromServer = false,
                ScreenName = message.Recipient.Identifier,
                StoreMessageIfRecipientIsOffline = true
            };

            dataOut.MessageData.MessageText = message.Text;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler<MessageEventArgs> MessageReceived;

        void IOfflineMessageService.DeleteOfflineMessages()
        {
            var dataOut = new Snac1502();

            var req = new DeleteOfflineMessagesRequest {ClientUin = long.Parse(Context.Identity.Identifier)};

            dataOut.MetaData.MetaRequest = req;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        void IOfflineMessageService.RequestOfflineMessages()
        {
            var dataOut = new Snac1502();

            var req = new OfflineMessageRequest {ClientUin = long.Parse(Context.Identity.Identifier)};

            dataOut.MetaData.MetaRequest = req;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler AllOfflineMessagesReceived;

        private void AnalyseSnac0407(Snac0407 snac)
        {
            try
            {
                var senderId = snac.ScreenName;
                var messageText = snac.MessageData.MessageText;

                var sender = Context.GetService<IStorageService>().GetContactByIdentifier(senderId);
                var recipient = Context.Identity;

                var msg = new IcqMessage(sender, recipient, messageText);

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
            try
            {
                switch (snac.MetaData.MetaResponse.ResponseType)
                {
                    case MetaResponseType.OfflineMessageResponse:
                        var resp = (OfflineMessageResponse) snac.MetaData.MetaResponse;

                        var senderId = resp.ClientUin.ToString();
                        var messageText = resp.MessageText;

                        var sender = Context.GetService<IStorageService>().GetContactByIdentifier(senderId);
                        var recipient = Context.Identity;

                        var msg = new IcqOfflineMessage(sender, recipient, messageText, resp.DateSent);

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