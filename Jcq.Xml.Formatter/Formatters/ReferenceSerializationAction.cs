// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceSerializationAction.cs" company="Jan-Cornelius Molnar">
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

using System.Xml;

namespace JCsTools.Xml.Formatter
{
    public class ReferenceSerializationAction : IPropertySerializationAction
    {
        private readonly PropertyDescriptor _PropertyDescriptor;
        private readonly ISerializer _Serializer;

        public ReferenceSerializationAction(PropertyDescriptor descriptor, ISerializer formatter)
        {
            _PropertyDescriptor = descriptor;
            _Serializer = formatter;
        }

        PropertyDescriptor IPropertySerializationAction.PropertyDescriptor
        {
            get { return _PropertyDescriptor; }
        }

        void IPropertySerializationAction.SerializeProperty(object graph, XmlWriter writer)
        {
            string objectId;
            object propertyValue;

            propertyValue = _PropertyDescriptor.GetValue(graph);
            objectId = _Serializer.GetSerializeObjectId(propertyValue).ToString();

            writer.WriteAttributeString(_PropertyDescriptor.PropertyName, objectId);
        }
    }
}