// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqContact.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Linq;
using Jcq.Core;
using Jcq.IcqProtocol.Contracts;
using Newtonsoft.Json;

namespace Jcq.IcqProtocol
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

            IconHashReceived?.Invoke(this, EventArgs.Empty);
        }

        public void SetIconData(List<byte> value)
        {
            if (IconData != null && IconData.SequenceEqual(value))
                return;

            IconData = value;

            Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon data for {0}", Identifier);

            IconDataReceived?.Invoke(this, EventArgs.Empty);
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