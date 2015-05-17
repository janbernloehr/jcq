// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaSearchByUinResponse.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using JCsTools.Core;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class MetaSearchByUinResponse : MetaInformationResponse
    {
        public MetaSearchByUinResponse() : base(MetaResponseSubType.SearchUserFoundReply)
        {
        }

        public int FoundUserUin { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool AutorizationRequired { get; set; }
        public SearchUserStatus Status { get; set; }
        public byte Gender { get; set; }
        public int Age { get; set; }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart + 2;

            Kernel.Logger.Log("MetaSearchByUinResponse", TraceEventType.Information,
                "found at index {0}; total size: {1}", index, data.Count);

            if (data[index] == 0xa)
            {
                // Search succeeded
                index += 1;

                int dataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
                index += 2;

                Kernel.Logger.Log("MetaSearchByUinResponse", TraceEventType.Information,
                    "succeeded; data size: {0}", dataSize);

                FoundUserUin = (int) ByteConverter.ToUInt32LE(data.GetRange(index, 4));
                index += 4;

                Nickname = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
                index += 2 + (string.IsNullOrEmpty(Nickname) ? 0 : Nickname.Length + 1);

                FirstName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
                index += 2 + (string.IsNullOrEmpty(FirstName) ? 0 : FirstName.Length + 1);

                LastName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
                index += 2 + (string.IsNullOrEmpty(LastName) ? 0 : LastName.Length + 1);

                EmailAddress = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
                index += 2 + (string.IsNullOrEmpty(EmailAddress) ? 0 : EmailAddress.Length + 1);

                if (data[index] == 0)
                    AutorizationRequired = true;
                index += 1;

                Status = (SearchUserStatus) Convert.ToInt32(ByteConverter.ToUInt16LE(data.GetRange(index, 2)));
                index += 2;

                Gender = data[index];
                index += 1;

                Age = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
                index += 2;
            }
            else
            {
                Kernel.Logger.Log("MetaSearchByUinResponse", TraceEventType.Information, "invalid data.");
            }
        }
    }
}