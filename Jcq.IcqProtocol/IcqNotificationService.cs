// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqNotificationService.cs" company="Jan-Cornelius Molnar">
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
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.DataTypes;
using NotificationType = Jcq.IcqProtocol.Contracts.NotificationType;

namespace Jcq.IcqProtocol
{
    public class IcqNotificationService : ContextService, INotificationService
    {
        public IcqNotificationService(IContext context) : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            connector.RegisterSnacHandler(0x4, 0x14, new Action<Snac0414>(AnalyseSnac0414));
        }

        public void SendNotification(IContact receiver, NotificationType type)
        {
            var dataOut = new Snac0414 {Channel = MessageChannel.Channel1PlainText};


            switch (type)
            {
                case NotificationType.TypingCanceled:
                    dataOut.NotificationType = DataTypes.NotificationType.TextCleared;
                    break;
                case NotificationType.TypingFinished:
                    dataOut.NotificationType = DataTypes.NotificationType.TextTyped;
                    break;
                case NotificationType.TypingStarted:
                    dataOut.NotificationType = DataTypes.NotificationType.TypingBegun;
                    break;
            }

            dataOut.ScreenName = receiver.Identifier;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler<TypingNotificationEventArgs> TypingNotification;

        private void AnalyseSnac0414(Snac0414 snac)
        {
            try
            {
                NotificationType type;

                IContact contact = Context.GetService<IStorageService>().GetContactByIdentifier(snac.ScreenName);

                switch (snac.NotificationType)
                {
                    case DataTypes.NotificationType.TextTyped:
                        type = NotificationType.TypingFinished;
                        break;
                    case DataTypes.NotificationType.TypingBegun:
                        type = NotificationType.TypingStarted;
                        break;
                    default:
                        //case DataTypes.NotificationType.TextCleared:
                        type = NotificationType.TypingCanceled;
                        break;
                }

                TypingNotification?.Invoke(this, new TypingNotificationEventArgs(contact, type));
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}