//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqContact : BaseStorageItem, Interfaces.IContact, IComparable, IComparable<IcqContact>
  {
    private Interfaces.IGroup _Group;

    public IcqContact()
    {

    }

    public IcqContact(string id, string name) : base(id, name)
    {
      Attributes("Status") = IcqStatusCodes.Offline;
    }

    internal void SetGroup(Interfaces.IGroup @group)
    {
      _Group = @group;
    }

    public int ItemId {
      get { return (int)Attributes("ItemId"); }
      set {
        Attributes("ItemId") = value;
        OnPropertyChanged("ItemId");
      }
    }

    public int DenyRecordItemId {
      get { return (int)Attributes("DenyRecordItemId"); }
      set {
        Attributes("DenyRecordItemId") = value;
        OnPropertyChanged("DenyRecordItemId");
      }
    }

    public int PermitRecordItemId {
      get { return (int)Attributes("PermitRecordItemId"); }
      set {
        Attributes("PermitRecordItemId") = value;
        OnPropertyChanged("PermitRecordItemId");
      }
    }

    public int IgnoreRecordItemId {
      get { return (int)Attributes("IgnoreRecordItemId"); }
      set {
        Attributes("IgnoreRecordItemId") = value;
        OnPropertyChanged("IgnoreRecordItemId");
      }
    }

    public DateTime Interfaces.IContact.MemberSince {
      get { return (System.DateTime)Attributes("MemberSince"); }
      set {
        Attributes("MemberSince") = value;
        OnPropertyChanged("MemberSince");
      }
    }

    public DateTime Interfaces.IContact.SignOnTime {
      get { return (System.DateTime)Attributes("SignOnTime"); }
      set {
        Attributes("SignOnTime") = value;
        OnPropertyChanged("SignOnTime");
      }
    }

    public string Interfaces.IContact.FirstName {
      get { return (string)Attributes("FirstName"); }
      set {
        Attributes("FirstName") = value;
        OnPropertyChanged("FirstName");
      }
    }

    public string Interfaces.IContact.LastName {
      get { return (string)Attributes("LastName"); }
      set {
        Attributes("LastName") = value;
        OnPropertyChanged("LastName");
      }
    }

    public string Interfaces.IContact.EmailAddress {
      get { return (string)Attributes("EmailAddress"); }
      set {
        Attributes("EmailAddress") = value;
        OnPropertyChanged("EmailAddress");
      }
    }

    public Interfaces.ContactGender Interfaces.IContact.Gender {
      get {
        byte value = (byte)Attributes("Gender");

        switch (value) {
          case 1:
            return Interfaces.ContactGender.Male;
            break;
          case 2:
            return Interfaces.ContactGender.Female;
            break;
          default:
            return Interfaces.ContactGender.Unknown;
            break;
        }
      }
      set {
        Attributes("Gender") = value;
        OnPropertyChanged("Gender");
      }
    }

    public bool Interfaces.IContact.AuthorizationRequired {
      get { return (bool)Attributes("AuthorizationRequired"); }
      set {
        Attributes("AuthorizationRequired") = value;
        OnPropertyChanged("AuthorizationRequired");
      }
    }

    public List<byte> Interfaces.IContact.IconHash {
      get {
        if (Attributes("IconHash") == null)
          return null;

        return (List<byte>)Attributes("IconHash");
      }
    }

    public List<byte> Interfaces.IContact.IconData {
      get {
        if (Attributes("IconData") == null)
          return null;

        return (List<byte>)Attributes("IconData");
      }
    }

    internal void Interfaces.IContact.SetIconHash(List<byte> value)
    {
      List<byte> oldValue;

      oldValue = IconHash;

      if (IconData != null && CompareLists(oldValue, value))
        return;

      Attributes("IconHash") = value;

      Core.Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon hash for {0}", Identifier);

      if (IconHashReceived != null) {
        IconHashReceived(this, EventArgs.Empty);
      }
    }

    private bool CompareLists(List<byte> left, List<byte> right)
    {
      bool ln = left == null;
      bool rn = right == null;
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

      for (i = 0; i <= lcount - 1; i++) {
        if (left(i) != right(i))
          return false;
      }

      return true;
    }

    internal void Interfaces.IContact.SetIconData(List<byte> value)
    {
      List<byte> oldValue;

      oldValue = IconData;

      if (CompareLists(oldValue, value))
        return;

      Attributes("IconData") = value;

      Core.Kernel.Logger.Log("IcqContact", TraceEventType.Information, "Received new icon data for {0}", Identifier);

      if (IconDataReceived != null) {
        IconDataReceived(this, EventArgs.Empty);
      }
    }

    public System.DateTime LastShortUserInfoRequest {
      get { return (System.DateTime)Attributes("LastShortUserInfoRequest"); }
      set { Attributes("LastShortUserInfoRequest") = value; }
    }

    public Interfaces.IGroup Interfaces.IContact.Group {
      get { return _Group; }
    }

    public Interfaces.IStatusCode Interfaces.IContact.Status {
      get { return (Interfaces.IStatusCode)Attributes("Status"); }
      set {
        if (!object.ReferenceEquals(Status, value)) {
          Attributes("Status") = value;
          OnPropertyChanged("Status");
        }
      }
    }

    public int System.IComparable<IcqContact>.CompareTo(IcqContact other)
    {
      int x;

      x = Comparer.Default.Compare(Identifier, other.Identifier);

      if (x == 0) {
        x = Status.CompareTo(other.Status);
      }

      return x;
    }

    public int System.IComparable.CompareTo1(object obj)
    {
      IcqContact x = obj as IcqContact;

      if (x != null)
        return CompareTo(x);
    }

    public event IconDataReceivedEventHandler IconDataReceived;
    public delegate void IconDataReceivedEventHandler(object sender, System.EventArgs e);

    public event IconHashReceivedEventHandler IconHashReceived;
    public delegate void IconHashReceivedEventHandler(object sender, System.EventArgs e);

  }
}

