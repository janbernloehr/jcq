// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqPrivacyService.cs" company="Jan-Cornelius Molnar">
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
using System.Threading.Tasks;
using Jcq.Core.Contracts.Collections;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    public class IcqPrivacyService : ContextService, IPrivacyService
    {
        private readonly IReadOnlyNotifyingCollection<IContact> _ignoreList;
        private readonly IReadOnlyNotifyingCollection<IContact> _invisibleList;
        private readonly IReadOnlyNotifyingCollection<IContact> _visibleList;

        public IcqPrivacyService(IContext context) : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            var svStorage = context.GetService<IStorageService>() as IcqStorageService;

            _ignoreList = svStorage.IgnoreList;
            _invisibleList = svStorage.InvisibleList;
            _visibleList = svStorage.VisibleList;
        }

        IReadOnlyNotifyingCollection<IContact> IPrivacyService.IgnoreList
        {
            get { return _ignoreList; }
        }

        IReadOnlyNotifyingCollection<IContact> IPrivacyService.InvisibleList
        {
            get { return _invisibleList; }
        }

        IReadOnlyNotifyingCollection<IContact> IPrivacyService.VisibleList
        {
            get { return _visibleList; }
        }

        Task IPrivacyService.AddContactToIgnoreList(IContact contact)
        {
            var svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            var icontact = (IcqContact) contact;

            var trans = new AddContactToIgnoreListTransaction(svStorage, icontact);

            return svStorage.CommitSSITransaction(trans);
        }

        Task IPrivacyService.AddContactToInvisibleList(IContact contact)
        {
            var svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            var icontact = (IcqContact) contact;

            var trans = new AddContactToInvisibleListTransaction(svStorage, icontact);

            return svStorage.CommitSSITransaction(trans);
        }

        Task IPrivacyService.AddContactToVisibleList(IContact contact)
        {
            var svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            var icontact = (IcqContact) contact;

            var trans = new AddContactToVisibleListTransaction(svStorage, icontact);

            return svStorage.CommitSSITransaction(trans);
        }

        Task IPrivacyService.RemoveContactFromIgnoreList(IContact contact)
        {
            var svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            var icontact = (IcqContact) contact;

            var trans = new RemoveContactFromIgnoreListTransaction(svStorage, icontact);

            return svStorage.CommitSSITransaction(trans);
        }

        Task IPrivacyService.RemoveContactFromInvisibleList(IContact contact)
        {
            var svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            var icontact = (IcqContact) contact;

            var trans = new RemoveContactFromInvisibleListTransaction(svStorage, icontact);

            return svStorage.CommitSSITransaction(trans);
        }

        Task IPrivacyService.RemoveContactFromVisibleList(IContact contact)
        {
            var svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            var icontact = (IcqContact) contact;

            var trans = new RemoveContactFromVisibleListTransaction(svStorage, icontact);

            return svStorage.CommitSSITransaction(trans);
        }
    }
}