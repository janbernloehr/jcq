// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Identity.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Linq;

namespace JCsTools.IdentityManager
{
    public class Identity : IIdentity
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public Identity(string id)
        {
            Identifier = id;
        }

        public string Identifier { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public T GetAttribute<T>(IIdentityAttribute<T> key)
        {
            return (T) GetAttribute(key.AttributeName);
        }

        public string[] GetAttributeNames()
        {
            return values.Keys.ToArray();
        }

        public void SetAttribute<T>(IIdentityAttribute<T> key, T value)
        {
            SetAttribute(key.AttributeName, value);
        }

        public void SetAttribute(string key, object value)
        {
            if (values.ContainsKey(key))
            {
                values[key] = value;
            }
            else
            {
                values.Add(key, value);
            }
        }

        public object GetAttribute(string key)
        {
            return values[key];
        }

        public bool HasAttribute(string key)
        {
            return values.ContainsKey(key);
        }

        public bool HasAttribute<T>(IIdentityAttribute<T> key)
        {
            return HasAttribute(key.AttributeName);
        }
    }
}