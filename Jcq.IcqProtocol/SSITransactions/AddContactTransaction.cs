// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddContactTransaction.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface
{
    public class AddContactTransaction : BaseSSITransaction
    {
        private readonly IcqContact _Contact;
        private readonly IcqGroup _Group;

        public AddContactTransaction(IcqStorageService owner, IcqContact contact, IcqGroup @group) : base(owner)
        {
            _Contact = contact;
            _Group = @group;
        }

        public IcqContact Contact
        {
            get { return _Contact; }
        }

        public IcqGroup Group
        {
            get { return _Group; }
        }

        public override Snac CreateSnac()
        {
            Snac1308 data;
            SSIBuddyRecord item;

            item = new SSIBuddyRecord();

            item.ItemName = Contact.Identifier;
            item.ItemId = Service.GetNextSSIItemId();
            item.GroupId = Group.GroupId;

            data = new Snac1308();
            data.BuddyRecords.Add(item);

            return data;
        }

        public override void OnComplete(SSIActionResultCode action)
        {
            switch (action)
            {
                case SSIActionResultCode.Success:
                    Service.InnerAddContactToStorage(Contact, Group);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Cannot add contact '{0}' {1}: {2}", Contact.Name,
                        Contact.Identifier, action));
                    break;
            }
        }

        public override void Validate()
        {
            if (Contact.ItemId > 0)
                throw new InvalidOperationException("Cannot add a contact which is already member of the contact list.");
        }
    }
}