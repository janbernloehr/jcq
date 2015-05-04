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
namespace JCsTools.JCQ.IcqInterface.Interfaces
{
  public interface IContact : IStorageItem
  {
    IGroup Group { get; }
    DateTime MemberSince { get; set; }
      get; set;
    DateTime SignOnTime { get; set; }
      get; set;
    string FirstName { get; set; }
      get; set;
    string LastName { get; set; }
      get; set;
    string EmailAddress { get; set; }
      get; set;
    ContactGender Gender { get; set; }
      get; set;
    bool AuthorizationRequired { get; set; }
      get; set;
    List<byte> IconHash { get; }
    List<byte> IconData { get; }
    void SetIconHash(List<byte> value);
    void SetIconData(List<byte> value);
    IStatusCode Status { get; set; }
      get; set;
    event EventHandler IconHashReceived;
    event EventHandler IconDataReceived;
  }

  public enum ContactGender : byte
  {
    Unknown = 0,
    Male = 1,
    Female = 2
  }
}

