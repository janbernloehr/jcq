// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestAvatarAction.cs" company="Jan-Cornelius Molnar">
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

using System.Diagnostics;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class RequestAvatarAction : IAvatarServiceAction
    {
        private readonly IContact _Contact;
        private readonly IcqIconService _Service;

        public RequestAvatarAction(IcqIconService service, IContact contact)
        {
            _Service = service;
            _Contact = contact;
        }

        public IContact Contact
        {
            get { return _Contact; }
        }

        public IcqIconService Service
        {
            get { return _Service; }
        }

        public void Execute()
        {
            if (_Contact.IconHash == null)
                return;

            Snac1004 dataOut;
            dataOut = new Snac1004();
            dataOut.IconHash.AddRange(_Contact.IconHash);
            dataOut.Uin = _Contact.Identifier;

            Debug.WriteLine(string.Format("Requesting Icon for {0}.", _Contact.Identifier), "IcqIconService");

            Service.Send(dataOut);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", base.ToString(), _Contact.Identifier);
        }
    }
}