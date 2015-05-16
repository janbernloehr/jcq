// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacFamily09Tests.cs" company="Jan-Cornelius Molnar">
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
    public class SnacFamily09Tests
    {
        //[TestMethod]
        //public void Snac0901DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0C, 0x00, 0x09, 0x00, 0x01, 0x00, 0x00, 0x7A, 0xC6, 0x21, 0x63, 
        //        0x00, 0x0E
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0901>(f);

        //    Assert.Inconclusive("Verify that Snac0901 was deserialized correctly.");
        //}

        [TestMethod]
        public void Snac0902DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0x9B, 0x00, 0x0A, 0x00, 0x09, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0902>(f);

            Assert.Inconclusive("Verify that Snac0902 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0903DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x06, 0x00, 0x16, 0x00, 0x09, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x02, 0x00, 0x02, 0x00, 0xC8, 0x00, 0x01, 0x00, 0x02, 0x00, 0xC8
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0903>(f);

            Assert.Inconclusive("Verify that Snac0903 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0904DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0E, 0x00, 0x09, 0x00, 0x04, 0x00, 0x00, 0x97, 0xD2, 0x07, 0x63,
                0x00, 0x00, 0xFF, 0xFF
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0904>(f);

            Assert.Inconclusive("Verify that Snac0904 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0905DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x61, 0xEC, 0x00, 0x3B, 0x00, 0x09, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07,
                0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x36, 0x07, 0x36, 0x32, 0x31, 0x38, 0x39, 0x30, 0x30,
                0x08, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39,
                0x39, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x37, 0x07, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30,
                0x30
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0905>(f);

            Assert.Inconclusive("Verify that Snac0905 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0906DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x62, 0x04, 0x00, 0x1A, 0x00, 0x09, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06,
                0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x37, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x36,
                0x96
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0906>(f);

            Assert.Inconclusive("Verify that Snac0906 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0907DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x61, 0xFA, 0x00, 0x38, 0x00, 0x09, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07,
                0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x36, 0x08, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
                0x31, 0x04, 0x31, 0x30, 0x30, 0x31, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x38, 0x07, 0x36,
                0x32, 0x31, 0x38, 0x38, 0x39, 0x37, 0x07, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0907>(f);

            Assert.Inconclusive("Verify that Snac0907 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0908DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x62, 0x04, 0x00, 0x1A, 0x00, 0x09, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08,
                0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x37, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x36,
                0x96
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0908>(f);

            Assert.Inconclusive("Verify that Snac0908 was deserialized correctly.");
        }
    }
}