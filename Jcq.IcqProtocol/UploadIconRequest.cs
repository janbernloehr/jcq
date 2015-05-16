// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadIconRequest.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using System.Security.Cryptography;

namespace JCsTools.JCQ.IcqInterface
{
    public class UploadIconRequest
    {
        private readonly byte[] _iconData;
        private readonly byte[] _iconMd5;

        public UploadIconRequest(byte[] data)
        {
            _iconData = data;

            using (var cg = new MD5CryptoServiceProvider())
            {
                using (var ms = new MemoryStream(IconData))
                {
                    _iconMd5 = cg.ComputeHash(ms);
                }
            }
        }

        public bool IsCompleted { get; set; }
        public bool IsAccepted { get; set; }
        public long RequestId { get; set; }

        public byte[] IconData
        {
            get { return _iconData; }
        }

        public byte[] IconMd5
        {
            get { return _iconMd5; }
        }
    }
}