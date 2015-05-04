// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextMessageViewModel.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class TextMessageViewModel : MessageViewModel
    {
        private readonly string _Message;
        private readonly ContactViewModel _Sender;

        public TextMessageViewModel(IMessage message, Brush foreground)
            : this(
                DateTime.Now, ContactViewModelCache.GetViewModel(message.Sender),
                ContactViewModelCache.GetViewModel(message.Recipient), message.Text, foreground)
        {
        }

        public TextMessageViewModel(DateTime created, ContactViewModel sender, ContactViewModel recipient,
            string message, Brush foreground) : base(created, recipient, foreground)
        {
            _Sender = sender;
            _Message = message;
        }

        public ContactViewModel Sender
        {
            get { return _Sender; }
        }

        public string Message
        {
            get { return _Message; }
        }
    }
}