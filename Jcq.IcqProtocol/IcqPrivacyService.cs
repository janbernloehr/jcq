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
using JCsTools.Core.Interfaces;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqPrivacyService : ContextService, IPrivacyService
    {
        private readonly IReadOnlyNotifyingCollection<IContact> _IgnoreList;
        private readonly IReadOnlyNotifyingCollection<IContact> _InvisibleList;
        private readonly IReadOnlyNotifyingCollection<IContact> _VisibleList;

        public IcqPrivacyService(IContext context) : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            var svStorage = context.GetService<IStorageService>() as IcqStorageService;

            _IgnoreList = svStorage.IgnoreList;
            _InvisibleList = svStorage.InvisibleList;
            _VisibleList = svStorage.VisibleList;
        }

        IReadOnlyNotifyingCollection<IContact> IPrivacyService.IgnoreList
        {
            get { return _IgnoreList; }
        }

        IReadOnlyNotifyingCollection<IContact> IPrivacyService.InvisibleList
        {
            get { return _InvisibleList; }
        }

        IReadOnlyNotifyingCollection<IContact> IPrivacyService.VisibleList
        {
            get { return _VisibleList; }
        }

        void IPrivacyService.AddContactToIgnoreList(IContact contact)
        {
            AddContactToIgnoreListTransaction trans;
            IcqStorageService svStorage;
            IcqContact icontact;

            svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            icontact = (IcqContact) contact;

            trans = new AddContactToIgnoreListTransaction(svStorage, icontact);

            svStorage.CommitSSITransaction(trans);
        }

        void IPrivacyService.AddContactToInvisibleList(IContact contact)
        {
            AddContactToInvisibleListTransaction trans;
            IcqStorageService svStorage;
            IcqContact icontact;

            svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            icontact = (IcqContact) contact;

            trans = new AddContactToInvisibleListTransaction(svStorage, icontact);

            svStorage.CommitSSITransaction(trans);
        }

        void IPrivacyService.AddContactToVisibleList(IContact contact)
        {
            AddContactToVisibleListTransaction trans;
            IcqStorageService svStorage;
            IcqContact icontact;

            svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            icontact = (IcqContact) contact;

            trans = new AddContactToVisibleListTransaction(svStorage, icontact);

            svStorage.CommitSSITransaction(trans);
        }

        void IPrivacyService.RemoveContactFromIgnoreList(IContact contact)
        {
            RemoveContactFromIgnoreListTransaction trans;
            IcqStorageService svStorage;
            IcqContact icontact;

            svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            icontact = (IcqContact) contact;

            trans = new RemoveContactFromIgnoreListTransaction(svStorage, icontact);

            svStorage.CommitSSITransaction(trans);
        }

        void IPrivacyService.RemoveContactFromInvisibleList(IContact contact)
        {
            RemoveContactFromInvisibleListTransaction trans;
            IcqStorageService svStorage;
            IcqContact icontact;

            svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            icontact = (IcqContact) contact;

            trans = new RemoveContactFromInvisibleListTransaction(svStorage, icontact);

            svStorage.CommitSSITransaction(trans);
        }

        void IPrivacyService.RemoveContactFromVisibleList(IContact contact)
        {
            RemoveContactFromVisibleListTransaction trans;
            IcqStorageService svStorage;
            IcqContact icontact;

            svStorage = (IcqStorageService) Context.GetService<IStorageService>();
            icontact = (IcqContact) contact;

            trans = new RemoveContactFromVisibleListTransaction(svStorage, icontact);

            svStorage.CommitSSITransaction(trans);
        }
    }
}