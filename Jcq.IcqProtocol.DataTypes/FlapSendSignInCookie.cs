// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapSendSignInCookie.cs" company="Jan-Cornelius Molnar">
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
    public class FlapSendSignInCookie : Flap
    {
        private readonly TlvAuthorizationCookie _authorizationCookie = new TlvAuthorizationCookie();

        public FlapSendSignInCookie() : base(FlapChannel.NewConnectionNegotiation)
        {
        }

        public TlvAuthorizationCookie AuthorizationCookie
        {
            get { return _authorizationCookie; }
        }

        public override int CalculateDataSize()
        {
            return 4 + _authorizationCookie.CalculateTotalSize();
        }

        public override List<byte> Serialize()
        {
            var data = base.Serialize();

            data.AddRange(ByteConverter.GetBytes((uint) 1));

            data.AddRange(_authorizationCookie.Serialize());

            return data;
        }

        public override void Deserialize(List<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}