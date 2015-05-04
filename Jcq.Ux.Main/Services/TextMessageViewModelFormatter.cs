// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextMessageViewModelFormatter.cs" company="Jan-Cornelius Molnar">
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
using System.Xml;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;
using JCsTools.Xml.Formatter;

namespace JCsTools.JCQ.Ux
{
    public class TextMessageViewModelFormatter : DefaultReferenceFormatter
    {
        public TextMessageViewModelFormatter(ISerializer parent)
            : base(parent, typeof (TextMessageViewModel), false, false)
        {
        }

        protected override object CreateObject(Type type, XmlReader reader)
        {
            DateTime dateCreated;
            string senderIdentifier;
            string recipientIdentifier;
            string message;

            IContact senderModel;
            IContact recipientModel;

            ContactViewModel sender;
            ContactViewModel recipient;

            var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();

            dateCreated = XmlConvert.ToDateTime(reader.GetAttribute("created"), XmlDateTimeSerializationMode.Utc);
            senderIdentifier = reader.GetAttribute("sender");
            recipientIdentifier = reader.GetAttribute("recipient");
            message = reader.GetAttribute("message");

            senderModel = svStorage.GetContactByIdentifier(senderIdentifier);
            recipientModel = svStorage.GetContactByIdentifier(recipientIdentifier);

            sender = ContactViewModelCache.GetViewModel(senderModel);
            recipient = ContactViewModelCache.GetViewModel(recipientModel);

            return new TextMessageViewModel(dateCreated, sender, recipient, message, MessageColors.HistoryColor);
        }

        protected override void DeserializeProperties(object graph, XmlReader reader)
        {
            // Nothing to do here
        }

        protected override void SerializeProperties(object graph, XmlWriter writer)
        {
            var entity = (TextMessageViewModel) graph;

            writer.WriteAttributeString("created",
                XmlConvert.ToString(entity.DateCreated, XmlDateTimeSerializationMode.Utc));
            writer.WriteAttributeString("sender", entity.Sender.Identifier);
            writer.WriteAttributeString("recipient", entity.Recipient.Identifier);
            writer.WriteAttributeString("message", entity.Message);
        }
    }
}