// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewModelService.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public class ContactWindowViewModelService : ContextService, IContactWindowViewModelService
    {
        private readonly Dictionary<ContactViewModel, MessageWindow> _Windows =
            new Dictionary<ContactViewModel, MessageWindow>();

        public ContactWindowViewModelService(IContext context) : base(context)
        {
        }

        public MessageWindowViewModel GetMessageWindowViewModel(ContactViewModel contact)
        {
            lock (_Windows)
            {
                if (!_Windows.ContainsKey(contact))
                {
                    _Windows.Add(contact, new MessageWindow(contact));
                }

                return _Windows[contact].ViewModel;
            }
        }

        public bool IsMessageWindowViewModelAvailable(ContactViewModel contact)
        {
            lock (_Windows)
            {
                return _Windows.ContainsKey(contact);
            }
        }

        public void RemoveMessageWindowViewModel(ContactViewModel contact)
        {
            lock (_Windows)
            {
                _Windows.Remove(contact);
            }
        }
    }
}