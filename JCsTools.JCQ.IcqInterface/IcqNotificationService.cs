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
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqNotificationService : ContextService, Interfaces.INotificationService
  {
    public IcqNotificationService(Interfaces.IContext context) : base(context)
    {

      IcqConnector connector = context.GetService<Interfaces.IConnector>() as IcqConnector;

      if (connector == null)
        throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

      connector.RegisterSnacHandler<DataTypes.Snac0414>(0x4, 0x14, new Action<DataTypes.Snac0414>(AnalyseSnac0414));
    }

    private void AnalyseSnac0414(DataTypes.Snac0414 snac)
    {
      try {
        Interfaces.IContact contact;
        Interfaces.NotificationType type;

        contact = Context.GetService<Interfaces.IStorageService>.GetContactByIdentifier(snac.ScreenName);

        switch (snac.NotificationType) {
          case DataTypes.NotificationType.TextCleared:
            type = Interfaces.NotificationType.TypingCanceled;
            break;
          case DataTypes.NotificationType.TextTyped:
            type = Interfaces.NotificationType.TypingFinished;
            break;
          case DataTypes.NotificationType.TypingBegun:
            type = Interfaces.NotificationType.TypingStarted;
            break;
        }

        if (TypingNotificationEventArgs != null) {
          TypingNotificationEventArgs(this, new Interfaces.TypingNotificationEventArgs(contact, type));
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public void Interfaces.INotificationService.SendNotification(Interfaces.IContact receiver, Interfaces.NotificationType type)
    {
      DataTypes.Snac0414 dataOut;

      dataOut = new DataTypes.Snac0414();

      dataOut.Channel = DataTypes.MessageChannel.Channel1PlainText;

      switch (type) {
        case Interfaces.NotificationType.TypingCanceled:
          dataOut.NotificationType = DataTypes.NotificationType.TextCleared;
          break;
        case Interfaces.NotificationType.TypingFinished:
          dataOut.NotificationType = DataTypes.NotificationType.TextTyped;
          break;
        case Interfaces.NotificationType.TypingStarted:
          dataOut.NotificationType = DataTypes.NotificationType.TypingBegun;
          break;
      }

      dataOut.ScreenName = receiver.Identifier;

      IIcqDataTranferService transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }

    public event TypingNotificationEventArgsEventHandler TypingNotificationEventArgs;
    public delegate void TypingNotificationEventArgsEventHandler(object sender, Interfaces.TypingNotificationEventArgs e);
  }
}

