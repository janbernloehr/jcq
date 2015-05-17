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
    public class Flap : ISerializable
    {
        public const int SizeFixPart = 6;
        private readonly List<ISerializable> _dataItems = new List<ISerializable>();

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
            get { return _dataItems; }
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
            size += _dataItems.Sum(x => x.CalculateTotalSize());
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
                        index += x.TotalSize;

                        _dataItems.Add(x);

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
        }

        public virtual List<byte> Serialize()
        {
            FlapDataSize = CalculateDataSize();

            var data = new List<byte>(SizeFixPart + FlapDataSize)
            {
                0x2a,
                (byte) Channel
            };

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