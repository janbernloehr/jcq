// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationContext.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public static class SerializationContext
    {
        private static readonly Type _ContextType = typeof (SerializationContext);
        private static readonly string _AssemblyName = "Jcq.IcqProtocol.DataTypes";
        private static readonly string _SnacNamespace = "JCsTools.JCQ.IcqInterface.DataTypes";

        private static Type GetSnacMapping(SnacDescriptor desc)
        {
            string typeName;
            Type snacType;

            typeName = string.Format("{1}.Snac{0}, {2}", SnacDescriptor.GetKey(desc).Replace(",", ""), _SnacNamespace,
                _AssemblyName);
            snacType = Type.GetType(typeName, false, true);

            return snacType;
        }

        public static Snac DeserializeSnac(int offset, SnacDescriptor desc, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);

            Type type;
            Snac x;

            type = GetSnacMapping(desc);

            if (type == null)
                return null;

            x = (Snac) Activator.CreateInstance(type);
            x.Deserialize(data);

            return x;
        }
    }
}