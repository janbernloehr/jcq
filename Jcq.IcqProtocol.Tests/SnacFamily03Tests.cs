// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacFamily03Tests.cs" company="Jan-Cornelius Molnar">
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
    [TestClass]
    public class SnacFamily03Tests
    {
        //[TestMethod]
        //public void Snac0301DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0C, 0x00, 0x03, 0x00, 0x01, 0x00, 0x00, 0x97, 0xD2, 0x07, 0x63, 
        //        0x00, 0x0E
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0301>(f);

        //    Assert.Inconclusive("Verify that Snac0301 was deserialized correctly.");
        //}

        [TestMethod]
        public void Snac0302SerializeTest()
        {
            var f = new Flap(FlapChannel.SnacData) { DatagramSequenceNumber = 0x229B };
            var s = new Snac0302
            {
                RequestId = 2
            };

            f.DataItems.Add(s);

            var actual = f.Serialize();

            var expected = new byte[]
            {
                0x2A, 0x02, 0x22, 0x9B, 0x00, 0x0A,
                0x00, 0x03, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x02
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Snac0303DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x04, 0x00, 0x1C,
                0x00, 0x03, 0x00, 0x03,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x02, 0x00, 0x02, 0x02, 0xEE, // TLV02
                0x00, 0x01, 0x00, 0x02, 0x02, 0x58, // TLV01
                0x00, 0x03, 0x00, 0x02, 0x02, 0x00 // TLV03
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0303>(f);

            Assert.AreEqual(0x0258, s.MaxBuddylistSize.MaxNumberOfContactListEntries);
            Assert.AreEqual(0x02EE, s.MaxNumberOfWatchers.MaxNumberOfWatchers);
            Assert.AreEqual(0x0200, s.MaxOnlineNotifications.MaxOnlineNotifications);
        }

        //[TestMethod]
        //public void Snac0304SerializeTest()
        //{
        //    var f = new Flap(FlapChannel.SnacData) { DatagramSequenceNumber = 0x229B };
        //    var s = new Snac0304
        //    {
        //        RequestId = 4
        //    };

        //    f.DataItems.Add(s);

        //    var actual = f.Serialize();

        //    var expected = new byte[]
        //    {
        //        0x2A, 0x02, 0x62, 0x79, 0x00, 0x48,
        //        0x00, 0x03, 0x00, 0x04,
        //        0x00, 0x00, 0x00, 0x00, 0x00, 0x04,
        //        0x07, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
        //        0x04, 0x31, 0x30, 0x30, 0x31,
        //        0x07, 0x36, 0x32,0x62,0x31, 0x38, 0x38, 0x39, 0x37,
        //        0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x38, 0x07, 0x36, 0x32,
        //        0x62,
        //        0x31, 0x38, 0x38, 0x39, 0x39, 0x07, 0x36, 0x32, 0x31, 0x38, 0x39, 0x30, 0x30, 0x08, 0x31, 0x30,
        //        0x10,
        //        0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x35,
        //        0x95
        //    };

        //    Assert.Inconclusive("Verify that Snac0304 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0305DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x62, 0x79, 0x00, 0x12, 0x00, 0x03, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 
        //        0x07, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 
        //        0x00
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0305>(f);

        //    Assert.Inconclusive("Verify that Snac0305 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0306DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x16, 0x91, 0x00, 0x0A, 0x00, 0x03, 0x00, 0x06, 0x00, 0x00, 0x00, 0x03, 0x00, 0x06
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0306>(f);

        //    Assert.Inconclusive("Verify that Snac0306 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0307DeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x63, 0x79, 0x00, 0x48, 0x00, 0x03, 0x00, 0x07, 0x00, 0x00, 0x00, 0x03, 0x00, 0x07, 
        //        0x07, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x04, 0x31, 0x30, 0x30, 0x31, 0x07, 0x36, 0x32, 
        //        0x62, 
        //        0x31, 0x38, 0x38, 0x39, 0x37, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x38, 0x07, 0x36, 0x32, 
        //        0x62, 
        //        0x31, 0x38, 0x38, 0x39, 0x39, 0x07, 0x36, 0x32, 0x31, 0x38, 0x39, 0x30, 0x30, 0x08, 0x31, 0x30, 
        //        0x10, 
        //        0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x35, 
        //        0x95
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0307>(f);

        //    Assert.Inconclusive("Verify that Snac0307 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0308DeserializeTest()
        //{
        //    var data = new byte[]
        //    {

        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0308>(f);

        //    Assert.Inconclusive("Verify that Snac0308 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac0309DeserializeTest()
        //{
        //    var data = new byte[]
        //    {

        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac0309>(f);

        //    Assert.Inconclusive("Verify that Snac0309 was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac030ADeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x3E, 0x11, 0x00, 0x14, 0x00, 0x03, 0x00, 0x0A, 0x00, 0x00, 0x82, 0x95, 0xE9, 0x15, 
        //        0x09, 0x31, 0x33, 0x33, 0x39, 0x34, 0x38, 0x38, 0x32, 0x32, 
        //        0x22
        //    };

        //    var f = SerializationTools.DeserializeFlap(data);
        //    var s = SerializationTools.DeserializeSnac<Snac030A>(f);

        //    Assert.Inconclusive("Verify that Snac030A was deserialized correctly.");
        //}

        [TestMethod]
        public void Snac030Bv1DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x17, 0x00, 0xA9,
                0x00, 0x03, 0x00, 0x0B,
                0x00, 0x00, 0x82, 0x95, 0xE9, 0x1B,
                0x07, 0x33, 0x34, 0x31, 0x33, 0x39, 0x35, 0x30, // UIN len + string
                0x00, 0x00, // warning level
                0x00, 0x07, // number of tlv
                0x00, 0x01, 0x00, 0x02, // TLV01
                0x00, 0x50,
                0x00, 0x0C, 0x00, 0x25, // TLV0C
                0x3E, 0x4C, 0xCF, 0x2D,
                0x00, 0x00, 0x4A, 0x68,
                0x04,
                0x00, 0x08,
                0x70, 0x7B, 0xD5, 0x5B,
                0x00, 0x00, 0x00, 0x50,
                0x00, 0x00, 0x00, 0x03,
                0x3B, 0xFF, 0xE8, 0x8A,
                0x3D, 0xCF, 0x01, 0x51,
                0x3D, 0xCE, 0xF8, 0x61,
                0x00, 0x00,
                0x00, 0x0A, 0x00, 0x04, // TLV0A
                0x3E, 0x4C, 0xCF, 0x2D,
                0x00, 0x06, 0x00, 0x04, // TLV06
                0x20, 0x12, 0x00, 0x00,
                0x00, 0x0D, 0x00, 0x40, // TLV0D
                0x09, 0x46, 0x13, 0x49, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00,
                0x09, 0x46, 0x13, 0x4E, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00,
                0x97, 0xB1, 0x27, 0x51, 0x24, 0x3C, 0x43, 0x34, 0xAD, 0x22, 0xD6, 0xAB, 0xF7, 0x3F, 0x14, 0x92,
                0x09, 0x46, 0x13, 0x44, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00,
                0x00, 0x0F, 0x00, 0x04, // TLV0F
                0x00, 0x00, 0x88, 0xB4,
                0x00, 0x03, 0x00, 0x04, // TLV03
                0x3D, 0xD4, 0x42, 0x7F
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac030B>(f);

            Assert.AreEqual(1, s.UserInfos.Count);

            UserInfo info = s.UserInfos.First();

            Assert.AreEqual("3413950", info.Uin);
            Assert.AreEqual(0, info.WarningLevel);

            Assert.AreEqual(UserClass.ICQNonCommercialAccountFlag | UserClass.ICQUserSign, info.UserClass.UserClass);
            Assert.AreEqual(0x3E4CCF2D, info.DCInfo.DcInternalIpAddress);
            Assert.AreEqual(0x4A68, info.DCInfo.DcPort);
            Assert.AreEqual(DcType.NormalDirectConnectionWithoutProxyFirewall, info.DCInfo.DcByte);
            Assert.AreEqual(0x8, info.DCInfo.DcProtocolVersion);
            Assert.AreEqual(0x707BD55B, info.DCInfo.DcAuthCookie);
            Assert.AreEqual(80, info.DCInfo.WebFrontPort);
            Assert.AreEqual(ByteConverter.ToDateTimeFromUInt32FileStamp(new byte[] { 0x3B, 0xFF, 0xE8, 0x8A }),
                info.DCInfo.LastInfoUpdate);
            Assert.AreEqual(ByteConverter.ToDateTimeFromUInt32FileStamp(new byte[] { 0x3D, 0xCF, 0x01, 0x51 }),
                info.DCInfo.LastExtInfoUpdate);
            Assert.AreEqual(ByteConverter.ToDateTimeFromUInt32FileStamp(new byte[] { 0x3D, 0xCE, 0xF8, 0x61 }),
                info.DCInfo.LastExtStatusUpdate);

            //Assert.AreEqual(0x2012, info.UserStatus.UserFlag);
            Assert.AreEqual(UserStatus.Online, info.UserStatus.UserStatus);

            CollectionAssert.AreEqual(new[]
            {
                ByteConverter.ToGuid(new byte[]
                {0x09, 0x46, 0x13, 0x49, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00}),
                ByteConverter.ToGuid(new byte[]
                {0x09, 0x46, 0x13, 0x4E, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00}),
                ByteConverter.ToGuid(new byte[]
                {0x97, 0xB1, 0x27, 0x51, 0x24, 0x3C, 0x43, 0x34, 0xAD, 0x22, 0xD6, 0xAB, 0xF7, 0x3F, 0x14, 0x92}),
                ByteConverter.ToGuid(new byte[]
                {0x09, 0x46, 0x13, 0x44, 0x4C, 0x7F, 0x11, 0xD1, 0x82, 0x22, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00})
            }, info.UserCapabilities.Capabilites);

            Assert.AreEqual(ByteConverter.ToDateTimeFromUInt32FileStamp(new byte[] { 0x00, 0x00, 0x88, 0xB4 }), info.OnlineTime.OnlineTime);
            Assert.AreEqual(ByteConverter.ToDateTimeFromUInt32FileStamp(new byte[] { 0x3D, 0xD4, 0x42, 0x7F }), info.SignOnTime.SignOnTime);
        }

        [TestMethod]
        public void Snac030Bv2DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x26, 0x12, 0x00, 0x7F, 0x00, 0x03, 0x00, 0x0B, 0x00, 0x00, 0xC8, 0x4D, 0xE4, 0x6F,
                0x09, 0x33, 0x34, 0x38, 0x39, 0x33, 0x34, 0x37, 0x37, 0x34, 0x00, 0x00, 0x00, 0x07, 0x00, 0x01,
                0x00, 0x02, 0x00, 0x50, 0x00, 0x0C, 0x00, 0x25, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x04, 0x00, 0x09, 0xCD, 0xA9, 0x3C, 0x4C, 0x00, 0x00, 0x7C, 0x0F, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00,
                0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x04, 0x10, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00,
                0x04, 0x00, 0x00, 0x03, 0xF6, 0x00, 0x1D, 0x00, 0x14, 0x00, 0x01, 0x01, 0x10, 0x51, 0xBD, 0x67,
                0x50, 0x54, 0x3B, 0xD5, 0xCE, 0x72, 0x88, 0x14, 0x1D, 0xA9, 0x05, 0x0D, 0x70, 0x00, 0x03, 0x00,
                0x04, 0x41, 0xEC, 0x78, 0x5A
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac030B>(f);

            Assert.Inconclusive("Verify that Snac030B was deserialized correctly.");
        }

        [TestMethod]
        public void Snac030CDeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3F, 0x3A, 0x00, 0x1D,
                0x00, 0x03, 0x00, 0x0C,
                0x00, 0x00, 0x82, 0x99, 0x4A, 0xAC,
                0x08, 0x33, 0x32, 0x37, 0x31, 0x34, 0x39, 0x34, 0x34, // UIN length + string
                0x00, 0x00, // warning level
                0x00, 0x01, // number of TLVs
                0x00, 0x01, 0x00, 0x02, // TLV01
                0x00, 0x00
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac030C>(f);

            Assert.AreEqual(1, s.UserInfos.Count);

            var info = s.UserInfos.First();

            Assert.AreEqual("32714944", info.Uin );
            Assert.AreEqual(0, info.WarningLevel);

            Assert.AreEqual((Jcq.IcqProtocol.DataTypes.UserClass)0, info.UserClass.UserClass);
        }
    }
}