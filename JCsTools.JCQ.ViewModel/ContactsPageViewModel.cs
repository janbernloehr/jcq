//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Linq;
namespace JCsTools.JCQ.ViewModel
{
  public class ContactsPageViewModel : System.Windows.Threading.DispatcherObject
  {
    private GroupViewModel _MasterGroup;

    public IEnumerable<IcqInterface.Interfaces.IStatusCode> AvailableStatuses {
      get { return ApplicationService.Current.Context.GetService<IStatusService>.AvailableStatuses; }
    }

    public IcqInterface.Interfaces.IContact Contact {
      get { return ApplicationService.Current.Context.Identity; }
    }

    public IdentityManager.IIdentity Identity {
      get { return ApplicationService.Current.Identity; }
    }

    public GroupViewModel MasterGroup {
      get {
        if (_MasterGroup == null) {
          object svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();

          _MasterGroup = GroupViewModelCache.GetViewModel(svStorage.MasterGroup);
        }

        return _MasterGroup;
      }
    }

    public void StartChatSessionWithContact(ContactViewModel contact)
    {
      object vm = ApplicationService.Current.Context.GetService<IContactWindowViewModelService>.GetMessageWindowViewModel(contact);
      vm.Show();
    }

    public void CreateContextMenuForContact(ContextMenu menu, ContactViewModel contact)
    {
      IContactContextMenuService sv;

      sv = ApplicationService.Current.Context.GetService<IContactContextMenuService>();
      menu.Items.Clear();

      foreach (MenuItem x in sv.GetMenuItems(contact)) {
        menu.Items.Add(x);
      }
    }

    public void ChangeStatus(IcqInterface.IcqStatusCode status)
    {
      IStatusService sv;

      sv = ApplicationService.Current.Context.GetService<IStatusService>();

      sv.SetIdentityStatus(status);
    }

    public void SignOut()
    {
      IcqInterface.Interfaces.IConnector svConnect;

      svConnect = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IConnector>();
      svConnect.SignOut();
    }
  }
}

