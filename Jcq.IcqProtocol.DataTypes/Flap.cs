// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Flap.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Flap : ISerializable
    {
        public const int SizeFixPart = 6;
        private readonly List<ISerializable> _DataItems = new List<ISerializable>();

        public Flap()
        {
            HasData = false;
        }

        public Flap(FlapChannel channel)
        {
            HasData = false;
            Channel = channel;
        }

        public FlapChannel Channel { get; set; }
        public int DatagramSequenceNumber { get; set; }

        public List<ISerializable> DataItems
        {
            get { return _DataItems; }
        }

        public int FlapDataSize { get; private set; }

        public int FlapTotalSize
        {
            get { return SizeFixPart + FlapDataSize; }
        }

        public int TotalSize
        {
            get { return SizeFixPart + FlapDataSize; }
        }

        public int DataSize
        {
            get { return FlapDataSize; }
        }

        public bool HasData { get; private set; }

        public virtual int CalculateDataSize()
        {
            var size = 0;
            size += _DataItems.Sum(x => x.CalculateTotalSize());
            return size;
        }

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public virtual void Deserialize(List<byte> data)
        {
            var index = 0;

            index += 1;

            Channel = (FlapChannel) data[index];
            index += 1;

            DatagramSequenceNumber = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            FlapDataSize = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            switch (Channel)
            {
                case FlapChannel.SnacData:
                    var descriptor = SnacDescriptor.GetDescriptor(index, data);
                    var x = SerializationContext.DeserializeSnac(index, descriptor, data);

                    try
                    {
                        if (x == null)
                            throw new NotImplementedException(string.Format("Snac {0} is not implemented.",
                                SnacDescriptor.GetKey(descriptor)));
                        index += x.SnacTotalSize;

                        _DataItems.Add(x);

                        if (index < FlapDataSize)
                            throw new InvalidOperationException(
                                string.Format("Deserialization of Snac {0} failed {1} bytes remaining.",
                                    SnacDescriptor.GetKey(descriptor), data.Count - index));
                    }
                    catch (Exception ex)
                    {
                        Kernel.Exceptions.PublishException(ex);
                    }
                    break;
                case FlapChannel.CloseConnectionNegotiation:
                    while (index < FlapDataSize)
                    {
                        var desc = TlvDescriptor.GetDescriptor(index, data);

                        switch (desc.TypeId)
                        {
                            case 0x1:
                                TlvScreenName tlv1;

                                tlv1 = new TlvScreenName();
                                tlv1.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv1);
                                break;
                            case 0x5:
                                TlvBosServerAddress tlv5;

                                tlv5 = new TlvBosServerAddress();
                                tlv5.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv5);
                                break;
                            case 0x6:
                                TlvAuthorizationCookie tlv6;

                                tlv6 = new TlvAuthorizationCookie();
                                tlv6.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv6);
                                break;
                            case 0x8:
                                TlvAuthFailed tlv8;

                                tlv8 = new TlvAuthFailed();
                                tlv8.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv8);
                                break;
                        }

                        index += desc.TotalSize;
                    }
                    break;
            }

            HasData = true;
        }

        public virtual List<byte> Serialize()
        {
            List<byte> data;

            FlapDataSize = CalculateDataSize();

            data = new List<byte>(SizeFixPart + FlapDataSize);

            data.Add(0x2a);
            data.Add((byte) Channel);
            data.AddRange(ByteConverter.GetBytes((ushort) DatagramSequenceNumber));

            if (FlapDataSize > UInt16.MaxValue)
                throw new OverflowException(string.Format("DataSize cannot exceed {0} bytes", UInt32.MaxValue));

            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(FlapDataSize)));

            foreach (var x in DataItems)
            {
                data.AddRange(x.Serialize());
            }

            return data;
        }
    }
}