// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0110.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac0110 : Snac
    {
        private readonly List<UserInfo> _UserInfos = new List<UserInfo>();

        public Snac0110() : base(0x1, 0x10)
        {
        }

        public int NewWarningLevel { get; set; }

        public List<UserInfo> UserInfos
        {
            get { return _UserInfos; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            NewWarningLevel = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            while (index < data.Count)
            {
                UserInfo info;

                info = new UserInfo();
                index += info.Deserialize(index, data);

                UserInfos.Add(info);
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + UserInfos.Sum(x => x.CalculateTotalSize());
        }
    }
}