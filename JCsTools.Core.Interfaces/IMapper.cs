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
/// Provides contract to implementation mapping. Mappings can either be configured by the
/// Microkernel ConfigurationSection or using the AddImplementationMapping method at runtime.
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
  public interface IMapper
  {
    System.Type GetImplementationType<I>();
    System.Type GetImplementationType(Type interfaceType);
    I CreateImplementation<I>(params System.Object[] args);
    object CreateImplementation(Type interfaceType, params System.Object[] args);
    bool ExistsImplementation<I>();
    bool ExistsImplementation(Type interfaceType);
    void AddImplementationMapping(System.Type contractType, System.Type implementationType);
  }
}

