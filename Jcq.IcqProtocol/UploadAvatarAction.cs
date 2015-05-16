// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadAvatarAction.cs" company="Jan-Cornelius Molnar">
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
using System.Threading;
using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface
{
    public class UploadAvatarAction : IAvatarServiceAction
    {
        private static int _referenceCounter;
        private readonly byte[] _avatarBytes;
        private readonly IcqIconService _service;

        public UploadAvatarAction(IcqIconService service, byte[] avatar)
        {
            _service = service;
            _avatarBytes = avatar;
        }

        public IcqIconService Service
        {
            get { return _service; }
        }

        void IAvatarServiceAction.Execute()
        {
            var dataOut = new Snac1002
            {
                ReferenceNumber = Interlocked.Increment(ref _referenceCounter)
            };
            dataOut.IconData.AddRange(_avatarBytes);

            Debug.WriteLine("Sending Icon to server.", "IcqIconService");

            Service.Send(dataOut);
        }
    }
}