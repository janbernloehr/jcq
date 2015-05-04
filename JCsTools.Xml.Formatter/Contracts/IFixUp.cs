// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFixUp.cs" company="Jan-Cornelius Molnar">
//  Copyright (c) 2015-2015 Jan-Cornelius Molnar
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JCsTools.Xml.Formatter
{
    /// <summary>
    /// Provides FixUp functionality for deserialization.
    /// </summary>
    public interface IFixUp
    {
        void Execute(ISerializer serializer);
    }
}