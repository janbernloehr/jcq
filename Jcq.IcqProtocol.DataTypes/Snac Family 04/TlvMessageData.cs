// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvMessageData.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Text;

namespace Jcq.IcqProtocol.DataTypes
{
    public class TlvMessageData : Tlv
    {
        public TlvMessageData()
            : base(0x2)
        {
            RequiredCapabilities.AddRange(new byte[] {1, 6});
        }

        public List<byte> RequiredCapabilities { get; } = new List<byte>();

        public string MessageText { get; set; }

        public override int CalculateDataSize()
        {
            return 4 + RequiredCapabilities.Count + 4 + 4 + 2*MessageText.Length;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            // fragment identifier
            data.Add(0x5);
            // fragment version
            data.Add(0x1);

            data.AddRange(ByteConverter.GetBytes((ushort) RequiredCapabilities.Count));
            data.AddRange(RequiredCapabilities);

            // fragment identifier
            data.Add(0x1);
            // fragment version
            data.Add(0x1);

            // Unicode (does not work ...)
            var messageBytes = Encoding.BigEndianUnicode.GetBytes(MessageText);
            data.AddRange(ByteConverter.GetBytes((ushort) (4 + messageBytes.Length)));

            data.AddRange(new byte[]
            {
                0x0,
                0x2,
                0x0,
                0x0
            });

            data.AddRange(messageBytes);

            //var messageBytes = ByteConverter.GetBytes(MessageText);
            //data.AddRange(ByteConverter.GetBytes((ushort)(4 + messageBytes.Length)));
            //data.AddRange(new byte[]
            //{
            //    0x0,
            //    0x0,
            //    0xff,
            //    0xff
            //});

            //data.AddRange(messageBytes);

            return data;
        }

        public override int Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart;

            index += 2;

            int length = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            RequiredCapabilities.AddRange(data.GetRange(index, length));
            index += length;

            index += 2;

            length = ByteConverter.ToUInt16(data.GetRange(index, 2));
            index += 2;

            index += 4;

            MessageText = Encoding.BigEndianUnicode.GetString(data.GetRange(index, length - 4).ToArray());

            index += length - 4;
            return index;
        }
    }
}