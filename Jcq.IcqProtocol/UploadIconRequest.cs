// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadIconRequest.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using System.Security.Cryptography;

namespace JCsTools.JCQ.IcqInterface
{
    public class UploadIconRequest
    {
        private readonly byte[] _iconData;
        private readonly byte[] _iconMd5;

        public UploadIconRequest(byte[] data)
        {
            _iconData = data;

            using (var cg = new MD5CryptoServiceProvider())
            {
                using (var ms = new MemoryStream(IconData))
                {
                    _iconMd5 = cg.ComputeHash(ms);
                }
            }
        }

        public bool IsCompleted { get; set; }
        public bool IsAccepted { get; set; }
        public long RequestId { get; set; }

        public byte[] IconData
        {
            get { return _iconData; }
        }

        public byte[] IconMd5
        {
            get { return _iconMd5; }
        }
    }
}