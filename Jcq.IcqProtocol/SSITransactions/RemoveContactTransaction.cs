// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveContactTransaction.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface
{
    public class RemoveContactTransaction : BaseSsiTransaction
    {
        private readonly IcqContact _contact;

        public RemoveContactTransaction(IcqStorageService owner, IcqContact contact) : base(owner)
        {
            _contact = contact;
        }

        public IcqContact Contact
        {
            get { return _contact; }
        }

        public override Snac CreateSnac()
        {
            var item = new SSIBuddyRecord
            {
                ItemId = Contact.ItemId,
                ItemName = Contact.Identifier,
                GroupId = Contact.Group.GroupId
            };

            // Create delete buddy snac
            var data = new Snac130A();
            data.BuddyRecords.Add(item);

            return data;
        }

        public override void OnComplete(SSIActionResultCode action)
        {
            switch (action)
            {
                case SSIActionResultCode.Success:
                    Service.InnerRemoveContactFromStorage(Contact);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Cannot remove contact '{0}' {1}: {2}",
                        Contact.Name, Contact.Identifier, action));
            }
        }

        public override void Validate()
        {
            if (Contact.ItemId == 0)
                throw new InvalidOperationException("Cannot remove a contact which is not a member of the contact list.");
        }
    }
}