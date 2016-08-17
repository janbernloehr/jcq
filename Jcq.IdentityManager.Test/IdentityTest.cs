// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityTest.cs" company="Jan-Cornelius Molnar">
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
using System.IO;
using Jcq.IdentityManager.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IdentityManager.Test
{
    /// <summary>
    ///     This is a test class for IdentityTest and is intended
    ///     to contain all IdentityTest Unit Tests
    /// </summary>
    [TestClass]
    public class IdentityTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///     A test for ImageUrl
        /// </summary>
        [TestMethod]
        public void ImageUrlTest()
        {
            string id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            string expected = Path.GetTempFileName();
            target.ImageUrl = expected;
            string actual = target.ImageUrl;

            Assert.AreEqual(expected, actual, false);
        }

        /// <summary>
        ///     A test for Identifier
        /// </summary>
        [TestMethod]
        public void IdentifierTest()
        {
            string id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            Assert.AreEqual(id, target.Identifier, "Identifier provided by constructor not set.");

            string expected = Guid.NewGuid().ToString();
            target.Identifier = expected;

            Assert.AreEqual(expected, target.Identifier, "Identifier provided by Property not set.");
        }

        /// <summary>
        ///     A test for Description
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            string id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            string expected = Guid.NewGuid().ToString();
            target.Description = expected;
            string actual = target.Description;

            Assert.AreEqual(expected, actual);
        }
    }
}