// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveContactFromVisibleListTransaction.cs" company="Jan-Cornelius Molnar">
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
    public class RemoveContactFromVisibleListTransaction : BaseSSITransaction
    {
        private readonly IcqContact _Contact;

        public RemoveContactFromVisibleListTransaction(IcqStorageService owner, IcqContact contact) : base(owner)
        {
            _Contact = contact;
        }

        public IcqContact Contact
        {
            get { return _Contact; }
        }

        public override Snac CreateSnac()
        {
            Snac130A data;
            SSIPermitRecord item;

            item = new SSIPermitRecord();

            item.ItemName = Contact.Identifier;
            item.ItemId = Contact.PermitRecordItemId;

            data = new Snac130A();
            data.PermitRecords.Add(item);

            return data;
        }

        public override void OnComplete(SSIActionResultCode action)
        {
            switch (action)
            {
                case SSIActionResultCode.Success:
                    Service.InnerRemoveContactFromVisibleList(Contact);
                    break;
                default:
                    throw new InvalidOperationException(
                        string.Format("Cannot remove contact '{0}' {1} from visible list: {2}", Contact.Name,
                            Contact.Identifier, action));
                    break;
            }
        }

        public override void Validate()
        {
            if (Contact.PermitRecordItemId > 0)
                throw new InvalidOperationException("Cannot remove a contact which is not a member of the visible list.");
        }
    }
}