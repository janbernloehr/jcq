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
namespace JCsTools.JCQ.Ux
{
  public class MessageFormattingService : Core.Service, IMessageFormattingService
  {
    public System.Windows.Documents.Paragraph IMessageFormattingService.FormatMessage(MessageSenderRole role, IcqInterface.Interfaces.IMessage message)
    {
      Paragraph messageContainer;

      Run prefix;
      Run content;
      Run suffix;

      messageContainer = new Paragraph();
      messageContainer.Margin = new System.Windows.Thickness(0);

      prefix = new Run();

      switch (role) {
        case MessageSenderRole.ContextIdentity:
          prefix.Foreground = Brushes.Blue;
          break;
        case MessageSenderRole.FirstAtt:
          prefix.Foreground = Brushes.Red;
          break;
        case MessageSenderRole.SecondAtt:
          prefix.Foreground = Brushes.DarkGreen;
          break;
        case MessageSenderRole.ThirdAtt:
          prefix.Foreground = Brushes.Orange;
          break;
      }

      prefix.Text = string.Format("{0} ~ {1}: ", System.DateTime.Now.ToShortTimeString, message.Sender.Name);

      content = new Run();

      content.Text = message.Text;

      messageContainer.Inlines.Add(prefix);
      messageContainer.Inlines.Add(content);

      if (message is IcqInterface.IcqOfflineMessage) {
        suffix = new Run();

        suffix.Text = string.Format(" [Offline Message {0}]", ((IcqInterface.IcqOfflineMessage)message).OfflineSentDate);

        messageContainer.Inlines.Add(suffix);
      }

      return messageContainer;
    }

    public string[] IMessageFormattingService.GetTextLinesFromDocument(System.Windows.Documents.FlowDocument document)
    {
      List<string> lines;
      System.Text.StringBuilder writer;

      lines = new List<string>();

      foreach (Paragraph par in document.Blocks) {
        writer = new System.Text.StringBuilder();

        foreach (Inline inline in par.Inlines) {
          Run text = inline as Run;

          if (text != null) {
            writer.Append(text.Text);
          }
        }

        lines.Add(writer.ToString);
      }

      return lines.ToArray;
    }

    public System.Windows.Documents.Paragraph IMessageFormattingService.FormatStatus(ContactViewModel contact, IcqInterface.Interfaces.IStatusCode status)
    {
      Paragraph statusContainer;
      Run content;

      statusContainer = new Paragraph();
      statusContainer.Margin = new System.Windows.Thickness(0);

      content = new Run();

      content.Foreground = System.Windows.Media.Brushes.Gray;

      content.Text = string.Format("{0} ~ {1} changed his/her status to {2}", System.DateTime.Now.ToShortTimeString, contact.Name, status.DisplayName);

      statusContainer.Inlines.Add(content);

      return statusContainer;
    }
  }
}

