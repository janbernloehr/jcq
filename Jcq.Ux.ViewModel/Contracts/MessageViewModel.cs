// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Windows.Media;

namespace JCsTools.JCQ.ViewModel
{
    public class MessageViewModel
    {
        private readonly DateTime _DateCreated;
        private readonly ContactViewModel _Recipient;

        public MessageViewModel(DateTime created, ContactViewModel recipient, Brush foreground)
        {
            _DateCreated = created;
            _Recipient = recipient;
            Foreground = foreground;
        }

        public DateTime DateCreated
        {
            get { return _DateCreated; }
        }

        public ContactViewModel Recipient
        {
            get { return _Recipient; }
        }

        public Brush Foreground { get; private set; }
    }
}