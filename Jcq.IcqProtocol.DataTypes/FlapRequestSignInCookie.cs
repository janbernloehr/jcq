// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapRequestSignInCookie.cs" company="Jan-Cornelius Molnar">
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
    public class FlapRequestSignInCookie : Flap
    {
        private readonly TlvClientBuildNumber _clientBuildNumber = new TlvClientBuildNumber();
        private readonly TlvClientCountry _clientCountry = new TlvClientCountry();
        private readonly TlvClientDistributionNumber _clientDistributionNumber = new TlvClientDistributionNumber();
        private readonly TlvClientId _clientId = new TlvClientId();
        private readonly TlvClientIdString _clientIdString = new TlvClientIdString();
        private readonly TlvClientLanguage _clientLanguage = new TlvClientLanguage();
        private readonly TlvClientLesserVersion _clientLesserVersion = new TlvClientLesserVersion();
        private readonly TlvClientMajorVersion _clientMajorVersion = new TlvClientMajorVersion();
        private readonly TlvClientMinorVersion _clientMinorVersion = new TlvClientMinorVersion();
        private readonly TlvPassword _password = new TlvPassword();
        private readonly TlvScreenName _screenName = new TlvScreenName();

        public FlapRequestSignInCookie() : base(FlapChannel.NewConnectionNegotiation)
        {
        }

        public TlvScreenName ScreenName
        {
            get { return _screenName; }
        }

        public TlvPassword Password
        {
            get { return _password; }
        }

        public TlvClientIdString ClientIdString
        {
            get { return _clientIdString; }
        }

        public TlvClientId ClientId
        {
            get { return _clientId; }
        }

        public TlvClientMajorVersion ClientMajorVersion
        {
            get { return _clientMajorVersion; }
        }

        public TlvClientMinorVersion ClientMinorVersion
        {
            get { return _clientMinorVersion; }
        }

        public TlvClientLesserVersion ClientLesserVersion
        {
            get { return _clientLesserVersion; }
        }

        public TlvClientBuildNumber ClientBuildNumber
        {
            get { return _clientBuildNumber; }
        }

        public TlvClientDistributionNumber ClientDistributionNumber
        {
            get { return _clientDistributionNumber; }
        }

        public TlvClientLanguage ClientLanguage
        {
            get { return _clientLanguage; }
        }

        public TlvClientCountry ClientCountry
        {
            get { return _clientCountry; }
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 4 + _screenName.CalculateTotalSize() + _password.CalculateTotalSize() +
                   _clientIdString.CalculateTotalSize() + _clientId.CalculateTotalSize() +
                   _clientMajorVersion.CalculateTotalSize() + _clientMinorVersion.CalculateTotalSize() +
                   _clientLesserVersion.CalculateTotalSize() + _clientBuildNumber.CalculateTotalSize() +
                   _clientDistributionNumber.CalculateTotalSize() + _clientLanguage.CalculateTotalSize() +
                   _clientCountry.CalculateTotalSize();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((uint) 1));
            data.AddRange(_screenName.Serialize());
            data.AddRange(_password.Serialize());
            data.AddRange(_clientIdString.Serialize());
            data.AddRange(_clientId.Serialize());
            data.AddRange(_clientMajorVersion.Serialize());
            data.AddRange(_clientMinorVersion.Serialize());
            data.AddRange(_clientLesserVersion.Serialize());
            data.AddRange(_clientBuildNumber.Serialize());
            data.AddRange(_clientDistributionNumber.Serialize());
            data.AddRange(_clientLanguage.Serialize());
            data.AddRange(_clientCountry.Serialize());

            return data;
        }
    }
}