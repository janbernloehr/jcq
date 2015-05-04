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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCsTools.Xml.Formatter;
///<summary>
///This is a test Class for DefaultValueFormatterTest and is intended
///to contain all DefaultValueFormatterTest Unit Tests
///</summary>
namespace JCsTools.Xml.Formatter.Tests
{
  [TestClass()]
  public class XmlValueFormatterTest
  {
    private TestContext testContextInstance;

    ///<summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    public void SerializeIntegerTest()
    {
      ISerializer context = new XmlSerializer();
      int graph = Environment.TickCount;
      Type type = graph.GetType;
      IValueFormatter target = new XmlValueFormatter(context, type);
      string expected = System.Xml.XmlConvert.ToString(graph);
      string actual;
      actual = target.Serialize(graph);
      Assert.AreEqual(expected, actual);
    }

    ///<summary>
    ///A test for Serialize
    ///</summary>
    [TestMethod()]
    public void SerializeDateTest()
    {
      ISerializer context = new XmlSerializer();
      System.DateTime graph = System.DateTime.Now;
      Type type = graph.GetType;
      IValueFormatter target = new XmlValueFormatter(context, type);
      string expected = System.Xml.XmlConvert.ToString(graph, System.Xml.XmlDateTimeSerializationMode.Utc);
      string actual;
      actual = target.Serialize(graph);
      Assert.AreEqual(expected, actual);
    }

    ///<summary>
    ///A test for Deserialize
    ///</summary>
    [TestMethod()]
    public void DeserializeDateTest()
    {
      ISerializer context = new XmlSerializer();
      // The original object
      System.DateTime graph = System.DateTime.Now;
      // The original type
      Type type = graph.GetType;

      string value = System.Xml.XmlConvert.ToString(graph, System.Xml.XmlDateTimeSerializationMode.Utc);
      System.DateTime expected = System.Xml.XmlConvert.ToDateTime(value, System.Xml.XmlDateTimeSerializationMode.Utc);
      IValueFormatter target = new XmlValueFormatter(context, type);

      System.DateTime actual;
      actual = (System.DateTime)target.Deserialize(value);
      Assert.AreEqual(expected, actual);
    }

    ///<summary>
    ///A test for Deserialize
    ///</summary>
    [TestMethod()]
    public void DeserializeIntegerTest()
    {
      ISerializer context = new XmlSerializer();
      // The original object
      int graph = Environment.TickCount;
      // The original type
      Type type = graph.GetType;

      string value = System.Xml.XmlConvert.ToString(graph);
      int expected = System.Xml.XmlConvert.ToInt32(value);
      IValueFormatter target = new XmlValueFormatter(context, type);

      int actual;
      actual = (int)target.Deserialize(value);
      Assert.AreEqual(expected, actual);
    }

    ///<summary>
    ///A test for DefaultValueFormatter Constructor
    ///</summary>
    [TestMethod()]
    public void DefaultValueFormatterConstructorTest()
    {
      ISerializer context = new XmlSerializer();
      Type type = typeof(short);
      XmlValueFormatter target = new XmlValueFormatter(context, type);
      //Assert.AreEqual(target.ValueType, type)
    }
  }
}

