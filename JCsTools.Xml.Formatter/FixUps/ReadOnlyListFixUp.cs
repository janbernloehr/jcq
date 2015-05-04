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
/// Represents a fixup for a ReadOnly Property which represents an object of type IList.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;

using System.Diagnostics;


namespace JCsTools.Xml.Formatter
{
  public class ReadOnlyListPropertyFixUp : IFixUp
  {
    public ReadOnlyListPropertyFixUp(int temporaryListId, IList targetList)
    {
      _TemporaryListId = temporaryListId;
      _TargetList = targetList;
    }

    private int _TemporaryListId;
    public int TemporaryListId {
      get { return _TemporaryListId; }
      set { _TemporaryListId = value; }
    }

    private IList _TargetList;
    public IList TargetList {
      get { return _TargetList; }
    }

    public void IFixUp.Execute(ISerializer serializer)
    {
      IList temporaryList = (IList)serializer.GetDeserializeObjectById(TemporaryListId);

      foreach ( item in temporaryList) {
        _TargetList.Add(item);
      }
    }
  }
}

