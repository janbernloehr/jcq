// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqNotificationService.cs" company="Jan-Cornelius Molnar">
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
using NotificationType = JCsTools.JCQ.IcqInterface.Interfaces.NotificationType;

namespace JCsTools.JCQ.IcqInterface
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
            Snac0414 dataOut;

            dataOut = new Snac0414();

            dataOut.Channel = MessageChannel.Channel1PlainText;

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
                IContact contact;
                NotificationType type;

                contact = Context.GetService<IStorageService>().GetContactByIdentifier(snac.ScreenName);

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

                if (TypingNotification != null)
                {
                    TypingNotification(this, new TypingNotificationEventArgs(contact, type));
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}