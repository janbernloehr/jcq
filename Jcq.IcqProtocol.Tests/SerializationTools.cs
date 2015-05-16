// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTools.cs" company="Jan-Cornelius Molnar">
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

using System.Linq;
using JCsTools.JCQ.IcqInterface.DataTypes;
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
            var item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof (T), "Flap DataItem Type");

            var s = (T) item;

            Assert.AreEqual(f.DataSize - 10, s.DataSize, "Snac Data Size");

            return (T) item;
        }
    }
}