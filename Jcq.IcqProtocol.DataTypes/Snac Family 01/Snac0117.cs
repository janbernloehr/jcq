// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0117.cs" company="Jan-Cornelius Molnar">
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
    public class Snac0117 : Snac
    {
        private readonly List<FamilyVersionPair> _FamilyNameVersionPairs = new List<FamilyVersionPair>();

        public Snac0117() : base(0x1, 0x17)
        {
        }

        public List<FamilyVersionPair> FamilyNameVersionPairs
        {
            get { return _FamilyNameVersionPairs; }
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var pair in _FamilyNameVersionPairs)
            {
                data.AddRange(ByteConverter.GetBytes((ushort) pair.FamilyNumber));
                data.AddRange(ByteConverter.GetBytes((ushort) pair.FamilyVersion));
            }

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return _FamilyNameVersionPairs.Count*4;
        }
    }
}