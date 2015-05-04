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
        private readonly TlvClientBuildNumber _ClientBuildNumber = new TlvClientBuildNumber();
        private readonly TlvClientCountry _ClientCountry = new TlvClientCountry();
        private readonly TlvClientDistributionNumber _ClientDistributionNumber = new TlvClientDistributionNumber();
        private readonly TlvClientId _ClientId = new TlvClientId();
        private readonly TlvClientIdString _ClientIdString = new TlvClientIdString();
        private readonly TlvClientLanguage _ClientLanguage = new TlvClientLanguage();
        private readonly TlvClientLesserVersion _ClientLesserVersion = new TlvClientLesserVersion();
        private readonly TlvClientMajorVersion _ClientMajorVersion = new TlvClientMajorVersion();
        private readonly TlvClientMinorVersion _ClientMinorVersion = new TlvClientMinorVersion();
        private readonly TlvPassword _Password = new TlvPassword();
        private readonly TlvScreenName _ScreenName = new TlvScreenName();

        public FlapRequestSignInCookie() : base(FlapChannel.NewConnectionNegotiation)
        {
        }

        public TlvScreenName ScreenName
        {
            get { return _ScreenName; }
        }

        public TlvPassword Password
        {
            get { return _Password; }
        }

        public TlvClientIdString ClientIdString
        {
            get { return _ClientIdString; }
        }

        public TlvClientId ClientId
        {
            get { return _ClientId; }
        }

        public TlvClientMajorVersion ClientMajorVersion
        {
            get { return _ClientMajorVersion; }
        }

        public TlvClientMinorVersion ClientMinorVersion
        {
            get { return _ClientMinorVersion; }
        }

        public TlvClientLesserVersion ClientLesserVersion
        {
            get { return _ClientLesserVersion; }
        }

        public TlvClientBuildNumber ClientBuildNumber
        {
            get { return _ClientBuildNumber; }
        }

        public TlvClientDistributionNumber ClientDistributionNumber
        {
            get { return _ClientDistributionNumber; }
        }

        public TlvClientLanguage ClientLanguage
        {
            get { return _ClientLanguage; }
        }

        public TlvClientCountry ClientCountry
        {
            get { return _ClientCountry; }
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 4 + _ScreenName.CalculateTotalSize() + _Password.CalculateTotalSize() +
                   _ClientIdString.CalculateTotalSize() + _ClientId.CalculateTotalSize() +
                   _ClientMajorVersion.CalculateTotalSize() + _ClientMinorVersion.CalculateTotalSize() +
                   _ClientLesserVersion.CalculateTotalSize() + _ClientBuildNumber.CalculateTotalSize() +
                   _ClientDistributionNumber.CalculateTotalSize() + _ClientLanguage.CalculateTotalSize() +
                   _ClientCountry.CalculateTotalSize();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((uint) 1));
            data.AddRange(_ScreenName.Serialize());
            data.AddRange(_Password.Serialize());
            data.AddRange(_ClientIdString.Serialize());
            data.AddRange(_ClientId.Serialize());
            data.AddRange(_ClientMajorVersion.Serialize());
            data.AddRange(_ClientMinorVersion.Serialize());
            data.AddRange(_ClientLesserVersion.Serialize());
            data.AddRange(_ClientBuildNumber.Serialize());
            data.AddRange(_ClientDistributionNumber.Serialize());
            data.AddRange(_ClientLanguage.Serialize());
            data.AddRange(_ClientCountry.Serialize());

            return data;
        }
    }
}