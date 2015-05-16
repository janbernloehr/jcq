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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;
using Newtonsoft.Json;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqContact : BaseStorageItem, IContact //, IComparable, IComparable<IcqContact>, INotifyPropertyChanged
    {
        private bool _authorizationRequired;
        private int _denyRecordItemId;
        private string _emailAddress;
        private string _firstName;
        private ContactGender _gender;
        private int _ignoreRecordItemId;
        private int _itemId;
        private string _lastName;
        private DateTime? _lastShortUserInfoRequest;
        private DateTime _memberSince;
        private int _permitRecordItemId;
        private DateTime _signOnTime;
        private IcqStatusCode _status;

        public IcqContact()
        {
        }

        public IcqContact(string identifier, string name)
            : base(identifier, name)
        {
            Status = IcqStatusCodes.Offline;
        }

        [JsonIgnore]
        public IcqGroup Group { get; private set; }

        public IcqStatusCode Status
        {
            get { return _status; }
            set
            {
                if (Status == value) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        public List<byte> IconHash { get; private set; }
        public List<byte> IconData { get; private set; }

        public void SetIconHash(List<byte> value)
        {
            if (IconHash != null && IconHash.SequenceEqual(value))
                return;

            IconHash = value;

            Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon hash for {0}", Identifier);

            if (IconHashReceived != null)
            {
                IconHashReceived(this, EventArgs.Empty);
            }
        }

        public void SetIconData(List<byte> value)
        {
            if (IconData != null && IconData.SequenceEqual(value))
                return;

            IconData = value;

            Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon data for {0}", Identifier);

            if (IconDataReceived != null)
            {
                IconDataReceived(this, EventArgs.Empty);
            }
        }

        public event EventHandler IconDataReceived;
        public event EventHandler IconHashReceived;

        IGroup IContact.Group
        {
            get { return Group; }
        }

        IStatusCode IContact.Status
        {
            get { return Status; }
            set { Status = (IcqStatusCode) value; }
        }

        internal void SetGroup(IcqGroup group)
        {
            Group = group;
        }

        #region Simple Properties

        public int ItemId
        {
            get { return _itemId; }
            set
            {
                _itemId = value;
                OnPropertyChanged();
            }
        }

        public int DenyRecordItemId
        {
            get { return _denyRecordItemId; }
            set
            {
                _denyRecordItemId = value;
                OnPropertyChanged();
            }
        }

        public int PermitRecordItemId
        {
            get { return _permitRecordItemId; }
            set
            {
                _permitRecordItemId = value;
                OnPropertyChanged();
            }
        }

        public int IgnoreRecordItemId
        {
            get { return _ignoreRecordItemId; }
            set
            {
                _ignoreRecordItemId = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastShortUserInfoRequest
        {
            get { return _lastShortUserInfoRequest; }
            set
            {
                _lastShortUserInfoRequest = value;
                OnPropertyChanged();
            }
        }


        public DateTime MemberSince
        {
            get { return _memberSince; }
            set
            {
                _memberSince = value;
                OnPropertyChanged();
            }
        }

        public DateTime SignOnTime
        {
            get { return _signOnTime; }
            set
            {
                _signOnTime = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
            }
        }

        public ContactGender Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged();
            }
        }

        public bool AuthorizationRequired
        {
            get { return _authorizationRequired; }
            set
            {
                _authorizationRequired = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}