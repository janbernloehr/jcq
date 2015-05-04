// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqException.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqException : ApplicationException
    {
        public IcqException(int serviceId, ErrorCode code, int subcode)
            : base(string.Format("Service: {0} ErrorCode: {1} ErrorSubCode: {2}", serviceId, code, subcode))
        {
            ServiceId = serviceId;
            ErrorCode = code;
            ErrorSubCode = subcode;
        }

        public ErrorCode ErrorCode { get; private set; }
        public int ErrorSubCode { get; private set; }
        public int ServiceId { get; private set; }
    }
}