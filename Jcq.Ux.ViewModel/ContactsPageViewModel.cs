// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsPageViewModel.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;
using JCsTools.IdentityManager;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class ContactsPageViewModel : DispatcherObject
    {
        private GroupViewModel _masterGroup;

        public IEnumerable<IStatusCode> AvailableStatuses
        {
            get { return ApplicationService.Current.Context.GetService<IStatusService>().AvailableStatuses; }
        }

        public IContact Contact
        {
            get { return ApplicationService.Current.Context.Identity; }
        }

        public IIdentity Identity
        {
            get { return ApplicationService.Current.Identity; }
        }

        public GroupViewModel MasterGroup
        {
            get
            {
                if (_masterGroup == null)
                {
                    var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();

                    _masterGroup = GroupViewModelCache.GetViewModel(svStorage.MasterGroup);
                }

                return _masterGroup;
            }
        }

        public void StartChatSessionWithContact(ContactViewModel contact)
        {
            var vm =
                ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .GetMessageWindowViewModel(contact);
            vm.Show();
        }

        public void CreateContextMenuForContact(ContextMenu menu, ContactViewModel contact)
        {
            var sv = ApplicationService.Current.Context.GetService<IContactContextMenuService>();
            menu.Items.Clear();

            foreach (var x in sv.GetMenuItems(contact))
            {
                menu.Items.Add(x);
            }
        }

        public void ChangeStatus(IcqStatusCode status)
        {
            var sv = ApplicationService.Current.Context.GetService<IStatusService>();

            sv.SetIdentityStatus(status);
        }

        public void SignOut()
        {
            var svConnect = ApplicationService.Current.Context.GetService<IConnector>();
            svConnect.SignOut();
        }
    }
}