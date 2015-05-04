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
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string actual = null;
            target.ImageUrl = expected;
            actual = target.ImageUrl;
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
            string actual = null;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///     A test for SetAttribute
        /// </summary>
        [TestMethod]
        public void GetSetAttributeTest()
        {
            var id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            var key = Guid.NewGuid().ToString();
            var value = new object();
            object actual = null;

            target.SetAttribute(key, value);

            actual = target.GetAttribute(key);

            Assert.AreEqual(value, actual);
        }

        /// <summary>
        ///     A test for SetAttribute
        /// </summary>
        [TestMethod]
        public void GetSetAttributeGenericTest()
        {
            var id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            var attribute = new TestAttribute();
            var value = Guid.NewGuid().ToString();
            string actual = null;

            target.SetAttribute(attribute, value);

            actual = target.GetAttribute(attribute);

            Assert.AreEqual(value, actual);
        }

        /// <summary>
        ///     A test for GetAttributeNames
        /// </summary>
        [TestMethod]
        public void GetAttributeNamesTest()
        {
            var id = Guid.NewGuid().ToString();
            IIdentity target = new Identity(id);

            var expected = default(List<string>);

            expected = new List<string>();

            for (var i = 1; i <= 5; i++)
            {
                string attributeName = null;
                attributeName = Guid.NewGuid().ToString();
                target.SetAttribute(attributeName, new object());
                expected.Add(attributeName);
            }

            var actual = default(List<string>);

            actual = target.GetAttributeNames().ToList();
            Assert.AreEqual(expected.Count, actual.Count, "The array lenghts do not match.");

            foreach (var item in expected)
            {
                Assert.IsTrue(actual.Contains(item), "Item not found in actual list.");
            }
        }

        public class TestAttribute : IIdentityAttribute<string>
        {
            public string AttributeName
            {
                get { return "TestAttribute"; }
            }
        }

        #region "Additional test attributes"

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //<ClassInitialize()>  _
        //Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        //End Sub
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //<ClassCleanup()>  _
        //Public Shared Sub MyClassCleanup()
        //End Sub
        //
        //Use TestInitialize to run code before running each test
        //<TestInitialize()>  _
        //Public Sub MyTestInitialize()
        //End Sub
        //
        //Use TestCleanup to run code after each test has run
        //<TestCleanup()>  _
        //Public Sub MyTestCleanup()
        //End Sub
        //

        #endregion
    }
}