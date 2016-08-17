// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageFormattingService.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jcq.Core;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.Ux.ViewModel;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.Main.Services
{
    public class MessageFormattingService : Service, IMessageFormattingService
    {
        public Paragraph FormatMessage(MessageSenderRole role, IMessage message)
        {
            var messageContainer = new Paragraph
            {
                Margin = new Thickness(0)
            };

            var prefix = new Run();

            switch (role)
            {
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

            prefix.Text = string.Format("{0} ~ {1}: ", DateTime.Now.ToShortTimeString(), message.Sender.Name);

            var content = new Run {Text = message.Text};

            messageContainer.Inlines.Add(prefix);
            messageContainer.Inlines.Add(content);

            var offlineMessage = message as IcqOfflineMessage;

            if (offlineMessage != null)
            {
                var suffix = new Run {Text = string.Format(" [Offline Message {0}]", offlineMessage.OfflineSentDate)};

                messageContainer.Inlines.Add(suffix);
            }

            return messageContainer;
        }

        public string[] GetTextLinesFromDocument(FlowDocument document)
        {
            var lines = new List<string>();

            foreach (Paragraph par in document.Blocks.Cast<Paragraph>())
            {
                var writer = new StringBuilder();

                foreach (Run text in par.Inlines.OfType<Run>())
                {
                    writer.Append(text.Text);
                }

                lines.Add(writer.ToString());
            }

            return lines.ToArray();
        }

        public Paragraph FormatStatus(ContactViewModel contact, IStatusCode status)
        {
            var statusContainer = new Paragraph
            {
                Margin = new Thickness(0)
            };

            var content = new Run
            {
                Foreground = Brushes.Gray,
                Text = string.Format("{0} ~ {1} changed his/her status to {2}", DateTime.Now.ToShortTimeString(),
                    contact.Name, status.DisplayName)
            };

            statusContainer.Inlines.Add(content);

            return statusContainer;
        }
    }
}