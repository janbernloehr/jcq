// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0112.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac0112 : Snac
    {
        private readonly TlvAuthorizationCookie _AuthorizationCookie = new TlvAuthorizationCookie();
        private readonly List<int> _Families = new List<int>();
        private readonly TlvServerAddress _ServerAddress = new TlvServerAddress();
        private int _tlvsSize;

        public Snac0112() : base(0x1, 0x12)
        {
        }

        public List<int> Families
        {
            get { return _Families; }
        }

        public TlvServerAddress ServerAddress
        {
            get { return _ServerAddress; }
        }

        public TlvAuthorizationCookie AuthorizationCookie
        {
            get { return _AuthorizationCookie; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            int familyCount = ByteConverter.ToUInt16(data.GetRange(index, 2));
            var familyIndex = 0;

            index += 2;

            while (familyIndex < familyCount)
            {
                _Families.Add(ByteConverter.ToUInt16(data.GetRange(index, 2)));

                index += 2;
                familyIndex++;
            }

            while (index + 4 <= data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x5:
                        _ServerAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x6:
                        _ServerAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                _tlvsSize += desc.TotalSize;
                index += desc.TotalSize;
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + 2*_Families.Count + _tlvsSize;
        }
    }
}