// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapRequestSignInCookie.cs" company="Jan-Cornelius Molnar">
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