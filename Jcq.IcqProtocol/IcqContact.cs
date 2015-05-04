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
using System.Diagnostics;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqContact : BaseStorageItem, IContact, IComparable, IComparable<IcqContact>
    {
        public IcqContact()
        {
        }

        public IcqContact(string id, string name) : base(id, name)
        {
            Attributes["Status"] = IcqStatusCodes.Offline;
        }

        public int ItemId
        {
            get { return (int) Attributes["ItemId"]; }
            set
            {
                Attributes["ItemId"] = value;
                OnPropertyChanged("ItemId");
            }
        }

        public int DenyRecordItemId
        {
            get { return (int) Attributes["DenyRecordItemId"]; }
            set
            {
                Attributes["DenyRecordItemId"] = value;
                OnPropertyChanged("DenyRecordItemId");
            }
        }

        public int PermitRecordItemId
        {
            get { return (int) Attributes["PermitRecordItemId"]; }
            set
            {
                Attributes["PermitRecordItemId"] = value;
                OnPropertyChanged("PermitRecordItemId");
            }
        }

        public int IgnoreRecordItemId
        {
            get { return (int) Attributes["IgnoreRecordItemId"]; }
            set
            {
                Attributes["IgnoreRecordItemId"] = value;
                OnPropertyChanged("IgnoreRecordItemId");
            }
        }

        public DateTime LastShortUserInfoRequest
        {
            get { return (DateTime) Attributes["LastShortUserInfoRequest"]; }
            set { Attributes["LastShortUserInfoRequest"] = value; }
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
            get { return (DateTime) Attributes["MemberSince"]; }
            set
            {
                Attributes["MemberSince"] = value;
                OnPropertyChanged("MemberSince");
            }
        }

        public DateTime SignOnTime
        {
            get { return (DateTime) Attributes["SignOnTime"]; }
            set
            {
                Attributes["SignOnTime"] = value;
                OnPropertyChanged("SignOnTime");
            }
        }

        public string FirstName
        {
            get { return (string) Attributes["FirstName"]; }
            set
            {
                Attributes["FirstName"] = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return (string) Attributes["LastName"]; }
            set
            {
                Attributes["LastName"] = value;
                OnPropertyChanged("LastName");
            }
        }

        public string EmailAddress
        {
            get { return (string) Attributes["EmailAddress"]; }
            set
            {
                Attributes["EmailAddress"] = value;
                OnPropertyChanged("EmailAddress");
            }
        }

        public ContactGender Gender
        {
            get
            {
                var value = (byte) Attributes["Gender"];

                switch (value)
                {
                    case 1:
                        return ContactGender.Male;
                    case 2:
                        return ContactGender.Female;
                    default:
                        return ContactGender.Unknown;
                }
            }
            set
            {
                Attributes["Gender"] = value;
                OnPropertyChanged("Gender");
            }
        }

        public bool AuthorizationRequired
        {
            get { return (bool) Attributes["AuthorizationRequired"]; }
            set
            {
                Attributes["AuthorizationRequired"] = value;
                OnPropertyChanged("AuthorizationRequired");
            }
        }

        public List<byte> IconHash
        {
            get
            {
                if (Attributes["IconHash"] == null)
                    return null;

                return (List<byte>) Attributes["IconHash"];
            }
        }

        public List<byte> IconData
        {
            get
            {
                if (Attributes["IconData"] == null)
                    return null;

                return (List<byte>) Attributes["IconData"];
            }
        }

        public IGroup Group { get; private set; }

        public IStatusCode Status
        {
            get { return (IStatusCode) Attributes["Status"]; }
            set
            {
                if (!ReferenceEquals(Status, value))
                {
                    Attributes["Status"] = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public void SetIconHash(List<byte> value)
        {
            List<byte> oldValue;

            oldValue = IconHash;

            if (IconData != null && CompareLists(oldValue, value))
                return;

            Attributes["IconHash"] = value;

            Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon hash for {0}", Identifier);

            if (IconHashReceived != null)
            {
                IconHashReceived(this, EventArgs.Empty);
            }
        }

        public void SetIconData(List<byte> value)
        {
            List<byte> oldValue;

            oldValue = IconData;

            if (CompareLists(oldValue, value))
                return;

            Attributes["IconData"] = value;

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