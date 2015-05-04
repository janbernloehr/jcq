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
/// Represents a Property.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;

using System.Diagnostics;


namespace JCsTools.Xml.Formatter
{
  public class PropertyDescriptor
  {
    private bool _IsString;
    private bool _IsValueType;
    private bool _IsIListDescendant;
    private System.Reflection.PropertyInfo _Info;

    public PropertyDescriptor(System.Reflection.PropertyInfo info)
    {
      _Info = info;
      _IsValueType = info.PropertyType.IsValueType;
      _IsString = object.ReferenceEquals(info.PropertyType, typeof(string));
      _IsIListDescendant = PropertyType.GetInterface(typeof(IList).ToString) != null;
    }

    /// <summary>
    /// Gets a value indicating whether the property type is a value type.
    /// </summary>
    public bool IsValueType {
      get { return _IsValueType; }
    }

    /// <summary>
    /// Gets a value indicating whether the property type is System.String.
    /// </summary>
    public bool IsString {
      get { return _IsString; }
    }

    /// <summary>
    /// Gets a value indicating whether the property type is a reference type.
    /// </summary>
    /// <remarks>IsReferenceType is false for System.String.</remarks>
    public bool IsReferenceType {
      get { return !_IsValueType & !_IsString; }
    }

    /// <summary>
    /// Gets a value indicating whether the property is [readonly].
    /// </summary>
    public bool IsReadOnly {
      get { return !_Info.CanWrite; }
    }

    /// <summary>
    /// Gets a value indicating whether the property type is a descendant of System.Collections.IList.
    /// </summary>
    public bool IsIListDescendant {
      get { return _IsIListDescendant; }
    }

    /// <summary>
    /// Gets the property name.
    /// </summary>
    public string PropertyName {
      get { return _Info.Name; }
    }

    /// <summary>
    /// Gets the property type.
    /// </summary>
    public Type PropertyType {
      get { return _Info.PropertyType; }
    }

    /// <summary>
    /// Sets the value of the property.
    /// </summary>
    public void SetValue(object obj, object value)
    {
      _Info.SetValue(obj, value, null);
    }

    /// <summary>
    /// Returns the value of the property.
    /// </summary>
    public object GetValue(object obj)
    {
      return _Info.GetValue(obj, null);
    }
  }
}

