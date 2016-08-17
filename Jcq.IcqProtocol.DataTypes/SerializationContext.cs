// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationContext.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Jcq.IcqProtocol.DataTypes
{
    public static class SerializationContext
    {
        private static readonly Type ContextType = typeof(SerializationContext);
        private static readonly string AssemblyName;
        private static readonly string SnacNamespace;

        static SerializationContext()
        {
            AssemblyName = ContextType.Assembly.FullName;
            SnacNamespace = ContextType.Namespace;
        }

        private static Type GetSnacMapping(SnacDescriptor desc)
        {
            string typeName = string.Format("{1}.Snac{0}, {2}",
                SnacDescriptor.GetKey(desc).Replace(",", ""), SnacNamespace, AssemblyName);

            Type snacType = Type.GetType(typeName, false, true);

            return snacType;
        }

        public static Snac DeserializeSnac(int offset, SnacDescriptor desc, List<byte> bytes)
        {
            var data = bytes.GetRange(offset, bytes.Count - offset);

            Type type = GetSnacMapping(desc);

            if (type == null)
                return null;

            var x = (Snac) Activator.CreateInstance(type);
            x.Deserialize(data);

            return x;
        }
    }
}