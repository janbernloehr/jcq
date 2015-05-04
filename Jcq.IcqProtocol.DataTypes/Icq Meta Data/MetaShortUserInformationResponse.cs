// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaShortUserInformationResponse.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
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

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart + 2;

            if (data[index] == 0xa)
            {
                SearchSucceeded = true;
            }
            else
            {
                SearchSucceeded = false;
                return;
            }

            index += 1;

            Nickname = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(Nickname) ? 0 : Nickname.Length + 1);

            FirstName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(FirstName) ? 0 : FirstName.Length + 1);

            LastName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(LastName) ? 0 : LastName.Length + 1);

            EmailAddress = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
            index += 2 + (string.IsNullOrEmpty(EmailAddress) ? 0 : EmailAddress.Length + 1);

            if (data[index] == 0)
                AuthorizationRequired = true;
            index += 1;
            index += 1;

            Gender = data[index];
        }
    }
}