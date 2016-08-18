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

namespace Jcq.IcqProtocol.DataTypes
{
    public class FlapRequestSignInCookie : Flap
    {
        public FlapRequestSignInCookie() : base(FlapChannel.NewConnectionNegotiation)
        {
        }

        public TlvScreenName ScreenName { get; } = new TlvScreenName();

        public TlvPassword Password { get; } = new TlvPassword();

        public TlvClientIdString ClientIdString { get; } = new TlvClientIdString();

        public TlvClientId ClientId { get; } = new TlvClientId();

        public TlvClientMajorVersion ClientMajorVersion { get; } = new TlvClientMajorVersion();

        public TlvClientMinorVersion ClientMinorVersion { get; } = new TlvClientMinorVersion();

        public TlvClientLesserVersion ClientLesserVersion { get; } = new TlvClientLesserVersion();

        public TlvClientBuildNumber ClientBuildNumber { get; } = new TlvClientBuildNumber();

        public TlvClientDistributionNumber ClientDistributionNumber { get; } = new TlvClientDistributionNumber();

        public TlvClientLanguage ClientLanguage { get; } = new TlvClientLanguage();

        public TlvClientCountry ClientCountry { get; } = new TlvClientCountry();

        public override int Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 4 + ScreenName.CalculateTotalSize() + Password.CalculateTotalSize() +
                   ClientIdString.CalculateTotalSize() + ClientId.CalculateTotalSize() +
                   ClientMajorVersion.CalculateTotalSize() + ClientMinorVersion.CalculateTotalSize() +
                   ClientLesserVersion.CalculateTotalSize() + ClientBuildNumber.CalculateTotalSize() +
                   ClientDistributionNumber.CalculateTotalSize() + ClientLanguage.CalculateTotalSize() +
                   ClientCountry.CalculateTotalSize();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((uint) 1));
            data.AddRange(ScreenName.Serialize());
            data.AddRange(Password.Serialize());
            data.AddRange(ClientIdString.Serialize());
            data.AddRange(ClientId.Serialize());
            data.AddRange(ClientMajorVersion.Serialize());
            data.AddRange(ClientMinorVersion.Serialize());
            data.AddRange(ClientLesserVersion.Serialize());
            data.AddRange(ClientBuildNumber.Serialize());
            data.AddRange(ClientDistributionNumber.Serialize());
            data.AddRange(ClientLanguage.Serialize());
            data.AddRange(ClientCountry.Serialize());

            return data;
        }
    }
}