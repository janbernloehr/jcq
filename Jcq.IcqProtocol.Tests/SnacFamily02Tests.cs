using System;
using System.Linq;
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
                0xDE, 
                0x53, 0x54, 0x00, 0x00, 0x97, 0xB1, 0x27, 0x51, 0x24, 0x3C, 0x43, 0x34, 0xAD, 0x22, 0xD6, 0xAB, 
                0xF7, 0x3F, 0x14, 0x92, 0x2E, 0x7A, 0x64, 0x75, 0xFA, 0xDF, 0x4D, 0xC8, 0x88, 0x6F, 0xEA, 0x35, 
                0x95, 0xFD, 0xB6, 0xDF, 0x09, 0x46, 0x13, 0x44, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 
                0xDE, 
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
                0x00, 0x01, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x35, 
                0x95
            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0205>(f);

            Assert.Inconclusive("Verify that Snac0205 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0206DeserializeTest()
        {
            var data = new byte[]
            {

            };

            var f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0206>(f);

            Assert.Inconclusive("Verify that Snac0206 was deserialized correctly.");
        }

        //[TestMethod]
        //public void Snac0207DeserializeTest()
        //{
        //    var data = new byte[]
        //    {

        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0207>(f);

        //    Assert.Inconclusive("Verify that Snac0207 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0208DeserializeTest()
        //{
        //    var data = new byte[]
        //    {

        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0208>(f);

        //    Assert.Inconclusive("Verify that Snac0208 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0209DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x00, 0x02, 0x00, 0x09, 0x00, 0x00, 0x00, 0x04, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x08, 0x75, 0x73, 
        //        0x2D, 0x61, 0x73, 0x63, 0x69, 0x69, 0x00, 0x0A, 0x00, 0x02, 0x00, 0x01, 0x00, 0x01, 0x00, 0x09, 
        //        0x46, 0x69, 0x72, 0x73, 0x74, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x02, 0x00, 0x08, 0x4C, 0x61, 0x73, 
        //        0x74, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x03, 0x00, 0x0A, 0x4D, 0x69, 0x64, 0x6C, 0x64, 0x65, 0x4E, 
        //        0x61, 0x6D, 0x65, 0x00, 0x04, 0x00, 0x0A, 0x4D, 0x61, 0x69, 0x64, 0x65, 0x6E, 0x4E, 0x61, 0x6D, 
        //        0x65, 0x00, 0x06, 0x00, 0x02, 0x52, 0x55, 0x00, 0x07, 0x00, 0x02, 0x53, 0x54, 0x00, 0x08, 0x00, 
        //        0x04, 0x43, 0x69, 0x74, 0x79, 0x00, 0x0C, 0x00, 0x08, 0x4E, 0x69, 0x63, 0x6B, 0x4E, 0x61, 0x6D, 
        //        0x65, 0x00, 0x0D, 0x00, 0x06, 0x36, 0x38, 0x30, 0x30, 0x33, 0x38, 0x00, 0x21, 0x00, 0x0A, 0x53, 
        //        0x80, 
        //        0x74, 0x72, 0x65, 0x65, 0x74, 0x41, 0x64, 0x64, 0x72
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0209>(f);

        //    Assert.Inconclusive("Verify that Snac0209 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac020ADeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x97, 0x63, 0x00, 0x0C, 0x00, 0x02, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x07, 0x00, 0x09, 
        //        0x00, 0x01
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac020A>(f);

        //    Assert.Inconclusive("Verify that Snac020A was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac020BDeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x58, 0x39, 0x00, 0x13, 0x00, 0x02, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x03, 0x00, 0x0B, 
        //        0x08, 0x73, 0x6F, 0x6D, 0x65, 0x6E, 0x69, 0x63, 0x6B
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac020B>(f);

        //    Assert.Inconclusive("Verify that Snac020B was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac020CDeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x97, 0x52, 0x00, 0x0E, 0x00, 0x02, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x03, 0x00, 0x0B, 
        //        0x00, 0x01, 0x00, 0x00
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac020C>(f);

        //    Assert.Inconclusive("Verify that Snac020C was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac020FDeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x58, 0x5A, 0x00, 0x0A, 0x00, 0x02, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x08, 0x00, 0x0F
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac020F>(f);

        //    Assert.Inconclusive("Verify that Snac020F was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0210DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x97, 0x64, 0x00, 0x0C, 0x00, 0x02, 0x00, 0x10, 0x00, 0x00, 0x00, 0x08, 0x00, 0x0F, 
        //        0x00, 0x01
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0210>(f);

        //    Assert.Inconclusive("Verify that Snac0210 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0215DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x58, 0x44, 0x00, 0x17, 0x00, 0x02, 0x00, 0x15, 0x00, 0x00, 0x00, 0x04, 0x00, 0x15, 
        //        0x00, 0x00, 0x00, 0x01, 0x08, 0x73, 0x6F, 0x6D, 0x65, 0x6E, 0x69, 0x63, 0x6B
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0215>(f);

        //    Assert.Inconclusive("Verify that Snac0215 was deserialized correctly.");
        //}


    }
}

