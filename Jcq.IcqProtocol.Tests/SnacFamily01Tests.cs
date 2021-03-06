﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacFamily01Tests.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using Jcq.IcqProtocol.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    [TestClass]
    public class SnacFamily01Tests
    {
        [TestMethod]
        public void Snac0101DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x99, 0xD0, 0x07, 0x62,
                0x00, 0x05
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0101>(f);

            Assert.AreEqual(ErrorCode.RequestedServiceUnavailable, s.ErrorCode);
        }

        [TestMethod]
        public void Snac0102DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x2, 0x3, 0x0, 0x5a, 0x0, 0x1, 0x0, 0x2, 0x0, 0x0, 0x0, 0x0, 0x0, 0x2,
                0x0, 0x1, 0x0, 0x3, 0x1, 0x10, 0x4, 0x7B, 0x0, 0x13, 0x0, 0x2, 0x1, 0x10, 0x4, 0x7B,
                0x0, 0x2, 0x0, 0x1, 0x1, 0x1, 0x4, 0x7B, 0x0, 0x3, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B,
                0x0, 0x15, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 0x0, 0x4, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B,
                0x0, 0x6, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 0x0, 0x9, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B,
                0x0, 0xA, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 0x0, 0xB, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0102>(f);

            Assert.IsNotNull(s.Families);
            Assert.AreEqual(10, s.Families.Count);

            FamilyIdToolPair family = s.Families[0];

            Assert.AreEqual(1, family.FamilyNumber);
            Assert.AreEqual(3, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[1];

            Assert.AreEqual(0x13, family.FamilyNumber);
            Assert.AreEqual(2, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[2];

            Assert.AreEqual(2, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0101, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[3];

            Assert.AreEqual(3, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[4];

            Assert.AreEqual(0x15, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[5];

            Assert.AreEqual(4, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[6];

            Assert.AreEqual(6, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[7];

            Assert.AreEqual(9, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[8];

            Assert.AreEqual(0xA, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[9];

            Assert.AreEqual(0xB, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);
        }

        [TestMethod]
        public void Snac0103DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3D, 0xFE, 0x00, 0x22, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00, 0x82, 0x95, 0xE5, 0x7A,
                0x00, 0x01, 0x00, 0x02, 0x00, 0x03, 0x00, 0x04, 0x00, 0x06, 0x00, 0x08, 0x00, 0x09, 0x00, 0x0A,
                0x00, 0x0B, 0x00, 0x0C, 0x00, 0x13, 0x00, 0x15
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0103>(f);

            Assert.IsNotNull(s.ServerSupportedFamilyIds);
            Assert.AreEqual(12, s.ServerSupportedFamilyIds.Count);

            Assert.AreEqual(1, s.ServerSupportedFamilyIds[0]);
            Assert.AreEqual(2, s.ServerSupportedFamilyIds[1]);
            Assert.AreEqual(3, s.ServerSupportedFamilyIds[2]);
            Assert.AreEqual(4, s.ServerSupportedFamilyIds[3]);
            Assert.AreEqual(6, s.ServerSupportedFamilyIds[4]);
            Assert.AreEqual(8, s.ServerSupportedFamilyIds[5]);
            Assert.AreEqual(9, s.ServerSupportedFamilyIds[6]);
            Assert.AreEqual(0xA, s.ServerSupportedFamilyIds[7]);
            Assert.AreEqual(0xB, s.ServerSupportedFamilyIds[8]);
            Assert.AreEqual(0xC, s.ServerSupportedFamilyIds[9]);
            Assert.AreEqual(0x13, s.ServerSupportedFamilyIds[10]);
            Assert.AreEqual(0x15, s.ServerSupportedFamilyIds[11]);
        }

        [TestMethod]
        public void Snac0104DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x02, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                0x00, 0x0D
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0104>(f);

            Assert.AreEqual(0x0D, s.ServiceFamilyId, "ServiceFamilyId");
        }

        [TestMethod]
        public void Snac0105DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE5, 0x66, 0x00, 0x3E, 0x00, 0x01, 0x00, 0x05, 0x00, 0x00, 0x8E, 0xCD, 0xB1, 0x97,
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                0x00, 0x0D, 0x00, 0x02, 0x00, 0x0D, 0x00, 0x05, 0x00, 0x0F, 0x31, 0x30, 0x2E, 0x31, 0x30, 0x2E,
                0x31, 0x30, 0x2E, 0x38, 0x3A, 0x35, 0x31, 0x39, 0x30, 0x00, 0x06, 0x00, 0x0F, 0x31, 0x32, 0x33,
                0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0105>(f);

            Assert.AreEqual(0x0D, s.ServiceFamily.ServiceFamilyId, "Service Family Id");
            Assert.AreEqual("10.10.10.8:5190", s.ServerAddress.ServerAddress, "ServerAddress");
            CollectionAssert.AreEqual(new byte[]
            {
                0x31, 0x32, 0x33, 0x34, 0x35,
                0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46
            }, s.AuthorizationCookie.AuthorizationCookie.ToArray(), "AuthorizationCookie");
        }

        [TestMethod]
        public void Snac0106Test()
        {
            var data = new byte[]
            {0x2A, 0x02, 0x22, 0x96, 0x00, 0x0A, 0x00, 0x01, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06};

            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize);
            Assert.AreEqual(data.Length - 6, f.DataSize);

            Assert.AreEqual(FlapChannel.SnacData, f.Channel);

            Assert.AreEqual(0x2296, f.DatagramSequenceNumber);

            Assert.IsNotNull(f.DataItems);
            Assert.AreEqual(1, f.DataItems.Count);

            // snac
            ISerializable item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof(Snac0106));

            var s = (Snac0106) item;

            Assert.AreEqual(0, s.DataSize);
        }

        [TestMethod]
        public void Snac0107DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x38, 0xBE, 0x03, 0x3B, 0x00, 0x01, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06,
                0x00, 0x05, 0x00, 0x01, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x09, 0xC4, 0x00, 0x00, 0x07, 0xD0,
                0x00, 0x00, 0x05, 0xDC, 0x00, 0x00, 0x03, 0x20, 0x00, 0x00, 0x0D, 0x69, 0x00, 0x00, 0x17, 0x70,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x0B, 0xB8, 0x00,
                0x00, 0x07, 0xD0, 0x00, 0x00, 0x05, 0xDC, 0x00, 0x00, 0x03, 0xE8, 0x00, 0x00, 0x17, 0x70, 0x00,
                0x00, 0x17, 0x70, 0x00, 0x00, 0xF9, 0x0B, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00,
                0x13, 0xEC, 0x00, 0x00, 0x13, 0x88, 0x00, 0x00, 0x0F, 0xA0, 0x00, 0x00, 0x0B, 0xB8, 0x00, 0x00,
                0x11, 0x47, 0x00, 0x00, 0x17, 0x70, 0x00, 0x00, 0x5C, 0xD8, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x14, 0x00, 0x00, 0x15, 0x7C, 0x00, 0x00, 0x14, 0xB4, 0x00, 0x00, 0x10, 0x68, 0x00, 0x00, 0x0B,
                0xB8, 0x00, 0x00, 0x17, 0x70, 0x00, 0x00, 0x1F, 0x40, 0x00, 0x00, 0xF9, 0x0B, 0x00, 0x00, 0x05,
                0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x15, 0x7C, 0x00, 0x00, 0x14, 0xB4, 0x00, 0x00, 0x10, 0x68,
                0x00, 0x00, 0x0B, 0xB8, 0x00, 0x00, 0x17, 0x70, 0x00, 0x00, 0x1F, 0x40, 0x00, 0x00, 0xF9, 0x0B,
                0x00, 0x00, 0x01, 0x00, 0x91, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x02, 0x00, 0x01, 0x00,
                0x03, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x05, 0x00, 0x01, 0x00, 0x06, 0x00, 0x01, 0x00,
                0x07, 0x00, 0x01, 0x00, 0x08, 0x00, 0x01, 0x00, 0x09, 0x00, 0x01, 0x00, 0x0A, 0x00, 0x01, 0x00,
                0x0B, 0x00, 0x01, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x01, 0x00,
                0x0F, 0x00, 0x01, 0x00, 0x10, 0x00, 0x01, 0x00, 0x11, 0x00, 0x01, 0x00, 0x12, 0x00, 0x01, 0x00,
                0x13, 0x00, 0x01, 0x00, 0x14, 0x00, 0x01, 0x00, 0x15, 0x00, 0x01, 0x00, 0x16, 0x00, 0x01, 0x00,
                0x17, 0x00, 0x01, 0x00, 0x18, 0x00, 0x01, 0x00, 0x19, 0x00, 0x01, 0x00, 0x1A, 0x00, 0x01, 0x00,
                0x1B, 0x00, 0x01, 0x00, 0x1C, 0x00, 0x01, 0x00, 0x1D, 0x00, 0x01, 0x00, 0x1E, 0x00, 0x01, 0x00,
                0x1F, 0x00, 0x01, 0x00, 0x20, 0x00, 0x01, 0x00, 0x21, 0x00, 0x02, 0x00, 0x01, 0x00, 0x02, 0x00,
                0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x02, 0x00, 0x04, 0x00, 0x02, 0x00, 0x06, 0x00, 0x02, 0x00,
                0x07, 0x00, 0x02, 0x00, 0x08, 0x00, 0x02, 0x00, 0x0A, 0x00, 0x02, 0x00, 0x0C, 0x00, 0x02, 0x00,
                0x0D, 0x00, 0x02, 0x00, 0x0E, 0x00, 0x02, 0x00, 0x0F, 0x00, 0x02, 0x00, 0x10, 0x00, 0x02, 0x00,
                0x11, 0x00, 0x02, 0x00, 0x12, 0x00, 0x02, 0x00, 0x13, 0x00, 0x02, 0x00, 0x14, 0x00, 0x02, 0x00,
                0x15, 0x00, 0x03, 0x00, 0x01, 0x00, 0x03, 0x00, 0x02, 0x00, 0x03, 0x00, 0x03, 0x00, 0x03, 0x00,
                0x06, 0x00, 0x03, 0x00, 0x07, 0x00, 0x03, 0x00, 0x08, 0x00, 0x03, 0x00, 0x09, 0x00, 0x03, 0x00,
                0x0A, 0x00, 0x03, 0x00, 0x0B, 0x00, 0x03, 0x00, 0x0C, 0x00, 0x04, 0x00, 0x01, 0x00, 0x04, 0x00,
                0x02, 0x00, 0x04, 0x00, 0x03, 0x00, 0x04, 0x00, 0x04, 0x00, 0x04, 0x00, 0x05, 0x00, 0x04, 0x00,
                0x07, 0x00, 0x04, 0x00, 0x08, 0x00, 0x04, 0x00, 0x09, 0x00, 0x04, 0x00, 0x0A, 0x00, 0x04, 0x00,
                0x0B, 0x00, 0x04, 0x00, 0x0C, 0x00, 0x04, 0x00, 0x0D, 0x00, 0x04, 0x00, 0x0E, 0x00, 0x04, 0x00,
                0x0F, 0x00, 0x04, 0x00, 0x10, 0x00, 0x04, 0x00, 0x11, 0x00, 0x04, 0x00, 0x12, 0x00, 0x04, 0x00,
                0x13, 0x00, 0x04, 0x00, 0x14, 0x00, 0x06, 0x00, 0x01, 0x00, 0x06, 0x00, 0x02, 0x00, 0x06, 0x00,
                0x03, 0x00, 0x08, 0x00, 0x01, 0x00, 0x08, 0x00, 0x02, 0x00, 0x09, 0x00, 0x01, 0x00, 0x09, 0x00,
                0x02, 0x00, 0x09, 0x00, 0x03, 0x00, 0x09, 0x00, 0x04, 0x00, 0x09, 0x00, 0x09, 0x00, 0x09, 0x00,
                0x0A, 0x00, 0x09, 0x00, 0x0B, 0x00, 0x0A, 0x00, 0x01, 0x00, 0x0A, 0x00, 0x02, 0x00, 0x0A, 0x00,
                0x03, 0x00, 0x0B, 0x00, 0x01, 0x00, 0x0B, 0x00, 0x02, 0x00, 0x0B, 0x00, 0x03, 0x00, 0x0B, 0x00,
                0x04, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x0C, 0x00, 0x02, 0x00, 0x0C, 0x00, 0x03, 0x00, 0x13, 0x00,
                0x01, 0x00, 0x13, 0x00, 0x02, 0x00, 0x13, 0x00, 0x03, 0x00, 0x13, 0x00, 0x04, 0x00, 0x13, 0x00,
                0x05, 0x00, 0x13, 0x00, 0x06, 0x00, 0x13, 0x00, 0x07, 0x00, 0x13, 0x00, 0x08, 0x00, 0x13, 0x00,
                0x09, 0x00, 0x13, 0x00, 0x0A, 0x00, 0x13, 0x00, 0x0B, 0x00, 0x13, 0x00, 0x0C, 0x00, 0x13, 0x00,
                0x0D, 0x00, 0x13, 0x00, 0x0E, 0x00, 0x13, 0x00, 0x0F, 0x00, 0x13, 0x00, 0x10, 0x00, 0x13, 0x00,
                0x11, 0x00, 0x13, 0x00, 0x12, 0x00, 0x13, 0x00, 0x13, 0x00, 0x13, 0x00, 0x14, 0x00, 0x13, 0x00,
                0x15, 0x00, 0x13, 0x00, 0x16, 0x00, 0x13, 0x00, 0x17, 0x00, 0x13, 0x00, 0x18, 0x00, 0x13, 0x00,
                0x19, 0x00, 0x13, 0x00, 0x1A, 0x00, 0x13, 0x00, 0x1B, 0x00, 0x13, 0x00, 0x1C, 0x00, 0x13, 0x00,
                0x1D, 0x00, 0x13, 0x00, 0x1E, 0x00, 0x13, 0x00, 0x1F, 0x00, 0x13, 0x00, 0x20, 0x00, 0x13, 0x00,
                0x21, 0x00, 0x13, 0x00, 0x22, 0x00, 0x13, 0x00, 0x23, 0x00, 0x13, 0x00, 0x24, 0x00, 0x13, 0x00,
                0x25, 0x00, 0x13, 0x00, 0x26, 0x00, 0x13, 0x00, 0x27, 0x00, 0x13, 0x00, 0x28, 0x00, 0x15, 0x00,
                0x01, 0x00, 0x15, 0x00, 0x02, 0x00, 0x15, 0x00, 0x03, 0x00, 0x02, 0x00, 0x06, 0x00, 0x03, 0x00,
                0x04, 0x00, 0x03, 0x00, 0x05, 0x00, 0x09, 0x00, 0x05, 0x00, 0x09, 0x00, 0x06, 0x00, 0x09, 0x00,
                0x07, 0x00, 0x09, 0x00, 0x08, 0x00, 0x03, 0x00, 0x02, 0x00, 0x02, 0x00, 0x05, 0x00, 0x04, 0x00,
                0x06, 0x00, 0x04, 0x00, 0x02, 0x00, 0x02, 0x00, 0x09, 0x00, 0x02, 0x00, 0x0B, 0x00, 0x05, 0x00,
                0x00
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0107>(f);

            Assert.Inconclusive("Verify that Snac0107 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0108DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0x97, 0x00, 0x14,
                0x00, 0x01, 0x00, 0x08,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x08,
                0x00, 0x01,
                0x00, 0x02,
                0x00, 0x03,
                0x00, 0x04,
                0x00, 0x05
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0108>(f);

            Assert.AreEqual(5, s.RateGroupIds.Count);
            CollectionAssert.AreEqual(new[] {1, 2, 3, 4, 5}, s.RateGroupIds.ToArray());
        }

        [TestMethod]
        public void Snac010ADeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x39, 0x00, 0x00, 0x37,
                0x00, 0x01, 0x00, 0x0A,
                0x80, 0x00, 0x85, 0x1B, 0x57, 0x65,
                0x00, 0x06, // length
                0x00, 0x01, 0x00, 0x02, 0x00, 0x03, // dont know
                0x00, 0x02, // message code
                0x00, 0x03, // rate class ID
                0x00, 0x00, 0x00, 0x14, // window size
                0x00, 0x00, 0x13, 0xEC, // clear level
                0x00, 0x00, 0x13, 0x88, // alert level
                0x00, 0x00, 0x0F, 0xA0, // limit level
                0x00, 0x00, 0x0B, 0xB8, // disconnect level
                0x00, 0x00, 0x13, 0x17, // current level
                0x00, 0x00, 0x17, 0x70, // max level
                0x00, 0x00, 0x00, 0x00, // last time
                0x00
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac010A>(f);

            Assert.AreEqual(1, s.RateClasses.Count);

            RateClass rateClass = s.RateClasses[0];

            Assert.AreEqual(3, rateClass.ClassId);

            Assert.AreEqual(0x14, rateClass.WindowSize);
            Assert.AreEqual(0x13EC, rateClass.ClearLevel);
            Assert.AreEqual(0x1388, rateClass.AlertLevel);
            Assert.AreEqual(0xFA0, rateClass.LimitLevel);
            Assert.AreEqual(0xBB8, rateClass.DisconnectLevel);
            Assert.AreEqual(0x1317, rateClass.CurrentLevel);
            Assert.AreEqual(0x1770, rateClass.MaxLevel);
            Assert.AreEqual(0x0000, rateClass.LastTime);

            Assert.AreEqual(0, rateClass.CurrentState);
        }

        [TestMethod]
        public void Snac010BDeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE5, 0x65, 0x00, 0x0A,
                0x00, 0x01, 0x00, 0x0B,
                0x00, 0x00, 0x8E, 0xCC, 0xC4, 0xDF
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac010B>(f);

            Assert.AreEqual(0, s.RateGroupIds.Count);
        }

        [TestMethod]
        public void Snac010CDeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x1F, 0xBD, 0x00, 0x18,
                0x00, 0x01, 0x00, 0x0C,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x0C,
                0x00, 0x01, // family numbers
                0x00, 0x02,
                0x00, 0x03,
                0x00, 0x15,
                0x00, 0x04,
                0x00, 0x09,
                0x00, 0x0A
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac010C>(f);

            Assert.AreEqual(7, s.RateGroupIds.Count);
            CollectionAssert.AreEqual(new[] {0x1, 0x2, 0x3, 0x15, 0x4, 0x9, 0xA}, s.RateGroupIds.ToArray());
        }

        [TestMethod]
        public void Snac010DDeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE5, 0x65, 0x00, 0x0A,
                0x00, 0x01, 0x00, 0x0D,
                0x00, 0x00, 0x8E, 0xCC, 0xC4, 0xDE
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac010D>(f);

            Assert.AreEqual(0, s.RateGroupIds.Count);
        }

        [TestMethod]
        public void Snac010EDeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0x98, 0x00, 0x0A,
                0x00, 0x01, 0x00, 0x0E,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x0E
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac010E>(f);
        }

        [TestMethod]
        public void Snac010FDeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x02, 0x00, 0x75, // FLAP
                0x00, 0x01, 0x00, 0x0F, // SNAC
                0x00, 0x00, 0x00, 0x00, 0x00, 0x0E, // SNAC II
                0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x35, // UIN Length + UIN
                0x00, 0x00, // warning level
                0x00, 0x08, // number of TLVs
                0x00, 0x01, 0x00, 0x02, // TLV01; lenght 02
                0x00, 0x80,
                0x00, 0x0C, 0x00, 0x25, // TLV0C; length 37
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x0A, 0x00, 0x04, // TLV0A; length 4
                0x3E, 0x4C, 0xCF, 0xDE,
                0x00, 0x0F, 0x00, 0x04, // TLV0F; length 4
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x03, 0x00, 0x04, // TLV03; length 4
                0x3D, 0xD4, 0xCB, 0x2F,
                0x00, 0x0A, 0x00, 0x04, // TLV0A; length 4
                0x3E, 0x4C, 0xCF, 0xDE,
                0x00, 0x1E, 0x00, 0x04, // TLV1E; length 4
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x05, 0x00, 0x04, // TLV05; length 4
                0x34, 0x98, 0xDC, 0x74
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac010F>(f);

            Assert.Inconclusive("Verify that Snac010F was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0110DeserializeTest()
        {
            Assert.Inconclusive("We do not know the shape of Snac0110.");

            var data = new byte[]
            {
                0x2A, 0x02, 0x12, 0x05, 0x00, 0x38, // FLAP
                0x00, 0x01, 0x00, 0x10, // SNAC 
                0x00, 0x00, 0x8E, 0x36, 0xC7, 0xB1, // SNAC II
                0x00, 0x0A, // new warning level
                0x07, 0x36, 0x32, 0x31, 0x38, 0x38, 0x39, 0x35, // UIN length + string
                0x00, 0x00, // warning level
                0x00, 0x04, // no of TLVs
                0x00, 0x01, 0x00, 0x02, // TLV01; length 2
                0x00, 0x06,
                0x00, 0x04, 0x00, 0x02, // TLV04; length 2
                0x00, 0x00,
                0x00, 0x0F, 0x00, 0x04, // TLV0F; length 4
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x03, 0x00, 0x04, // TLV03; length 4
                0x3D, 0xE7, 0x38, 0x8B,
                0x00, 0x01, 0x00, 0x01 // what is that????
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0110>(f);

            Assert.Inconclusive("Verify that Snac0110 was deserialized correctly.");
        }

        [TestMethod]
        public void Snac0111DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE5, 0x65, 0x00, 0x0E,
                0x00, 0x01, 0x00, 0x11,
                0x00, 0x00, 0x82, 0xE8, 0xD1, 0xD1,
                0x00, 0x00, 0x00, 0x3C
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0111>(f);

            Assert.AreEqual(TimeSpan.FromSeconds(60), s.IdleTime);
        }

        [TestMethod]
        public void Snac0112DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE5, 0x66, 0x00, 0x3A,
                0x00, 0x01, 0x00, 0x12,
                0x00, 0x00, 0x8E, 0xCC, 0xC4, 0xE0,
                0x00, 0x04, // number of family list
                0x00, 0x01, // list ...
                0x00, 0x02,
                0x00, 0x03,
                0x00, 0x15,
                0x00, 0x05, 0x00, 0x0F, // TLV05; length 15
                0x31, 0x30, 0x2E, 0x31, 0x30, 0x2E, 0x31,
                0x30, 0x2E, 0x38, 0x3A, 0x35, 0x31, 0x39, 0x30,
                0x00, 0x06, 0x00, 0x0F, // TLV06; length 15
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38,
                0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0112>(f);

            CollectionAssert.AreEqual(new[] {0x1, 0x2, 0x3, 0x15}, s.Families);
            Assert.AreEqual("10.10.10.8:5190", s.ServerAddress.ServerAddress);

            CollectionAssert.AreEqual(new byte[]
            {
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38,
                0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46
            }, s.AuthorizationCookie.AuthorizationCookie);
        }

        [TestMethod]
        public void Snac0113DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3E, 0x00, 0x00, 0x18,
                0x00, 0x01, 0x00, 0x13,
                0x00, 0x00, 0x82, 0x95, 0xE6, 0x5C,
                0x00, 0x05, // motd message type
                0x00, 0x02, 0x00, 0x02, // TLV02; length 2
                0x00, 0x1E,
                0x00, 0x03, 0x00, 0x02, // TLV03; length 2
                0x04, 0xB0
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0113>(f);
        }

        [TestMethod]
        public void Snac0114DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x12, 0x95, 0x00, 0x0E,
                0x00, 0x01, 0x00, 0x14,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x14,
                0x00, 0x00, 0x00, 0x03
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0114>(f);
        }

        [TestMethod]
        public void Snac0116DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE5, 0x65, 0x00, 0x0A, 0x00, 0x01, 0x00, 0x16, 0x00, 0x00, 0x89, 0xCD, 0xC1, 0xE1
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0116>(f);
        }

        [TestMethod]
        public void Snac0117DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x22, 0x95, 0x00, 0x32,
                0x00, 0x01, 0x00, 0x17,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x17,
                0x00, 0x01, 0x00, 0x03, // families no+version ...
                0x00, 0x13, 0x00, 0x02,
                0x00, 0x02, 0x00, 0x01,
                0x00, 0x03, 0x00, 0x01,
                0x00, 0x15, 0x00, 0x01,
                0x00, 0x04, 0x00, 0x01,
                0x00, 0x06, 0x00, 0x01,
                0x00, 0x09, 0x00, 0x01,
                0x00, 0x0A, 0x00, 0x01,
                0x00, 0x0B, 0x00, 0x01
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0117>(f);

            Assert.AreEqual(10, s.FamilyNameVersionPairs.Count);

            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[0].FamilyNumber);
            Assert.AreEqual(0x3, s.FamilyNameVersionPairs[0].FamilyVersion);

            Assert.AreEqual(0x13, s.FamilyNameVersionPairs[1].FamilyNumber);
            Assert.AreEqual(0x2, s.FamilyNameVersionPairs[1].FamilyVersion);

            Assert.AreEqual(0x2, s.FamilyNameVersionPairs[2].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[2].FamilyVersion);

            Assert.AreEqual(0x3, s.FamilyNameVersionPairs[3].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[3].FamilyVersion);

            Assert.AreEqual(0x15, s.FamilyNameVersionPairs[4].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[4].FamilyVersion);

            Assert.AreEqual(0x4, s.FamilyNameVersionPairs[5].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[5].FamilyVersion);

            Assert.AreEqual(0x6, s.FamilyNameVersionPairs[6].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[6].FamilyVersion);

            Assert.AreEqual(0x9, s.FamilyNameVersionPairs[7].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[7].FamilyVersion);

            Assert.AreEqual(0xA, s.FamilyNameVersionPairs[8].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[8].FamilyVersion);

            Assert.AreEqual(0xB, s.FamilyNameVersionPairs[9].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[9].FamilyVersion);
        }

        [TestMethod]
        public void Snac0118DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x3D, 0xFF, 0x00, 0x3A,
                0x00, 0x01, 0x00, 0x18,
                0x00, 0x00, 0x82, 0x95, 0xE6, 0x5B,
                0x00, 0x01, 0x00, 0x03,
                0x00, 0x02, 0x00, 0x01,
                0x00, 0x03, 0x00, 0x01,
                0x00, 0x04, 0x00, 0x01,
                0x00, 0x06, 0x00, 0x01,
                0x00, 0x08, 0x00, 0x01,
                0x00, 0x09, 0x00, 0x01,
                0x00, 0x0A, 0x00, 0x01,
                0x00, 0x0B, 0x00, 0x01,
                0x00, 0x0C, 0x00, 0x01,
                0x00, 0x13, 0x00, 0x04,
                0x00, 0x15, 0x00, 0x01
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0118>(f);

            Assert.AreEqual(12, s.FamilyNameVersionPairs.Count);

            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[0].FamilyNumber);
            Assert.AreEqual(0x3, s.FamilyNameVersionPairs[0].FamilyVersion);

            Assert.AreEqual(0x2, s.FamilyNameVersionPairs[1].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[1].FamilyVersion);

            Assert.AreEqual(0x3, s.FamilyNameVersionPairs[2].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[2].FamilyVersion);

            Assert.AreEqual(0x4, s.FamilyNameVersionPairs[3].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[3].FamilyVersion);

            Assert.AreEqual(0x6, s.FamilyNameVersionPairs[4].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[4].FamilyVersion);

            Assert.AreEqual(0x8, s.FamilyNameVersionPairs[5].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[5].FamilyVersion);

            Assert.AreEqual(0x9, s.FamilyNameVersionPairs[6].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[6].FamilyVersion);

            Assert.AreEqual(0xA, s.FamilyNameVersionPairs[7].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[7].FamilyVersion);

            Assert.AreEqual(0xB, s.FamilyNameVersionPairs[8].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[8].FamilyVersion);

            Assert.AreEqual(0xC, s.FamilyNameVersionPairs[9].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[9].FamilyVersion);

            Assert.AreEqual(0x13, s.FamilyNameVersionPairs[10].FamilyNumber);
            Assert.AreEqual(0x4, s.FamilyNameVersionPairs[10].FamilyVersion);

            Assert.AreEqual(0x15, s.FamilyNameVersionPairs[11].FamilyNumber);
            Assert.AreEqual(0x1, s.FamilyNameVersionPairs[11].FamilyVersion);
        }

        //[TestMethod]
        //public void Snac011EDeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0x1F, 0xB6, 0x00, 0x41, 0x00, 0x01, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 
        //        0x00, 0x06, 0x00, 0x04, 0x00, 0x03, 0x00, 0x00, 0x00, 0x08, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0C, 
        //        0x00, 0x25, 0x0A, 0x0A, 0x0A, 0x08, 0x00, 0x00, 0x26, 0x2C, 0x04, 0x00, 0x07, 0x34, 0xF9, 0xC6, 
        //        0x54, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x03, 0x3D, 0xDA, 0xEE, 0xD0, 0x3D, 0xE1, 0x8A, 
        //        0x3B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //        0x2A, 0x02, 0x23, 0x2B, 0x00, 0x12, 0x00, 0x01, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 
        //        0x00, 0x06, 0x00, 0x04, 0x20, 0x03, 0x00, 0x20, 
        //        0x2A, 0x02, 0x7D, 0x0E, 0x00, 0x4A, 0x00, 0x01, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 
        //        0x00, 0x06, 0x00, 0x04, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x25, 0x0A, 0x0A, 0x0A, 0x08, 
        //        0x00, 0x00, 0x4F, 0x24, 0x04, 0x00, 0x07, 0xF4, 0x5E, 0x6B, 0xEC, 0x00, 0x00, 0x00, 0x50, 0x00, 
        //        0x00, 0x00, 0x03, 0x3E, 0x20, 0x8F, 0xE5, 0x3D, 0xF2, 0x44, 0xFB, 0x3D, 0xF7, 0xEF, 0x47, 0x00, 
        //        0x00, 0x00, 0x11, 0x00, 0x05, 0x01, 0xE5, 0x8F, 0x20, 0x3E, 0x00, 0x12, 0x00, 0x02, 0x00, 0x00
        //    };

        //    var f = DeserializeFlap(data);
        //    var s = DeserializeSnac<Snac011E>(f);

        //    Assert.Inconclusive("Verify that Snac011E was deserialized correctly.");
        //}

        //[TestMethod]
        //public void Snac011FDeserializeTest()
        //{
        //    var data = new byte[]
        //    {
        //        0x2A, 0x02, 0xE5, 0x65, 0x00, 0x12, 0x00, 0x01, 0x00, 0x1F, 0x00, 0x00, 0x82, 0xE8, 0xD1, 0xD1, 
        //        0x03, 0xFF, 0xFF, 0xFF, 0x03, 0xFF, 0xFF, 0xFF
        //    };

        //    var f = DeserializeFlap(data);
        //    var s = DeserializeSnac<Snac011F>(f);

        //    Assert.Inconclusive("Verify that Snac011F was deserialized correctly.");
        //}

        [TestMethod]
        public void Snac0120DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xD5, 0x66, 0x00, 0x1A,
                0x00, 0x01, 0x00, 0x20,
                0x00, 0x00, 0x87, 0xE4, 0xD2, 0xC8,
                0x1D, 0xF8, 0xCB, 0xAE, 0x55, 0x23, 0xB8, 0x39, 0xA0, 0xE1, 0x0D, 0xB3, 0xA4, 0x6D, 0x3B, 0x39
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0120>(f);

            CollectionAssert.AreEqual(
                new byte[]
                {0x1D, 0xF8, 0xCB, 0xAE, 0x55, 0x23, 0xB8, 0x39, 0xA0, 0xE1, 0x0D, 0xB3, 0xA4, 0x6D, 0x3B, 0x39},
                s.Md5HashArray);
        }

        [TestMethod]
        public void Snac0121DeserializeTest()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x26, 0x0F, 0x00, 0x1E,
                0x00, 0x01, 0x00, 0x21,
                0x00, 0x00, 0xC8, 0x4C, 0xDF, 0xCE,
                0x00, 0x01,
                0x41,
                0x10,
                0xFC, 0x23, 0xB3, 0xF7, 0xC8, 0x00, 0xD3, 0x92, 0xEF, 0x5E, 0x06, 0x8A, 0xD3, 0x66, 0x7F, 0xC7
            };

            Flap f = SerializationTools.DeserializeFlap(data);
            var s = SerializationTools.DeserializeSnac<Snac0121>(f);

            Assert.AreEqual(ExtendedStatusNotificationType.UploadIconRequest, s.Notification.Type);

            Assert.IsInstanceOfType(s.Notification, typeof(UploadIconNotification));

            var notification = (UploadIconNotification)s.Notification;

            Assert.AreEqual(UploadIconFlag.FirstUpload, notification.IconFlag);

            CollectionAssert.AreEqual(new byte[] { 0xFC, 0x23, 0xB3, 0xF7, 0xC8, 0x00, 0xD3, 0x92, 0xEF, 0x5E, 0x06, 0x8A, 0xD3, 0x66, 0x7F, 0xC7 }, 
                notification.IconHash);
        }
    }
}