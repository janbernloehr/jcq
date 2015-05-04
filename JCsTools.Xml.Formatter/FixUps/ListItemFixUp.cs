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
/// Represents a fixup for an IList item.
/// </summary>
/// <remarks>Used for items which represent a reference type.</remarks>
using Microsoft.VisualBasic;
using System;
using System.Collections;

using System.Diagnostics;


namespace JCsTools.Xml.Formatter
{
  public class ListItemFixUp : IFixUp
  {
    private readonly int _ItemId;
    private readonly IList _List;

    public ListItemFixUp(int itemId, IList list)
    {
      _ItemId = itemId;
      _List = list;
    }

    /// <summary>
    /// Gets the Id of the list item to fix up.
    /// </summary>
    public int ItemId {
      get { return _ItemId; }
    }

    /// <summary>
    /// Gets the List to fix up.
    /// </summary>
    public IList List {
      get { return _List; }
    }

    public void IFixUp.Execute(ISerializer serializer)
    {
      object item = serializer.GetDeserializeObjectById(ItemId);

      List.Add(item);
    }
  }
}

