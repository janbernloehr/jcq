// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityTest.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.IO;
using JCsTools.IdentityManager;
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
            var id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            var expected = Path.GetTempFileName();
            target.ImageUrl = expected;
            var actual = target.ImageUrl;

            Assert.AreEqual(expected, actual, false);
        }

        /// <summary>
        ///     A test for Identifier
        /// </summary>
        [TestMethod]
        public void IdentifierTest()
        {
            var id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            Assert.AreEqual(id, target.Identifier, "Identifier provided by constructor not set.");

            var expected = Guid.NewGuid().ToString();
            target.Identifier = expected;

            Assert.AreEqual(expected, target.Identifier, "Identifier provided by Property not set.");
        }

        /// <summary>
        ///     A test for Description
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            var expected = Guid.NewGuid().ToString();
            target.Description = expected;
            var actual = target.Description;

            Assert.AreEqual(expected, actual);
        }
    }
}