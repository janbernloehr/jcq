// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveContactFromInvisibleListTransaction.cs" company="Jan-Cornelius Molnar">
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
using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol
{
    public class RemoveContactFromInvisibleListTransaction : BaseSsiTransaction
    {
        public RemoveContactFromInvisibleListTransaction(IcqStorageService owner, IcqContact contact) : base(owner)
        {
            Contact = contact;
        }

        public IcqContact Contact { get; }

        public override Snac CreateSnac()
        {
            var item = new SSIDenyRecord
            {
                ItemName = Contact.Identifier,
                ItemId = Contact.DenyRecordItemId
            };

            var data = new Snac130A();
            data.DenyRecords.Add(item);

            return data;
        }

        public override void OnComplete(SSIActionResultCode action)
        {
            switch (action)
            {
                case SSIActionResultCode.Success:
                    Service.InnerRemoveContactFromInvisibleList(Contact);
                    break;
                default:
                    throw new InvalidOperationException(
                        string.Format("Cannot remove contact '{0}' {1} from invisible list: {2}", Contact.Name,
                            Contact.Identifier, action));
            }
        }

        public override void Validate()
        {
            if (Contact.DenyRecordItemId == 0)
                throw new InvalidOperationException(
                    "Cannot remove a contact which is not a member of the invisible list.");
        }
    }
}