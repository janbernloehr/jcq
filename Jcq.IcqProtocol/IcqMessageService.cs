// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqMessageService.cs" company="Jan-Cornelius Molnar">
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
using Jcq.Core;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
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