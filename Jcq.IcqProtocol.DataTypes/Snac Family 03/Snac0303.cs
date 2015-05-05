// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0303.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0303 : Snac
    {
        private readonly TlvMaxBuddylistSize _MaxBuddylistSize = new TlvMaxBuddylistSize();
        private readonly TlvMaxNumberOfWatchers _MaxNumberOfWatchers = new TlvMaxNumberOfWatchers();
        private readonly TlvMaxOnlineNotifications _MaxOnlineNotifications = new TlvMaxOnlineNotifications();

        public Snac0303() : base(0x3, 0x3)
        {
        }

        public TlvMaxBuddylistSize MaxBuddylistSize
        {
            get { return _MaxBuddylistSize; }
        }

        public TlvMaxNumberOfWatchers MaxNumberOfWatchers
        {
            get { return _MaxNumberOfWatchers; }
        }

        public TlvMaxOnlineNotifications MaxOnlineNotifications
        {
            get { return _MaxOnlineNotifications; }
        }

        public override int CalculateDataSize()
        {
            return _MaxBuddylistSize.CalculateTotalSize() + _MaxNumberOfWatchers.CalculateTotalSize() +
                   _MaxOnlineNotifications.CalculateTotalSize();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x1:
                        _MaxBuddylistSize.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x2:
                        _MaxNumberOfWatchers.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x3:
                        _MaxOnlineNotifications.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}