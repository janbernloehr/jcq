// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0121.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0121 : Snac
    {
        public Snac0121() : base(0x1, 0x21)
        {
        }

        public ExtendedStatusNotification Notification { get; private set; }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            if (data.Count > index + 2 && (data[index] == 0 & data[index + 1] == 6))
            {
                // Icq sends information about the service version

                index += 2;

                var desc = TlvDescriptor.GetDescriptor(index, data);

                index += desc.TotalSize;
            }

            ExtendedStatusNotificationType type;

            type = (ExtendedStatusNotificationType) ByteConverter.ToUInt16(data.GetRange(index, 2));

            switch (type)
            {
                case ExtendedStatusNotificationType.UploadIconRequest:
                    Notification = new UploadIconNotification();
                    break;
                case ExtendedStatusNotificationType.iChatAvialable:
                    Notification = new ChatAvailableNotification();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            Notification.Deserialize(data.GetRange(index, data.Count - index));

            index += Notification.TotalSize;

            TotalSize = index;
        }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }
    }
}