// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaShortUserInformationResponse.cs" company="Jan-Cornelius Molnar">
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
    public class MetaShortUserInformationResponse : MetaInformationResponse
    {
        public MetaShortUserInformationResponse() : base(MetaResponseSubType.ShortUserInformationReply)
        {
        }

        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool AuthorizationRequired { get; set; }
        public byte Gender { get; set; }
        public bool SearchSucceeded { get; set; }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            int index = SizeFixPart + 2;

            if (data[index] == 0xa)
            {
                SearchSucceeded = true;
            }
            else
            {
                SearchSucceeded = false;
                return index;
            }

            index += 1;

            Nickname = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(Nickname) ? 1 : Nickname.Length + 1);

            FirstName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(FirstName) ? 1 : FirstName.Length + 1);

            LastName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(LastName) ? 1 : LastName.Length + 1);

            EmailAddress = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(EmailAddress) ? 1 : EmailAddress.Length + 1);

            // the following is not correct
            // there are always 2*6 bytes following...

            if (data[index] == 0)
                AuthorizationRequired = true;
            index += 1;
            index += 1;

            Gender = data[index];
            index += 1;

            
            return index;
        }
    }
}