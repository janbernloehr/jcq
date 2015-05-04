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
  public class MappingConfigElement : System.Configuration.ConfigurationElement
  {
    public MappingConfigElement()
    {

    }

    [System.Configuration.ConfigurationProperty("InterfaceType", Defaultvalue = "", IsKey = true, IsRequired = true)]
    public string InterfaceType {
      get { return (string)this("InterfaceType"); }
      set { this("InterfaceType") = value; }
    }

    [System.Configuration.ConfigurationProperty("MappingType", Defaultvalue = "", IsRequired = true)]
    public string MappingType {
      get { return (string)this("MappingType"); }
      set { this("MappingType") = value; }
    }
  }
}

