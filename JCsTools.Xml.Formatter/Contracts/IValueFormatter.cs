// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueFormatter.cs" company="Jan-Cornelius Molnar">
//  Copyright (c) 2015-2015 Jan-Cornelius Molnar
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace JCsTools.Xml.Formatter
{
    /// <summary>
    ///     Provides Serialization functionality for a reference type.
    /// </summary>
    public interface IValueFormatter
    {
        Type ValueType { get; }
        string Serialize(object value);
        object Deserialize(string value);
    }
}