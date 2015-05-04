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
using System.Collections.ObjectModel;
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqGroup : BaseStorageItem, Interfaces.IGroup
  {
    private Core.NotifyingCollection<Interfaces.IContact> _Contacts;
    private Core.NotifyingCollection<Interfaces.IGroup> _Groups;

    public IcqGroup(string id, int groupId) : base(id, id)
    {

      _Contacts = new Core.NotifyingCollection<Interfaces.IContact>();
      _Groups = new Core.NotifyingCollection<Interfaces.IGroup>();

      Attributes("GroupId") = groupId;
    }

    public int GroupId {
      get { return (int)Attributes("GroupId"); }
      set { Attributes("GroupId") = value; }
    }

    public Core.Interfaces.INotifyingCollection<Interfaces.IContact> Interfaces.IGroup.Contacts {
      get { return _Contacts; }
    }

    public Core.Interfaces.INotifyingCollection<Interfaces.IGroup> Interfaces.IGroup.Groups {
      get { return _Groups; }
    }
  }
}

