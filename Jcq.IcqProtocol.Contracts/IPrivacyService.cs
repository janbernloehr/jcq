// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPrivacyService.cs" company="Jan-Cornelius Molnar">
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

using JCsTools.Core.Interfaces;

namespace JCsTools.JCQ.IcqInterface.Interfaces
{
    public interface IPrivacyService : IContextService
    {
        IReadOnlyNotifyingCollection<IContact> VisibleList { get; }
        IReadOnlyNotifyingCollection<IContact> InvisibleList { get; }
        IReadOnlyNotifyingCollection<IContact> IgnoreList { get; }
        void AddContactToVisibleList(IContact contact);
        void RemoveContactFromVisibleList(IContact contact);
        void AddContactToInvisibleList(IContact contact);
        void RemoveContactFromInvisibleList(IContact contact);
        void AddContactToIgnoreList(IContact contact);
        void RemoveContactFromIgnoreList(IContact contact);
    }
}