// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac0102.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac0102 : Snac
    {
        private readonly List<FamilyIdToolPair> _families = new List<FamilyIdToolPair>();

        public Snac0102() : base(0x1, 0x2)
        {
        }

        public List<FamilyIdToolPair> Families
        {
            get { return _families; }
        }

        public override int CalculateDataSize()
        {
            return _families.Count*FamilyIdToolPair.SizeFixPart;
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            foreach (var x in _families)
            {
                data.AddRange(x.Serialize());
            }

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var fam = new FamilyIdToolPair();

                fam.Deserialize(data.GetRange(index, 8));

                _families.Add(fam);

                index += FamilyIdToolPair.SizeFixPart;
            }

            TotalSize = index;
        }
    }
}