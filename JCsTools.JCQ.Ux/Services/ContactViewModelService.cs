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
namespace JCsTools.JCQ.Ux
{
  public class ContactWindowViewModelService : IcqInterface.ContextService, IContactWindowViewModelService
  {
    private Dictionary<ContactViewModel, MessageWindow> _Windows = new Dictionary<ContactViewModel, MessageWindow>();

    public ContactWindowViewModelService(IcqInterface.Interfaces.IContext context) : base(context)
    {
    }

    public ViewModel.MessageWindowViewModel ViewModel.IContactWindowViewModelService.GetMessageWindowViewModel(ViewModel.ContactViewModel contact)
    {
      lock (_Windows) {
        if (!_Windows.ContainsKey(contact)) {
          _Windows.Add(contact, new MessageWindow(contact));
        }

        return _Windows(contact).ViewModel;
      }
    }

    public bool ViewModel.IContactWindowViewModelService.IsMessageWindowViewModelAvailable(ViewModel.ContactViewModel contact)
    {
      lock (_Windows) {
        return _Windows.ContainsKey(contact);
      }
    }

    public void ViewModel.IContactWindowViewModelService.RemoveMessageWindowViewModel(ViewModel.ContactViewModel contact)
    {
      lock (_Windows) {
        _Windows.Remove(contact);
      }
    }
  }
}

