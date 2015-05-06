using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    [TestClass]
    public class SnacFamily07Tests
    {

        //[TestMethod]
        //public void Snac0701DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0C, 0x00, 0x07, 0x00, 0x01, 0x00, 0x00, 0x99, 0xD0, 0x07, 0x62, 
        //        0x00, 0x0E
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0701>(f);

        //    Assert.Inconclusive("Verify that Snac0701 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0702DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x27, 0x03, 0x00, 0x0E, 0x00, 0x07, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 
        //        0x00, 0x01, 0x00, 0x00
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0702>(f);

        //    Assert.Inconclusive("Verify that Snac0702 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0703DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x3D, 0x1E, 0x00, 0x28, 0x00, 0x07, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 
        //        0x00, 0x03, 0x00, 0x01, 0x00, 0x11, 0x00, 0x16, 0x41, 0x56, 0x53, 0x68, 0x75, 0x74, 0x6B, 0x6F, 
        //        0x40, 0x6D, 0x61, 0x69, 0x6C, 0x2E, 0x6B, 0x68, 0x73, 0x74, 0x75, 0x2E, 0x72, 0x75
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0703>(f);

        //    Assert.Inconclusive("Verify that Snac0703 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0704DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x6B, 0xC0, 0x00, 0x1B, 0x00, 0x07, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 
        //        0x00, 0x01, 0x00, 0x0D, 0x52, 0x45, 0x41, 0x4C, 0x52, 0x65, 0x67, 0x72, 0x65, 0x73, 0x73, 0x6F, 
        //        0x72, 
        //        0x2A, 0x02, 0x6B, 0xC4, 0x00, 0x21, 0x00, 0x07, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x04, 
        //        0x00, 0x11, 0x00, 0x13, 0x61, 0x61, 0x61, 0x61, 0x61, 0x40, 0x6B, 0x70, 0x73, 0x6D, 0x2E, 0x6B, 
        //        0x68, 0x73, 0x74, 0x75, 0x2E, 0x72, 0x75
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0704>(f);

        //    Assert.Inconclusive("Verify that Snac0704 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0705DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x3D, 0x1C, 0x00, 0x1F, 0x00, 0x07, 0x00, 0x05, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 
        //        0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x0D, 0x52, 0x45, 0x41, 0x4C, 0x52, 0x65, 0x67, 0x72, 
        //        0x65, 0x73, 0x73, 0x6F, 0x72
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0705>(f);

        //    Assert.Inconclusive("Verify that Snac0705 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0706DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x6B, 0xCA, 0x00, 0x0A, 0x00, 0x07, 0x00, 0x06, 0x00, 0x00, 0x00, 0x07, 0x00, 0x06
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0706>(f);

        //    Assert.Inconclusive("Verify that Snac0706 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0707DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x12, 0x8B, 0x00, 0x22, 0x00, 0x07, 0x00, 0x07, 0x00, 0x00, 0x00, 0x01, 0x00, 0x06, 
        //        0x00, 0x1E, 0x00, 0x04, 0x00, 0x12, 0x68, 0x74, 0x74, 0x70, 0x3A, 0x2F, 0x2F, 0x77, 0x77, 0x77, 
        //        0x2E, 0x61, 0x6F, 0x6C, 0x2E, 0x63, 0x6F, 0x6D
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0707>(f);

        //    Assert.Inconclusive("Verify that Snac0707 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0708DeserializeTest()
        //{
        //    var data = new byte[]
        //    {

        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0708>(f);

        //    Assert.Inconclusive("Verify that Snac0708 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0709DeserializeTest()
        //{
        //    var data = new byte[]
        //    {

        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0709>(f);

        //    Assert.Inconclusive("Verify that Snac0709 was deserialized correctly.");
        //}


    }
}