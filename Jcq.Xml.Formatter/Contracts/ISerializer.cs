// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializer.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.Xml.Formatter
{
    /// <summary>
    ///     Provides serialization functionality.
    /// </summary>
    public interface ISerializer
    {
        int GetSerializeObjectId(object graph);
        object GetDeserializeObjectById(int id);
        void Serialize(object graph, XmlWriter writer);
        object Deserialize(XmlReader reader);
        void RegisterValueFormatter(Type type, IValueFormatter formatter);
        void RegisterReferenceFormatter(Type type, IReferenceFormatter formatter);
        IValueFormatter GetValueFormatter(Type type);
        IReferenceFormatter GetReferenceFormatter(Type type);
        void RegisterObject(int id, object graph);
        void RegisterPropertyValueFixUp(PropertyValueFixUp fixup);
        void RegisterListItemFixUp(ListItemFixUp fixup);
        void RegisterReadOnlyListPropertyFixUp(ReadOnlyListPropertyFixUp fixup);
        void RegisterCustomFixUp(IFixUp fixup);
    }

    //Public Enum SerializationState
    //    Initializing
    //    Initialized
    //    Parsing
    //End Enum
}