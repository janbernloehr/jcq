// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsPageViewModel.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Windows.Controls;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.IdentityManager.Contracts;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.ViewModel
{
    public class ContactsPageViewModel : ViewModelBase
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
                if (_masterGroup != null) return _masterGroup;

                var svStorage = ApplicationService.Current.Context.GetService<IStorageService>();

                _masterGroup = GroupViewModelCache.GetViewModel(svStorage.MasterGroup);

                return _masterGroup;
            }
        }

        //TODO: Change to command.
        public void StartChatSessionWithContact(ContactViewModel contact)
        {
            MessageWindowViewModel vm =
                ApplicationService.Current.Context.GetService<IContactWindowViewModelService>()
                    .GetMessageWindowViewModel(contact);
            vm.Show();
        }

        public void CreateContextMenuForContact(ContextMenu menu, ContactViewModel contact)
        {
            var sv = ApplicationService.Current.Context.GetService<IContactContextMenuService>();
            menu.Items.Clear();

            foreach (MenuItem x in sv.GetMenuItems(contact))
            {
                menu.Items.Add(x);
            }
        }

        //TODO: Change to command.
        public void ChangeStatus(IcqStatusCode status)
        {
            var sv = ApplicationService.Current.Context.GetService<IStatusService>();

            sv.SetIdentityStatus(status);
        }

        //TODO: Change to command.
        public void SignOut()
        {
            var svConnect = ApplicationService.Current.Context.GetService<IConnector>();
            svConnect.SignOut();
        }
    }
}