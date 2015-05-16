// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlIdentityProviderTest.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using System.Linq;
using System.Xml.Linq;
using JCsTools.IdentityManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IdentityManager.Test
{
    /// <summary>
    ///     This is a test class for XmlIdentityProviderTest and is intended
    ///     to contain all XmlIdentityProviderTest Unit Tests
    /// </summary>
    [TestClass]
    public class JsonIdentityProviderTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///     A test for Save
        /// </summary>
        [TestMethod]
        public void SaveTest()
        {
            var xmlFile = new FileInfo(Path.GetTempFileName());
            var target = new JsonIdentityProvider(xmlFile);

            IIdentity id1 = new Identity("Jan-Cornelius Molnar")
            {
                Description = "Test String 1",
                ImageUrl = "D:\\Pictures\\pony.jpg"
            };

            //id1.SetAttribute("Password", "blank");
            //id1.SetAttribute("UIN", "148372642");

            target.Identities.Add(id1);

            var id2 = new IcqIdentity("123456")
            {
                Description = "Test String 2",
                ImageUrl = "D:\\Pictures\\pony_small.jpg",
                IcqPassword = "geheim",
                IcqUin = "12345"
            };


            target.Identities.Add(id2);

            target.Save();

            Assert.IsTrue(xmlFile.Length > 0, "No data was written.");

            target = new JsonIdentityProvider(xmlFile);

            target.Load();

            var identities = target.Identities;

            Assert.AreEqual(identities.Count, 2, "There should be 2 Identities in the list.");

            id1 = (from x in identities where x.Identifier == "Jan-Cornelius Molnar" select x).FirstOrDefault();

            Assert.IsNotNull(id1, "Identity 'Jan-Cornelius Molnar' was not loaded.");

            Assert.AreEqual("Test String 1", id1.Description , "Description was not loaded.");
            Assert.AreEqual("D:\\Pictures\\pony.jpg", id1.ImageUrl, "ImageUrl was not loaded.");

            var idx = target.GetIdentityByIdentifier("123456");

            Assert.IsInstanceOfType(idx, typeof(IcqIdentity));

            var idicq = (IcqIdentity) idx;

            Assert.AreEqual("12345", idicq.IcqUin, "UIN Attribute was not loaded.");
            Assert.AreEqual("geheim", idicq.IcqPassword, "Password Attribute was not loaded.");
        }

    }

    public class IcqIdentity : Identity
    {
        public IcqIdentity(string id)
            : base(id)
        {
            
        }

        public string IcqPassword { get; set; }

        public string IcqUin { get; set; }
    }
}