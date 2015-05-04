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
  public class IcqMessageService : ContextService, Interfaces.IMessageService, Interfaces.IOfflineMessageService
  {
    public event MessageReceivedEventHandler MessageReceived;
    public delegate void MessageReceivedEventHandler(object sender, Interfaces.MessageEventArgs e);

    public IcqMessageService(Interfaces.IContext context) : base(context)
    {

      IcqConnector connector = context.GetService<Interfaces.IConnector>() as IcqConnector;

      if (connector == null)
        throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

      connector.RegisterSnacHandler<DataTypes.Snac0407>(0x4, 0x7, new Action<DataTypes.Snac0407>(AnalyseSnac0407));
      connector.RegisterSnacHandler<DataTypes.Snac1503>(0x15, 0x3, new Action<DataTypes.Snac1503>(AnalyseSnac1503));
    }

    private void AnalyseSnac0407(DataTypes.Snac0407 snac)
    {
      IcqMessage msg;

      string senderId;
      string messageText;

      Interfaces.IContact sender;
      Interfaces.IContact recipient;

      try {
        senderId = snac.ScreenName;
        messageText = snac.MessageData.MessageText;

        sender = Context.GetService<Interfaces.IStorageService>.GetContactByIdentifier(senderId);
        recipient = Context.Identity;

        msg = new IcqMessage(sender, recipient, messageText);

        if (MessageReceived != null) {
          MessageReceived(this, new Interfaces.MessageEventArgs(msg));
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void AnalyseSnac1503(DataTypes.Snac1503 snac)
    {
      IcqOfflineMessage msg;

      string senderId;
      string messageText;

      Interfaces.IContact sender;
      Interfaces.IContact recipient;

      try {
        switch (snac.MetaData.MetaResponse.ResponseType) {
          case DataTypes.MetaResponseType.OfflineMessageResponse:
            DataTypes.OfflineMessageResponse resp = (DataTypes.OfflineMessageResponse)snac.MetaData.MetaResponse;

            senderId = (string)resp.ClientUin;
            messageText = resp.MessageText;

            sender = Context.GetService<Interfaces.IStorageService>.GetContactByIdentifier(senderId);
            recipient = Context.Identity;

            msg = new IcqOfflineMessage(sender, recipient, messageText, resp.DateSent);

            if (MessageReceived != null) {
              MessageReceived(this, new Interfaces.MessageEventArgs(msg));
            }


            break;
          case DataTypes.MetaResponseType.EndOfOfflineMessageResponse:
            if (AllOfflineMessagesReceived != null) {
              AllOfflineMessagesReceived(this, System.EventArgs.Empty);
            }

            break;
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public void Interfaces.IMessageService.SendMessage(Interfaces.IMessage message)
    {
      DataTypes.Snac0406 dataOut;

      dataOut = new DataTypes.Snac0406();
      dataOut.Channel = DataTypes.MessageChannel.Channel1PlainText;
      dataOut.CookieID = System.Environment.TickCount;
      dataOut.RequestAnAckFromServer = false;
      dataOut.ScreenName = message.Recipient.Identifier;
      dataOut.StoreMessageIfRecipientIsOffline = true;

      dataOut.MessageData.MessageText = message.Text;

      IIcqDataTranferService transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }

    public void Interfaces.IOfflineMessageService.DeleteOfflineMessages()
    {
      DataTypes.Snac1502 dataOut;

      dataOut = new DataTypes.Snac1502();

      DataTypes.DeleteOfflineMessagesRequest req;
      req = new DataTypes.DeleteOfflineMessagesRequest();
      req.ClientUin = (long)Context.Identity.Identifier;

      dataOut.MetaData.MetaRequest = req;

      IIcqDataTranferService transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }

    public void Interfaces.IOfflineMessageService.RequestOfflineMessages()
    {
      DataTypes.Snac1502 dataOut;

      dataOut = new DataTypes.Snac1502();

      DataTypes.OfflineMessageRequest req;
      req = new DataTypes.OfflineMessageRequest();
      req.ClientUin = (long)Context.Identity.Identifier;

      dataOut.MetaData.MetaRequest = req;

      IIcqDataTranferService transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }

    public event AllOfflineMessagesReceivedEventHandler AllOfflineMessagesReceived;
    public delegate void AllOfflineMessagesReceivedEventHandler(object sender, System.EventArgs e);
  }

}

