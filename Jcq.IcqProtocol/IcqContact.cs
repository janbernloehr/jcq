// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqContact.cs" company="Jan-Cornelius Molnar">
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqContact : BaseStorageItem, IContact, IComparable, IComparable<IcqContact>, INotifyPropertyChanged
    {
        private bool _AuthorizationRequired;
        private int _DenyRecordItemId;
        private string _EmailAddress;
        private string _FirstName;
        private ContactGender _Gender;
        private List<byte> _IconData;
        private List<byte> _IconHash;
        private int _IgnoreRecordItemId;
        private int _ItemId;
        private string _LastName;
        private DateTime? _LastShortUserInfoRequest;
        private DateTime _MemberSince;
        private int _PermitRecordItemId;
        private DateTime _SignOnTime;
        private IStatusCode _Status;

        public IcqContact()
        {
        }

        public IcqContact(string id, string name) : base(id, name)
        {
            Status = IcqStatusCodes.Offline;
        }

        public int ItemId
        {
            get { return _ItemId; }
            set
            {
                _ItemId = value;
                OnPropertyChanged();
            }
        }

        public int DenyRecordItemId
        {
            get { return _DenyRecordItemId; }
            set
            {
                _DenyRecordItemId = value;
                OnPropertyChanged();
            }
        }

        public int PermitRecordItemId
        {
            get { return _PermitRecordItemId; }
            set
            {
                _PermitRecordItemId = value;
                OnPropertyChanged();
            }
        }

        public int IgnoreRecordItemId
        {
            get { return _IgnoreRecordItemId; }
            set
            {
                _IgnoreRecordItemId = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastShortUserInfoRequest
        {
            get
            {
                var request = _LastShortUserInfoRequest;

                return request;
            }
            set { _LastShortUserInfoRequest = value; }
        }

        int IComparable.CompareTo(object obj)
        {
            var x = obj as IcqContact;

            if (x != null)
                return CompareTo(x);

            return 0;
        }

        public int CompareTo(IcqContact other)
        {
            int x;

            x = Comparer.Default.Compare(Identifier, other.Identifier);

            if (x == 0)
            {
                x = Status.CompareTo(other.Status);
            }

            return x;
        }

        public DateTime MemberSince
        {
            get { return _MemberSince; }
            set
            {
                _MemberSince = value;
                OnPropertyChanged();
            }
        }

        public DateTime SignOnTime
        {
            get { return _SignOnTime; }
            set
            {
                _SignOnTime = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged();
            }
        }

        public string EmailAddress
        {
            get { return _EmailAddress; }
            set
            {
                _EmailAddress = value;
                OnPropertyChanged();
            }
        }

        public ContactGender Gender
        {
            get
            {
               return _Gender;
            }
            set
            {
                _Gender = value;
                OnPropertyChanged();
            }
        }

        public bool AuthorizationRequired
        {
            get { return _AuthorizationRequired; }
            set
            {
                _AuthorizationRequired = value;
                OnPropertyChanged();
            }
        }

        public List<byte> IconHash
        {
            get
            {
                if (_IconHash == null)
                    return null;

                return _IconHash;
            }
            private set { _IconHash = value; }
        }

        public List<byte> IconData
        {
            get
            {
                if (_IconData == null)
                    return null;

                return _IconData;
            }
            private set { _IconData = value; }
        }

        public IGroup Group { get; private set; }

        public IStatusCode Status
        {
            get { return _Status; }
            set
            {
                if (Status == value) return;
                _Status = value;
                OnPropertyChanged();
            }
        }

        public void SetIconHash(List<byte> value)
        {
            var oldValue = IconHash;

            if (IconData != null && CompareLists(oldValue, value))
                return;

            _IconHash = value;

            Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon hash for {0}", Identifier);

            if (IconHashReceived != null)
            {
                IconHashReceived(this, EventArgs.Empty);
            }
        }

        public void SetIconData(List<byte> value)
        {
            var oldValue = IconData;

            if (CompareLists(oldValue, value))
                return;

            _IconData = value;

            Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon data for {0}", Identifier);

            if (IconDataReceived != null)
            {
                IconDataReceived(this, EventArgs.Empty);
            }
        }

        public event EventHandler IconDataReceived;
        public event EventHandler IconHashReceived;

        internal void SetGroup(IGroup @group)
        {
            Group = @group;
        }

        private bool CompareLists(List<byte> left, List<byte> right)
        {
            var ln = left == null;
            var rn = right == null;
            int lcount;
            int rcount;

            if (ln & rn)
                return true;
            if (ln != rn)
                return false;

            lcount = left.Count;
            rcount = right.Count;

            if (lcount == 0 & rcount == 0)
                return true;
            if (lcount != rcount)
                return false;

            for (var i = 0; i <= lcount - 1; i++)
            {
                if (left[i] != right[i])
                    return false;
            }

            return true;
        }
    }
}