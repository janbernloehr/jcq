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
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public class MetaShortUserInformationResponse : MetaInformationResponse
  {
    public MetaShortUserInformationResponse() : base(MetaResponseSubType.ShortUserInformationReply)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    private string _Nickname;
    public string Nickname {
      get { return _Nickname; }
      set { _Nickname = value; }
    }

    private string _FirstName;
    public string FirstName {
      get { return _FirstName; }
      set { _FirstName = value; }
    }

    private string _LastName;
    public string LastName {
      get { return _LastName; }
      set { _LastName = value; }
    }

    private string _EmailAddress;
    public string EmailAddress {
      get { return _EmailAddress; }
      set { _EmailAddress = value; }
    }

    private bool _AutorizationRequired;
    public bool AuthorizationRequired {
      get { return _AutorizationRequired; }
      set { _AutorizationRequired = value; }
    }

    private byte _Gender;
    public byte Gender {
      get { return _Gender; }
      set { _Gender = value; }
    }

    private bool _SearchSucceeded;
    public bool SearchSucceeded {
      get { return _SearchSucceeded; }
      set { _SearchSucceeded = value; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = MetaResponse.SizeFixPart + 2;

      if (data(index) == 0xa) {
        _SearchSucceeded = true;
      } else {
        _SearchSucceeded = false;
        return;
      }

      index += 1;

      _Nickname = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
      index += 2 + string.IsNullOrEmpty(_Nickname) ? 0 : _Nickname.Length + 1;

      _FirstName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
      index += 2 + string.IsNullOrEmpty(_FirstName) ? 0 : _FirstName.Length + 1;

      _LastName = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
      index += 2 + string.IsNullOrEmpty(_LastName) ? 0 : _LastName.Length + 1;

      _EmailAddress = ByteConverter.ToZeroBasedStringFromUInt16LEIndex(index, data);
      index += 2 + string.IsNullOrEmpty(_EmailAddress) ? 0 : _EmailAddress.Length + 1;

      if (data(index) == 0)
        _AutorizationRequired = true;
      index += 1;
      index += 1;

      _Gender = data(index);
    }
  }
}

