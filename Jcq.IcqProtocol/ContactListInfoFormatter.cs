// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactListInfoFormatter.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Xml.Formatter;

namespace JCsTools.JCQ.IcqInterface
{
    public class ContactListInfoFormatter : DefaultReferenceFormatter
    {
        public ContactListInfoFormatter(XmlSerializer parent) : base(parent, typeof (ContactListInfo))
        {
        }

        protected override object CreateObject(Type type, XmlReader reader)
        {
            string itemCountValue;
            string dateChangedValue;

            int itemCount;
            DateTime dateChanged;

            itemCountValue = reader.GetAttribute("itemCount");
            dateChangedValue = reader.GetAttribute("dateChanged");

            itemCount = XmlConvert.ToInt32(itemCountValue);
            dateChanged = XmlConvert.ToDateTime(dateChangedValue, XmlDateTimeSerializationMode.Utc);

            return new ContactListInfo(itemCount, dateChanged);
        }

        protected override void SerializeProperties(object graph, XmlWriter writer)
        {
            ContactListInfo info;

            info = (ContactListInfo) graph;

            writer.WriteAttributeString("itemCount", XmlConvert.ToString(info.ItemCount));
            writer.WriteAttributeString("dateChanged",
                XmlConvert.ToString(info.DateChanged, XmlDateTimeSerializationMode.Utc));
        }

        protected override void DeserializeProperties(object graph, XmlReader reader)
        {
            // We need to override this method since the base implementation does deserialize
            // the properties and tries to assign them to the graph.
            // Since this object has only ReadOnly properties which already were set by the
            // CreateObject method, there is nothing to do here.
        }
    }
}