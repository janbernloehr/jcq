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
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCsTools.Xml.Formatter;
///<summary>
///This is a test Class for XmlFormatterTest and is intended
///to contain all XmlFormatterTest Unit Tests
///</summary>
namespace JCsTools.Xml.Formatter.Tests
{
  [TestClass()]
  public class XmlFormatterTest
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
    ///A test for Serialize
    ///</summary>
    [TestMethod()]
    public void SerializeStructuredObjectTest()
    {
      XmlSerializer target = new XmlSerializer();

      object graph = new Person {
        FirstName = Environment.UserName,
        Age = Environment.TickCount,
        Address = new Address {
          StreetName = Environment.SystemDirectory,
          City = Environment.MachineName
        }
      };

      Random rand = new Random();

      for (i = 1; i <= 100; i++) {
        graph.Data.Add(Convert.ToByte(rand.Next(byte.MinValue, byte.MaxValue)));
      }

      graph.Girlfriends.Add("Angie");
      graph.Girlfriends.Add("Sahra");

      target.InitializeForUnitTest();
      target.RegisterReferenceFormatter(typeof(List<byte>), new ListOfByteFormatter(target));

      using (sw == new System.IO.StringWriter()) {
        XmlWriter writer = new System.Xml.XmlTextWriter(sw);
        target.Serialize(graph, writer);

        Assert.IsFalse(string.IsNullOrEmpty(sw.ToString));
        Assert.Inconclusive(sw.ToString);
      }
    }

    ///<summary>
    ///A test for Serialize
    ///</summary>
    [TestMethod()]
    public void DeserializeStructuredObjectTest2()
    {
      XmlSerializer target = new XmlSerializer();
      string xml = My.Resources.DummyDataDocument;

      Person graph;

      target.RegisterReferenceFormatter(typeof(List<byte>), new ListOfByteFormatter(target));

      using (sr == new System.IO.StringReader(xml)) {
        using (reader == new System.Xml.XmlTextReader(sr)) {
          graph = (Person)target.Deserialize(reader);

          Assert.IsNotNull(graph);
          Assert.AreEqual("Jan-Cornelius Molnar", graph.FirstName);
          Assert.AreEqual(12362486, graph.Age);

          Assert.IsNotNull(graph.Address);

          Assert.AreEqual("C:\\Windows\\system32", graph.Address.StreetName);
          Assert.AreEqual("JAN-NB", graph.Address.City);

          Assert.IsNotNull(graph.Girlfriends);

          Assert.IsTrue(graph.Girlfriends.Count == 2);

          Assert.AreEqual("Angie", graph.Girlfriends(0));
          Assert.AreEqual("Sahra", graph.Girlfriends(1));

          Assert.IsTrue(graph.Data.Count == 100);
        }
      }
    }

    ///<summary>
    ///A test for XmlFormatter Constructor
    ///</summary>
    [TestMethod()]
    public void XmlFormatterConstructorTest()
    {
      XmlSerializer target = new XmlSerializer();
    }
  }
}

