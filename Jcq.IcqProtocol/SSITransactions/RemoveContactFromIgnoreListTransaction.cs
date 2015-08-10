// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveContactFromIgnoreListTransaction.cs" company="Jan-Cornelius Molnar">
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
    public class RemoveContactFromIgnoreListTransaction : BaseSsiTransaction
    {
        public RemoveContactFromIgnoreListTransaction(IcqStorageService owner, IcqContact contact) : base(owner)
        {
            Contact = contact;
        }

        public IcqContact Contact { get; }

        public override Snac CreateSnac()
        {
            var item = new SSIIgnoreListRecord
            {
                ItemName = Contact.Identifier,
                ItemId = Contact.IgnoreRecordItemId
            };

            var data = new Snac130A();
            data.IgnoreListRecords.Add(item);

            return data;
        }

        public override void OnComplete(SSIActionResultCode action)
        {
            switch (action)
            {
                case SSIActionResultCode.Success:
                    Service.InnerRemoveContactFromIgnoreList(Contact);
                    break;
                default:
                    throw new InvalidOperationException(
                        string.Format("Cannot remove contact '{0}' {1} from ignore list: {2}", Contact.Name,
                            Contact.Identifier, action));
            }
        }

        public override void Validate()
        {
            if (Contact.IgnoreRecordItemId == 0)
                throw new InvalidOperationException("Cannot remove a contact which is not a member of the ignore list.");
        }
    }
}