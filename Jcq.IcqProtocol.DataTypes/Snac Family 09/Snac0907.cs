// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0907.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0907 : Snac
    {
        public Snac0907() : base(0x9, 0x7)
        {
        }

        public List<string> UsersToAdd { get; set; }

        public override int CalculateDataSize()
        {
            return UsersToAdd.Where(x => !string.IsNullOrEmpty(x)).Sum(x => x.Length);
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var uin in UsersToAdd)
            {
                data.Add((byte) uin.Length);
                data.AddRange(ByteConverter.GetBytes(uin));
            }

            return data;
        }
    }
}