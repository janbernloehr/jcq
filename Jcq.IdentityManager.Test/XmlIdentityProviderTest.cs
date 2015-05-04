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
    public class XmlIdentityProviderTest
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
            var target = new XmlIdentityProvider(xmlFile);

            IIdentity id1 = new Identity("Jan-Cornelius Molnar");

            id1.Description = "Test String 1";
            id1.ImageUrl = "D:\\Pictures\\pony.jpg";
            id1.SetAttribute("Password", "blank");
            id1.SetAttribute("UIN", "148372642");

            target.Identities.Add(id1);

            IIdentity id2 = new Identity("123456");

            id2.Description = "Test String 2";
            id2.ImageUrl = "D:\\Pictures\\pony_small.jpg";
            id2.SetAttribute("Password", "geheim");
            id2.SetAttribute("UIN", "123456");

            target.Identities.Add(id2);

            target.Save();

            Assert.IsTrue(xmlFile.Length > 0, "No data was written.");

            target = new XmlIdentityProvider(xmlFile);

            target.Load();

            var identities = target.Identities;

            Assert.AreEqual(identities.Count, 2, "There should be 2 Identities in the list.");

            id1 = (from x in identities where x.Identifier == "Jan-Cornelius Molnar" select x).FirstOrDefault();

            Assert.IsNotNull(id1, "Identity 'Jan-Cornelius Molnar' was not loaded.");

            Assert.AreEqual(id1.Description, "Test String 1", "Description was not loaded.");
            Assert.AreEqual(id1.ImageUrl, "D:\\Pictures\\pony.jpg", "ImageUrl was not loaded.");

            var uinValue = (string) id1.GetAttribute("UIN");
            var passwordValue = (string) id1.GetAttribute("Password");

            Assert.AreEqual(uinValue, "148372642", "UIN Attribute was not loaded.");
            Assert.AreEqual(passwordValue, "blank", "Password Attribute was not loaded.");
        }

        /// <summary>
        ///     A test for Load
        /// </summary>
        [TestMethod]
        public void LoadTest()
        {
            dynamic xdocument = new XDocument(new XDeclaration("1.0", null, "yes"), "",
                new XElement("IdentityData",
                    new XElement("Identities", new XElement("Id", "b93e1c5f-b92d-4f89-82a1-2b9e1b2428d0"),
                        new XElement("Identifier", "123456"), new XElement("ImageUrl", "D:\\Pictures\\pony_small.jpg"),
                        new XElement("Description", "Test String 2")),
                    new XElement("Identities", new XElement("Id", "b28729bf-a90e-4397-a4ea-7cdfd36baa60"),
                        new XElement("Identifier", "Jan-Cornelius Molnar"),
                        new XElement("ImageUrl", "D:\\Pictures\\pony.jpg"), new XElement("Description", "Test String 1")),
                    new XElement("Attributes", new XElement("Id", "a2ed8862-26f3-4da5-bb57-672a6a9d1907"),
                        new XElement("IdentityId", "b93e1c5f-b92d-4f89-82a1-2b9e1b2428d0"), new XElement("Key", "UIN"),
                        new XElement("Value", new XAttribute("xsi:type", "xs:string"),
                            new XAttribute("xmlns:xs", "http://www.w3.org/2001/XMLSchema"),
                            new XAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"), "123456")),
                    new XElement("Attributes", new XElement("Id", "4ee27193-302e-4ee5-81d9-57b1f822b61c"),
                        new XElement("IdentityId", "b93e1c5f-b92d-4f89-82a1-2b9e1b2428d0"),
                        new XElement("Key", "Password"),
                        new XElement("Value", new XAttribute("xsi:type", "xs:string"),
                            new XAttribute("xmlns:xs", "http://www.w3.org/2001/XMLSchema"),
                            new XAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"), "geheim")),
                    new XElement("Attributes", new XElement("Id", "7a84c3e0-e6c9-4069-ab59-a699bf48f754"),
                        new XElement("IdentityId", "b28729bf-a90e-4397-a4ea-7cdfd36baa60"), new XElement("Key", "UIN"),
                        new XElement("Value", new XAttribute("xsi:type", "xs:string"),
                            new XAttribute("xmlns:xs", "http://www.w3.org/2001/XMLSchema"),
                            new XAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"), "148372642")),
                    new XElement("Attributes", new XElement("Id", "7e195eb9-3846-4fd6-8069-303a456688b0"),
                        new XElement("IdentityId", "b28729bf-a90e-4397-a4ea-7cdfd36baa60"),
                        new XElement("Key", "Password"),
                        new XElement("Value", new XAttribute("xsi:type", "xs:string"),
                            new XAttribute("xmlns:xs", "http://www.w3.org/2001/XMLSchema"),
                            new XAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"), "blank"))));

            var xmlFile = new FileInfo(Path.GetTempFileName());

            xdocument.Save(xmlFile.FullName);

            var target = new XmlIdentityProvider(xmlFile);
            target.Load();

            var identities = target.Identities;

            Assert.AreEqual(identities.Count, 2, "There should be 2 Identities in the list.");

            var id1 = (from x in identities where x.Identifier == "Jan-Cornelius Molnar" select x).FirstOrDefault();

            Assert.IsNotNull(id1, "Identity 'Jan-Cornelius Molnar' was not loaded.");

            Assert.AreEqual(id1.Description, "Test String 1", "Description was not loaded.");
            Assert.AreEqual(id1.ImageUrl, "D:\\Pictures\\pony.jpg", "ImageUrl was not loaded.");

            var uinValue = (string) id1.GetAttribute("UIN");
            var passwordValue = (string) id1.GetAttribute("Password");

            Assert.AreEqual(uinValue, "148372642", "UIN Attribute was not loaded.");
            Assert.AreEqual(passwordValue, "blank", "Password Attribute was not loaded.");
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