// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReferenceFormatter.cs" company="Jan-Cornelius Molnar">
//  Copyright (c) 2015-2015 Jan-Cornelius Molnar
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Xml;

namespace JCsTools.Xml.Formatter
{
    /// <summary>
    ///     Provides Serialization functionality for a reference type.
    /// </summary>
    public interface IReferenceFormatter
    {
        Type ReferenceType { get; }
        void Serialize(object graph, XmlWriter writer);
        object Deserialize(XmlReader reader);
    }
}