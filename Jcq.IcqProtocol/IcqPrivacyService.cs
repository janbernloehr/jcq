// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqPrivacyService.cs" company="Jan-Cornelius Molnar">
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
using System.Threading.Tasks;
using JCsTools.Core.Interfaces;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
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