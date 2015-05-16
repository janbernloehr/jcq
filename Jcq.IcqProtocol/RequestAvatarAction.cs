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
        private readonly IContact _contact;
        private readonly IcqIconService _service;

        public RequestAvatarAction(IcqIconService service, IContact contact)
        {
            _service = service;
            _contact = contact;
        }

        public IContact Contact
        {
            get { return _contact; }
        }

        public IcqIconService Service
        {
            get { return _service; }
        }

        public void Execute()
        {
            if (_contact.IconHash == null)
                return;

            var dataOut = new Snac1004();
            dataOut.IconHash.AddRange(_contact.IconHash);
            dataOut.Uin = _contact.Identifier;

            Debug.WriteLine(string.Format("Requesting Icon for {0}.", _contact.Identifier), "IcqIconService");

            Service.Send(dataOut);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", base.ToString(), _contact.Identifier);
        }
    }
}