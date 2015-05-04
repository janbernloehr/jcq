// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializer.cs" company="Jan-Cornelius Molnar">
//  Copyright (c) 2015-2015 Jan-Cornelius Molnar
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