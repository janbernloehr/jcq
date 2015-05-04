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
namespace JCsTools.Core.Configuration
{
  public class SerializerConfigElement : System.Configuration.ConfigurationElement
  {
    public SerializerConfigElement()
    {

    }

    [System.Configuration.ConfigurationProperty("ObjectType", defaultvalue = "", IsKey = true, IsRequired = true)]
    public string ObjectType {
      get { return (string)this("ObjectType"); }
      set { this("ObjectType") = value; }
    }

    [System.Configuration.ConfigurationProperty("SerializerType", defaultvalue = "", IsRequired = true)]
    public string SerializerType {
      get { return (string)this("SerializerType"); }
      set { this("SerializerType") = value; }
    }
  }
}

