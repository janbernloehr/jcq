// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaSearchByUinResponse.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
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