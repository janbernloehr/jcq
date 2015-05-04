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
using System.Xml;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCsTools.Xml.Formatter;
///<summary>
///This is a test Class for DefaultReferenceFormatterTest and is intended
///to contain all DefaultReferenceFormatterTest Unit Tests
///</summary>
namespace JCsTools.Xml.Formatter.Tests
{
  [TestClass()]
  public class DefaultReferenceFormatterTest
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
    public void SerializePlainObjectTest()
    {
      XmlSerializer parent = new XmlSerializer();
      PlainPerson graph = new PlainPerson {
        FirstName = Environment.UserName,
        Age = Environment.TickCount
      };
      Type type = graph.GetType;
      IReferenceFormatter target = new DefaultReferenceFormatter(parent, type);

      parent.InitializeForUnitTest();

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
    public void SerializeStructuredObjectTest()
    {
      XmlSerializer parent = new XmlSerializer();
      object graph = new Person {
        FirstName = Environment.UserName,
        Age = Environment.TickCount,
        Address = new Address {
          StreetName = Environment.SystemDirectory,
          City = Environment.MachineName
        }
      };

      graph.Girlfriends.Add("Angie");
      graph.Girlfriends.Add("Sahra");

      Random rand = new Random();

      for (i = 1; i <= 100; i++) {
        graph.Data.Add(Convert.ToByte(rand.Next(byte.MinValue, byte.MaxValue)));
      }

      Type type = graph.GetType;
      IReferenceFormatter target = new DefaultReferenceFormatter(parent, type);

      parent.InitializeForUnitTest();

      using (sw == new System.IO.StringWriter()) {
        XmlWriter writer = new System.Xml.XmlTextWriter(sw);
        target.Serialize(graph, writer);

        Assert.IsFalse(string.IsNullOrEmpty(sw.ToString));
        Assert.Inconclusive(sw.ToString);
      }
    }

  }


  public class PlainPerson
  {
    private string _FirstName;
    public string FirstName {
      get { return _FirstName; }
      set { _FirstName = value; }
    }

    private int _Age;
    public int Age {
      get { return _Age; }
      set { _Age = value; }
    }

  }

  public class Person
  {
    private string _FirstName;
    public string FirstName {
      get { return _FirstName; }
      set { _FirstName = value; }
    }

    private int _Age;
    public int Age {
      get { return _Age; }
      set { _Age = value; }
    }

    private Address _Address;
    public Address Address {
      get { return _Address; }
      set { _Address = value; }
    }

    private System.Collections.Generic.List<string> _Girlfriends = new System.Collections.Generic.List<string>();
    public System.Collections.Generic.List<string> Girlfriends {
      get { return _Girlfriends; }
    }

    private System.Collections.Generic.List<byte> _Data = new System.Collections.Generic.List<byte>();
    public System.Collections.Generic.List<byte> Data {
      get { return _Data; }
    }
  }

  public class Address
  {
    private string _StreetName;
    public string StreetName {
      get { return _StreetName; }
      set { _StreetName = value; }
    }

    private string _City;
    public string City {
      get { return _City; }
      set { _City = value; }
    }

  }
}

