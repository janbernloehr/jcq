// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadIconNotification.cs" company="Jan-Cornelius Molnar">
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

namespace Jcq.IcqProtocol.DataTypes
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