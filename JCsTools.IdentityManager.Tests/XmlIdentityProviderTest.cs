//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCsTools.IdentityManager;
///<summary>
///This is a test Class for XmlIdentityProviderTest and is intended
///to contain all XmlIdentityProviderTest Unit Tests
///</summary>
namespace JCsTools.IdentityManager.Tests
{
  [TestClass()]
  public class XmlIdentityProviderTest
  {
    private TestContext testContextInstance;

    ///<summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get { return testContextInstance; }
      set { testContextInstance = Value; }
    }

#region Additional test attributes
    //
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //<ClassInitialize()>  _
    //Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    //End Sub
    //
    //Use ClassCleanup to run code after all tests in a Class have run
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


    ///<summary>
    ///A test for Save
    ///</summary>
    [TestMethod()]
    public void SaveTest()
    {
      FileInfo xmlFile = new FileInfo(Path.GetTempFileName);
      XmlIdentityProvider target = new XmlIdentityProvider(xmlFile);

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

      object identities = target.Identities;

      Assert.AreEqual(identities.Count, 2, "There should be 2 Identities in the list.");

      id1 = (from x in identitieswhere x.Identifier == "Jan-Cornelius Molnar").FirstOrDefault;

      Assert.IsNotNull(id1, "Identity 'Jan-Cornelius Molnar' was not loaded.");

      Assert.AreEqual(id1.Description, "Test String 1", "Description was not loaded.");
      Assert.AreEqual(id1.ImageUrl, "D:\\Pictures\\pony.jpg", "ImageUrl was not loaded.");

      string uinValue = id1.GetAttribute("UIN");
      string passwordValue = id1.GetAttribute("Password");

      Assert.AreEqual(uinValue, "148372642", "UIN Attribute was not loaded.");
      Assert.AreEqual(passwordValue, "blank", "Password Attribute was not loaded.");
    }

    ///<summary>
    ///A test for Load
    ///</summary>
    [TestMethod()]
    public void LoadTest()
    {
      object xdocument = ;

      FileInfo xmlFile = new FileInfo(Path.GetTempFileName);

      xdocument.Save(xmlFile.FullName);

      XmlIdentityProvider target = new XmlIdentityProvider(xmlFile);
      target.Load();

      object identities = target.Identities;

      Assert.AreEqual(identities.Count, 2, "There should be 2 Identities in the list.");

      object id1 = (from x in identitieswhere x.Identifier == "Jan-Cornelius Molnar").FirstOrDefault;

      Assert.IsNotNull(id1, "Identity 'Jan-Cornelius Molnar' was not loaded.");

      Assert.AreEqual(id1.Description, "Test String 1", "Description was not loaded.");
      Assert.AreEqual(id1.ImageUrl, "D:\\Pictures\\pony.jpg", "ImageUrl was not loaded.");

      string uinValue = id1.GetAttribute("UIN");
      string passwordValue = id1.GetAttribute("Password");

      Assert.AreEqual(uinValue, "148372642", "UIN Attribute was not loaded.");
      Assert.AreEqual(passwordValue, "blank", "Password Attribute was not loaded.");
    }
  }
}

