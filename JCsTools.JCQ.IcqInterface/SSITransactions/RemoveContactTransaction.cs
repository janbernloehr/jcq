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
namespace JCsTools.JCQ.IcqInterface
{
  public class RemoveContactTransaction : BaseSSITransaction
  {
    private readonly IcqContact _Contact;

    public RemoveContactTransaction(IcqStorageService owner, IcqContact contact) : base(owner)
    {
      _Contact = contact;
    }

    public IcqContact Contact {
      get { return _Contact; }
    }

    public override DataTypes.Snac CreateSnac()
    {
      DataTypes.Snac130A data;
      DataTypes.SSIBuddyRecord item;

      item = new DataTypes.SSIBuddyRecord();

      item.ItemId = Contact.ItemId;
      item.ItemName = Contact.Identifier;
      item.GroupId = ((IcqGroup)Contact.Group).GroupId;

      // Create delete buddy snac
      data = new DataTypes.Snac130A();
      data.BuddyRecords.Add(item);

      return data;
    }

    public override void OnComplete(DataTypes.SSIActionResultCode action)
    {
      switch (action) {
        case DataTypes.SSIActionResultCode.Success:
          Service.InnerRemoveContactFromStorage(Contact);
          break;
        default:
          throw new InvalidOperationException(string.Format("Cannot remove contact '{0}' {1}: {2}", Contact.Name, Contact.Identifier, action));
          break;
      }
    }

    public override void Validate()
    {
      if (Contact.ItemId == 0)
        throw new InvalidOperationException("Cannot remove a contact which is not a member of the contact list.");
    }
  }
}

