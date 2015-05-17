// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonIdentityProviderTest.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using System.Linq;
using Jcq.IdentityManager.Contracts;
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

            Assert.AreEqual("Test String 1", id1.Description, "Description was not loaded.");
            Assert.AreEqual("D:\\Pictures\\pony.jpg", id1.ImageUrl, "ImageUrl was not loaded.");

            var idx = target.GetIdentityByIdentifier("123456");

            Assert.IsInstanceOfType(idx, typeof (IcqIdentity));

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