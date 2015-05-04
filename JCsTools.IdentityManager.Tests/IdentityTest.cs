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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCsTools.IdentityManager;
///<summary>
///This is a test Class for IdentityTest and is intended
///to contain all IdentityTest Unit Tests
///</summary>
namespace JCsTools.IdentityManager.Tests
{
  [TestClass()]
  public class IdentityTest
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
    ///A test for ImageUrl
    ///</summary>
    [TestMethod()]
    public void ImageUrlTest()
    {
      string id = Guid.NewGuid.ToString;
      IIdentity target = new Identity(id);
      string expected = System.IO.Path.GetTempFileName;
      string actual;
      target.ImageUrl = expected;
      actual = target.ImageUrl;
      Assert.AreEqual(expected, actual, false);
    }

    ///<summary>
    ///A test for Identifier
    ///</summary>
    [TestMethod()]
    public void IdentifierTest()
    {
      string id = Guid.NewGuid.ToString;
      IIdentity target = new Identity(id);

      Assert.AreEqual(id, target.Identifier, "Identifier provided by constructor not set.");

      string expected = Guid.NewGuid.ToString;

      target.Identifier = expected;

      Assert.AreEqual(expected, target.Identifier, "Identifier provided by Property not set.");
    }

    ///<summary>
    ///A test for Description
    ///</summary>
    [TestMethod()]
    public void DescriptionTest()
    {
      string id = Guid.NewGuid.ToString;
      IIdentity target = new Identity(id);
      string expected = Guid.NewGuid.ToString;
      string actual;
      target.Description = expected;
      actual = target.Description;
      Assert.AreEqual(expected, actual);
    }

    ///<summary>
    ///A test for SetAttribute
    ///</summary>
    [TestMethod()]
    public void GetSetAttributeTest()
    {
      string id = Guid.NewGuid.ToString;
      IIdentity target = new Identity(id);

      string key = Guid.NewGuid.ToString;
      object value = new object();
      object actual;

      target.SetAttribute(key, value);

      actual = target.GetAttribute(key);

      Assert.AreEqual(value, actual);
    }

    ///<summary>
    ///A test for SetAttribute
    ///</summary>
    [TestMethod()]
    public void GetSetAttributeGenericTest()
    {
      string id = Guid.NewGuid.ToString;
      IIdentity target = new Identity(id);

      TestAttribute attribute = new TestAttribute();
      string value = Guid.NewGuid.ToString;
      string actual;

      target.SetAttribute(attribute, value);

      actual = target.GetAttribute(attribute);

      Assert.AreEqual(value, actual);
    }

    public class TestAttribute : JCsTools.IdentityManager.IIdentityAttribute<string>
    {
      public string IIdentityAttribute.AttributeName {
        get { return "TestAttribute"; }
      }
    }


    ///<summary>
    ///A test for GetAttributeNames
    ///</summary>
    [TestMethod()]
    public void GetAttributeNamesTest()
    {
      string id = Guid.NewGuid.ToString;
      IIdentity target = new Identity(id);

      System.Collections.Generic.List<string> expected;

      expected = new System.Collections.Generic.List<string>();

      for (i = 1; i <= 5; i++) {
        string attributeName;
        attributeName = Guid.NewGuid.ToString;
        target.SetAttribute(attributeName, new object());
        expected.Add(attributeName);
      }

      System.Collections.Generic.List<string> actual;

      actual = target.GetAttributeNames.ToList;
      Assert.AreEqual(expected.Count, actual.Count, "The array lenghts do not match.");

      foreach ( item in expected) {
        Assert.IsTrue(actual.Contains(item), "Item not found in actual list.");
      }
    }
  }
}

