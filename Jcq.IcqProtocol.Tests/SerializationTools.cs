// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTools.cs" company="Jan-Cornelius Molnar">
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

using System.Linq;
using Jcq.IcqProtocol.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    internal static class SerializationTools
    {
        /// <summary>
        ///     Deserialize a Flap object from the given byte array and perfom sanity checks.
        /// </summary>
        /// <param name="data">The data containing the flap.</param>
        internal static Flap DeserializeFlap(byte[] data)
        {
            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize, "Flap Total Size");
            Assert.AreEqual(data.Length - 6, f.DataSize, "Flap Data Size");

            Assert.AreEqual(ByteConverter.ToUInt16(new[] {data[2], data[3]}), f.DatagramSequenceNumber,
                "Flap DatagramSequenceNumber");

            Assert.IsNotNull(f.DataItems, "Flap DataItems");
            Assert.AreEqual(1, f.DataItems.Count, "Flap DataItems Count");

            return f;
        }

        /// <summary>
        ///     Deserialize a Snac(T) object from the given Flap and perform sanity checks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        internal static T DeserializeSnac<T>(Flap f)
            where T : Snac
        {
            Assert.AreEqual(FlapChannel.SnacData, f.Channel, "Flap Channel");

            // snac
            ISerializable item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof(T), "Flap DataItem Type");

            var s = (T) item;

            Assert.AreEqual(f.DataSize - 10, s.DataSize, "Snac Data Size");

            return (T) item;
        }
    }
}