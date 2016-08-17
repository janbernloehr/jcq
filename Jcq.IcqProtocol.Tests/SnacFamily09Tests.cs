// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacFamily09Tests.cs" company="Jan-Cornelius Molnar">
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

using Jcq.IcqProtocol.DataTypes;
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

            Flap f = SerializationTools.DeserializeFlap(data);
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

            Flap f = SerializationTools.DeserializeFlap(data);
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

            Flap f = SerializationTools.DeserializeFlap(data);
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

            Flap f = SerializationTools.DeserializeFlap(data);
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

            Flap f = SerializationTools.DeserializeFlap(data);
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

            Flap f = SerializationTools.DeserializeFlap(data);
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

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0908>(f);

            Assert.Inconclusive("Verify that Snac0908 was deserialized correctly.");
        }
    }
}