// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Flap.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Jcq.Core;

namespace Jcq.IcqProtocol.DataTypes
{
    public class Flap : ISerializable, IFlapDescriptor
    {
        public const int SizeFixPart = 6;

        public Flap()
        {
            HasData = false;
        }

        public Flap(FlapChannel channel)
        {
            HasData = false;
            Channel = channel;
        }

        public FlapChannel Channel { get; private set; }
        public int DatagramSequenceNumber { get; set; }

        public List<ISerializable> DataItems { get; } = new List<ISerializable>();
        
        public int TotalSize => SizeFixPart + DataSize;

        public string SnacKey
        {
            get
            {
                if (Descriptor != null)
                    return Descriptor.SnacKey;

                if (Channel == FlapChannel.SnacData && DataItems.Any())
                {
                    var key = Snac.GetKey((Snac)DataItems.First());

                    return $"{key.Item1:X2},{key.Item2:X2}";
                }

                return null;
            }
        }

        public int DataSize { get; private set; }

        public bool HasData { get; private set; }

        public virtual int CalculateDataSize()
        {
            int size = 0;
            size += DataItems.Sum(x => x.CalculateTotalSize());
            return size;
        }

        public int CalculateTotalSize()
        {
            return SizeFixPart + CalculateDataSize();
        }

        public FlapDescriptor Descriptor { get; private set; }

        public virtual int Deserialize(List<byte> data)
        {
            return Deserialize(FlapDescriptor.GetDescriptor(0, data), data);
        }

        public virtual int Deserialize(FlapDescriptor descriptor, List<byte> data)
        {
            Descriptor = descriptor;

            Channel = descriptor.Channel;
            DatagramSequenceNumber = descriptor.DatagramSequenceNumber;
            DataSize = descriptor.DataSize;

            int index = SizeFixPart;

            switch (Channel)
            {
                case FlapChannel.SnacData:
                    Snac x = SerializationContext.DeserializeSnac(index, descriptor.SnacDescriptor, data);

                    try
                    {
                        if (x == null)
                            throw new NotImplementedException(
                                $"Snac {descriptor.SnacDescriptor.Key} is not implemented.");
                        index += x.TotalSize;

                        DataItems.Add(x);

                        if (index < DataSize)
                            throw new InvalidOperationException(
                                $"Deserialization of Snac {descriptor.SnacDescriptor.Key} failed {data.Count - index} bytes remaining.");
                    }
                    catch (Exception ex)
                    {
                        Kernel.Exceptions.PublishException(ex);
                    }
                    break;
                case FlapChannel.CloseConnectionNegotiation:
                    while (index < DataSize)
                    {
                        TlvDescriptor desc = TlvDescriptor.GetDescriptor(index, data);

                        switch (desc.TypeId)
                        {
                            case 0x1:
                                var tlv1 = new TlvScreenName();
                                tlv1.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv1);
                                break;
                            case 0x5:
                                var tlv5 = new TlvBosServerAddress();
                                tlv5.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv5);
                                break;
                            case 0x6:
                                var tlv6 = new TlvAuthorizationCookie();
                                tlv6.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv6);
                                break;
                            case 0x8:
                                var tlv8 = new TlvAuthFailed();
                                tlv8.Deserialize(data.GetRange(index, desc.TotalSize));

                                DataItems.Add(tlv8);
                                break;
                        }

                        index += desc.TotalSize;
                    }
                    break;
            }

            HasData = true;
            return index;
        }

        public virtual List<byte> Serialize()
        {
            DataSize = CalculateDataSize();

            var data = new List<byte>(SizeFixPart + DataSize)
            {
                0x2a,
                (byte) Channel
            };

            data.AddRange(ByteConverter.GetBytes((ushort)DatagramSequenceNumber));

            if (DataSize > ushort.MaxValue)
                throw new OverflowException($"DataSize cannot exceed {uint.MaxValue} bytes");

            data.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(DataSize)));

            foreach (ISerializable x in DataItems)
            {
                data.AddRange(x.Serialize());
            }

            return data;
        }
    }
}