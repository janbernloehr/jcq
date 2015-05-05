// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultReferenceFormatterTest.cs" company="Jan-Cornelius Molnar">
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
using System.Xml;
using JCsTools.Xml.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.Xml.Formatter.Tests
{
    /// <summary>
    ///     This is a test class for DefaultReferenceFormatterTest and is intended
    ///     to contain all DefaultReferenceFormatterTest Unit Tests
    /// </summary>
    [TestClass]
    public class DefaultReferenceFormatterTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///     A test for Serialize
        /// </summary>
        [TestMethod]
        public void SerializePlainObjectTest()
        {
            var parent = new XmlSerializer();
            var graph = new PlainPerson
            {
                FirstName = Environment.UserName,
                Age = Environment.TickCount
            };
            var type = graph.GetType();
            IReferenceFormatter target = new DefaultReferenceFormatter(parent, type);

            parent.InitializeForUnitTest();

            using (var sw = new StringWriter())
            {
                XmlWriter writer = new XmlTextWriter(sw);
                target.Serialize(graph, writer);

                Assert.IsFalse(string.IsNullOrEmpty(sw.ToString()));

                Assert.Inconclusive(sw.ToString());
            }
        }

        /// <summary>
        ///     A test for Serialize
        /// </summary>
        [TestMethod]
        public void SerializeStructuredObjectTest()
        {
            var parent = new XmlSerializer();
            var graph = new Person
            {
                FirstName = Environment.UserName,
                Age = Environment.TickCount,
                Address = new Address
                {
                    StreetName = Environment.SystemDirectory,
                    City = Environment.MachineName
                }
            };

            graph.Girlfriends.Add("Angie");
            graph.Girlfriends.Add("Sahra");

            var rand = new Random();

            for (var i = 1; i <= 100; i++)
            {
                graph.Data.Add(Convert.ToByte(rand.Next(byte.MinValue, byte.MaxValue)));
            }

            Type type = graph.GetType();
            IReferenceFormatter target = new DefaultReferenceFormatter(parent, type);

            parent.InitializeForUnitTest();

            using (var sw = new StringWriter())
            {
                XmlWriter writer = new XmlTextWriter(sw);
                target.Serialize(graph, writer);

                Assert.IsFalse(string.IsNullOrEmpty(sw.ToString()));
                Assert.Inconclusive(sw.ToString());
            }
        }
    }
}