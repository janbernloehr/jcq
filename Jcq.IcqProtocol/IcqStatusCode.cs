// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqStatusCode.cs" company="Jan-Cornelius Molnar">
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
using System.Collections;
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol
{
    public class IcqStatusCode : IStatusCode, IComparable<IcqStatusCode>
    {
        private readonly int _sortIndex;

        public IcqStatusCode(string name, UserStatus status, int sortIndex)
        {
            DisplayName = name;
            UserStatus = status;
            _sortIndex = sortIndex;
        }

        public UserStatus UserStatus { get; }

        public int CompareTo(IcqStatusCode other)
        {
            return Comparer.Default.Compare(_sortIndex, other._sortIndex);
        }

        public string DisplayName { get; }

        int IComparable.CompareTo(object obj)
        {
            var other = obj as IcqStatusCode;

            return other == null ? 0 : CompareTo(other);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IcqStatusCode;

            return other != null && other.UserStatus == UserStatus;
        }

        public override int GetHashCode()
        {
            return UserStatus.GetHashCode();
        }

        public static bool operator ==(IcqStatusCode status1, IcqStatusCode status2)
        {
            if (ReferenceEquals(status1, null))
            {
                return ReferenceEquals(status2, null);
            }

            return status1.Equals(status2);
        }

        public static bool operator !=(IcqStatusCode status1, IcqStatusCode status2)
        {
            return !(status1 == status2);
        }
    }
}