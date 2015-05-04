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
/// <summary>
/// The exception that is thrown when an attempt to find a service implementation for a service contract which does not exists fails.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core.Interfaces
{
  [Serializable()]
  public class ServiceNotFoundException : ArgumentException
  {
    public ServiceNotFoundException() : base(My.Resources.Strings.ServiceImplementationNotFoundMessage_Generic)
    {
    }

    public ServiceNotFoundException(string message) : base(message)
    {
    }

    public ServiceNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ServiceNotFoundException(Type interfaceType) : base(string.Format(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Strings.ServiceImplementationNotFoundMessage_WithTypeName, interfaceType.FullName))
    {
    }

    public ServiceNotFoundException(Type interfaceType, Exception innerException) : base(string.Format(System.Globalization.CultureInfo.InvariantCulture, My.Resources.Strings.ServiceImplementationNotFoundMessage_WithTypeName, interfaceType.FullName), innerException)
    {
    }

    protected ServiceNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

