// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteConverterTests.cs" company="Jan-Cornelius Molnar">
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
using Jcq.IcqProtocol.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    [TestClass]
    public class ByteConverterTests
    {
        [TestMethod]
        public void GetBytesFromUInt16Test()
        {
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0}, ByteConverter.GetBytes(0));
            CollectionAssert.AreEqual(new byte[] {0x0, 0x1}, ByteConverter.GetBytes(1));
            CollectionAssert.AreEqual(new byte[] {0xFF, 0xFF}, ByteConverter.GetBytes(ushort.MaxValue));
        }

        [TestMethod]
        public void GetBytesFromUInt32Test()
        {
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0, 0x0, 0x0}, ByteConverter.GetBytes((uint) 0));
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0, 0x0, 0x1}, ByteConverter.GetBytes((uint) 1));
            CollectionAssert.AreEqual(new byte[] {0xFF, 0xFF, 0xFF, 0xFF}, ByteConverter.GetBytes(uint.MaxValue));
        }

        [TestMethod]
        public void GetBytesFromUInt64Test()
        {
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0},
                ByteConverter.GetBytes((ulong) 0));
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1},
                ByteConverter.GetBytes((ulong) 1));
            CollectionAssert.AreEqual(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF},
                ByteConverter.GetBytes(ulong.MaxValue));
        }

        [TestMethod]
        public void GetBytesLEFromUInt16Test()
        {
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0}, ByteConverter.GetBytesLE(0));
            CollectionAssert.AreEqual(new byte[] {0x1, 0x0}, ByteConverter.GetBytesLE(1));
            CollectionAssert.AreEqual(new byte[] {0xFF, 0xFF}, ByteConverter.GetBytesLE(ushort.MaxValue));
        }

        [TestMethod]
        public void GetBytesLEFromUInt32Test()
        {
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0, 0x0, 0x0}, ByteConverter.GetBytesLE((uint) 0));
            CollectionAssert.AreEqual(new byte[] {0x1, 0x0, 0x0, 0x0}, ByteConverter.GetBytesLE((uint) 1));
            CollectionAssert.AreEqual(new byte[] {0xFF, 0xFF, 0xFF, 0xFF}, ByteConverter.GetBytesLE(uint.MaxValue));
        }

        [TestMethod]
        public void GetBytesLEFromUInt64Test()
        {
            CollectionAssert.AreEqual(new byte[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0},
                ByteConverter.GetBytesLE((ulong) 0));
            CollectionAssert.AreEqual(new byte[] {0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0},
                ByteConverter.GetBytesLE((ulong) 1));
            CollectionAssert.AreEqual(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF},
                ByteConverter.GetBytesLE(ulong.MaxValue));
        }

        [TestMethod]
        public void GetBytesFromGuidTest()
        {
            // <Guid("B8B91B4F-BDF2-452B-B7A7-29542EF5444D")>
            var g_net = new Guid("01020304-0506-0708-090A-0B0C0D0E0F10");
            var g_icq = new byte[]
            {0x01, 0x02, 0x03, 0x04, 0x5, 0x6, 0x7, 0x8, 0x9, 0x0A, 0xB, 0xC, 0xD, 0xE, 0xF, 0x10};

            CollectionAssert.AreEqual(g_icq, ByteConverter.GetBytes(g_net));
        }

        [TestMethod]
        public void GetBytesForUInt32Date()
        {
            var value = new DateTime(2015, 01, 01);

            CollectionAssert.AreEqual(new Byte[] {84, 164, 142, 0}, ByteConverter.GetBytesForUInt32Date(value));
        }

        [TestMethod]
        public void GetBytesForUInt64Date()
        {
            var value = new DateTime(2245, 01, 01);

            CollectionAssert.AreEqual(new Byte[] {0, 0, 0, 2, 5, 66, 167, 0}, ByteConverter.GetBytesForUInt64Date(value));
        }
    }
}