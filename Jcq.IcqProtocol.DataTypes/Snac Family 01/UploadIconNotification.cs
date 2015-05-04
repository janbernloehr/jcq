// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadIconNotification.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class UploadIconNotification : ExtendedStatusNotification
    {
        private readonly List<byte> _IconHash = new List<byte>(16);
        public UploadIconFlag IconFlag { get; set; }

        public List<byte> IconHash
        {
            get { return _IconHash; }
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            byte hashLength;

            IconFlag = (UploadIconFlag) data[index];
            index += 1;
            hashLength = data[index];
            index += 1;

            _IconHash.AddRange(data.GetRange(index, hashLength));
            index += hashLength;

            SetDataSize(index);
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}