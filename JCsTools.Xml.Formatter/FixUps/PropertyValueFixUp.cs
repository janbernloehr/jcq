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
/// Represents a property value fixup.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;

using System.Diagnostics;


namespace JCsTools.Xml.Formatter
{
  public class PropertyValueFixUp : IFixUp
  {
    private readonly int _ReferencedValueId;
    private readonly PropertyDescriptor _PropertyDescriptor;
    private readonly object _Graph;

    public PropertyValueFixUp(int valueId, PropertyDescriptor descriptor, object graph)
    {
      _ReferencedValueId = valueId;
      _PropertyDescriptor = descriptor;
      _Graph = graph;
    }

    /// <summary>
    /// Gets the Id of the referenced value.
    /// </summary>
    public int ReferencedValueId {
      get { return _ReferencedValueId; }
    }


    /// <summary>
    /// Gets the PropertyDescriptor for the Property which needs FixUp.
    /// </summary>
    public PropertyDescriptor PropertyDescriptor {
      get { return _PropertyDescriptor; }
    }


    /// <summary>
    /// Gets the object graph which hosts the Property.
    /// </summary>
    public object Graph {
      get { return _Graph; }
    }

    public void IFixUp.Execute(ISerializer serializer)
    {
      object value = serializer.GetDeserializeObjectById(ReferencedValueId);

      PropertyDescriptor.SetValue(Graph, value);
    }
  }
}

