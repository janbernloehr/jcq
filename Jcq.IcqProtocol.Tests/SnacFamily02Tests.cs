// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacFamily02Tests.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.JCQ.IcqInterface.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    [TestClass]
    public class SnacFamily02Tests
    {
        //[TestMethod]
        //public void Snac0201DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0C, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x99, 0xD0, 0x07, 0x62, 
        //        0x00, 0x0E
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0201>(f);

        //    Assert.Inconclusive("Verify that Snac0201 was deserialized correctly.");
        //}

        [TestMethod]
        public void Snac0202DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0x9B, 0x00, 0x0A, 0x00, 0x02, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0202>(f);

            Assert.Inconclusive("Verify that Snac0202 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0203DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x03, 0x00, 0x22, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x01, 0x00, 0x02, 0x04, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x10, 0x00, 0x03, 0x00, 0x02,
                0x00, 0x0A, 0x00, 0x04, 0x00, 0x02, 0x10, 0x00
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0203>(f);

            Assert.Inconclusive("Verify that Snac0203 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0204DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0xA0, 0x00, 0x4E, 0x00, 0x02, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04,
                0x00, 0x05, 0x00, 0x40, 0x09, 0x46, 0x13, 0x49, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45,
                0x53, 0x54, 0x00, 0x00, 0x97, 0xB1, 0x27, 0x51, 0x24, 0x3C, 0x43, 0x34, 0xAD, 0x22, 0xD6, 0xAB,
                0xF7, 0x3F, 0x14, 0x92, 0x2E, 0x7A, 0x64, 0x75, 0xFA, 0xDF, 0x4D, 0xC8, 0x88, 0x6F, 0xEA, 0x35,
                0x95, 0xFD, 0xB6, 0xDF, 0x09, 0x46, 0x13, 0x44, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45,
                0x53, 0x54, 0x00, 0x00
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0204>(f);

            Assert.Inconclusive("Verify that Snac0204 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0205DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0xA3, 0x00, 0x15, 0x00, 0x02, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                0x00, 0x01, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x35
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0205>(f);

            Assert.Inconclusive("Verify that Snac0205 was deserialized correctly.");
        }
    }
}