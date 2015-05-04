// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqMessage.cs" company="Jan-Cornelius Molnar">
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

using System.Collections;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqMessage : IMessage
    {
        private readonly Hashtable _Attributes = new Hashtable();

        public IcqMessage()
        {
        }

        public IcqMessage(IContact sender, IContact recipient, string text)
        {
            _Attributes["Sender"] = sender;
            _Attributes["Recipient"] = recipient;
            _Attributes["Text"] = text;
        }

        public Hashtable Attributes
        {
            get { return _Attributes; }
        }

        public IContact Sender
        {
            get { return (IContact) _Attributes["Sender"]; }
            set { _Attributes["Sender"] = value; }
        }

        public IContact Recipient
        {
            get { return (IContact) _Attributes["Recipient"]; }
            set { _Attributes["Recipient"] = value; }
        }

        public string Text
        {
            get { return (string) _Attributes["Text"]; }
            set { _Attributes["Text"] = value; }
        }
    }
}