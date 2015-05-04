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
  public class IcqStatusCodes
  {
    private static readonly IcqStatusCode _Online = new IcqStatusCode("Online", IcqInterface.DataTypes.UserStatus.Online, 0);
    private static readonly IcqStatusCode _Free4Chat = new IcqStatusCode("Free4Chat", IcqInterface.DataTypes.UserStatus.FreeForChat, 1);
    private static readonly IcqStatusCode _Invisible = new IcqStatusCode("Invisible", IcqInterface.DataTypes.UserStatus.Invisible, 2);
    private static readonly IcqStatusCode _Away = new IcqStatusCode("Away", IcqInterface.DataTypes.UserStatus.Away, 3);
    private static readonly IcqStatusCode _DoNotDisturb = new IcqStatusCode("Do not disturb", IcqInterface.DataTypes.UserStatus.DoNotDisturb, 4);
    private static readonly IcqStatusCode _Occupied = new IcqStatusCode("Occupied", IcqInterface.DataTypes.UserStatus.Occupied, 5);
    private static readonly IcqStatusCode _NotAvailable = new IcqStatusCode("Not Available", IcqInterface.DataTypes.UserStatus.NotAvailable, 6);
    private static readonly IcqStatusCode _Offline = new IcqStatusCode("Offline", IcqInterface.DataTypes.UserStatus.Offline, 7);
    private static readonly IcqStatusCode _Unknown = new IcqStatusCode("Unknown status", IcqInterface.DataTypes.UserStatus.Offline, 8);

    public static IcqStatusCode Online {
      get { return _Online; }
    }

    public static IcqStatusCode Offline {
      get { return _Offline; }
    }

    public static IcqStatusCode Occupied {
      get { return _Occupied; }
    }

    public static IcqStatusCode NotAvailable {
      get { return _NotAvailable; }
    }

    public static IcqStatusCode Invisible {
      get { return _Invisible; }
    }

    public static IcqStatusCode Free4Chat {
      get { return _Free4Chat; }
    }

    public static IcqStatusCode Away {
      get { return _Away; }
    }

    public static IcqStatusCode DoNotDisturb {
      get { return _DoNotDisturb; }
    }

    public static IcqStatusCode GetStatusCode(IcqInterface.DataTypes.UserStatus status)
    {
      switch (status) {
        case DataTypes.UserStatus.Away:
          return _Away;
          break;
        case DataTypes.UserStatus.DoNotDisturb:
          return _DoNotDisturb;
          break;
        case DataTypes.UserStatus.FreeForChat:
          return _Free4Chat;
          break;
        case DataTypes.UserStatus.Invisible:
          return _Invisible;
          break;
        case DataTypes.UserStatus.NotAvailable:
          return _NotAvailable;
          break;
        case DataTypes.UserStatus.Occupied:
          return _Occupied;
          break;
        case DataTypes.UserStatus.Offline:
          return _Offline;
          break;
        case DataTypes.UserStatus.Online:
          return _Online;
          break;
        default:
          return _Unknown;
          break;
      }
    }
  }

  public class IcqStatusCode : Interfaces.IStatusCode
  {
    private Hashtable _Attributes = new Hashtable();
    private string _DisplayName;
    private int sortIndex;

    public IcqStatusCode(string name, DataTypes.UserStatus icqStatus, int sort)
    {
      _DisplayName = name;
      _Attributes("IcqUserStatus") = icqStatus;
      sortIndex = sort;
    }

    public System.Collections.Hashtable Interfaces.IStatusCode.Attributes {
      get { return _Attributes; }
    }

    public string Interfaces.IStatusCode.DisplayName {
      get { return _DisplayName; }
    }

    public DataTypes.UserStatus IcqUserStatus {
      get { return (DataTypes.UserStatus)_Attributes("IcqUserStatus"); }
    }

    public int System.IComparable.CompareTo(object obj)
    {
      IcqStatusCode x = obj as IcqStatusCode;

      if (x != null)
        return Comparer.Default.Compare(sortIndex, x.sortIndex);
    }

    public override string ToString()
    {
      return DisplayName;
    }
  }
}

