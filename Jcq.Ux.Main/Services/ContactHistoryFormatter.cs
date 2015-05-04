// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactHistoryFormatter.cs" company="Jan-Cornelius Molnar">
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
    public class ContactHistoryFormatter : DefaultIListReferenceFormatter
    {
        public ContactHistoryFormatter(ISerializer context) : base(context, typeof (ContactHistory))
        {
        }

        protected override object CreateObject(Type type, XmlReader reader)
        {
            string identifier;
            IContact contact;

            identifier = reader.GetAttribute("contact");
            contact = ApplicationService.Current.Context.GetService<IStorageService>()
                .GetContactByIdentifier(identifier);

            return new ContactHistory(contact);
        }

        public override void Serialize(object graph, XmlWriter writer)
        {
            var history = (ContactHistory) graph;

            writer.WriteStartElement("list");

            WriteIdAttribute(graph, writer);
            WriteTypeAttribute(graph, writer);

            writer.WriteAttributeString("contact", history.Contact.Identifier);

            SerializeItems(history.Messages, writer);

            writer.WriteEndElement();
        }

        public override object Deserialize(XmlReader reader)
        {
            int objectId;
            Type type;
            ContactHistory history;

            reader.MoveToFirstAttribute();
            objectId = GetObjectId(reader);

            reader.MoveToNextAttribute();
            type = GetObjectType(reader);

            history = (ContactHistory) CreateObject(type, reader);

            if (!reader.IsEmptyElement)
            {
                DeserializeItems(history.Messages, reader);
            }

            Serializer.RegisterObject(objectId, history);

            return history;
        }
    }
}