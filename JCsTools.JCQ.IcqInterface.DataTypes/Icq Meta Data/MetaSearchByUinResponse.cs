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
  public class MetaSearchByUinResponse : MetaInformationResponse
  {
    public MetaSearchByUinResponse() : base(MetaResponseSubType.SearchUserFoundReply)
    {
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }


    private int _FoundUserUin;
    public int FoundUserUin {
      get { return _FoundUserUin; }
      set { _FoundUserUin = value; }
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
    public bool AutorizationRequired {
      get { return _AutorizationRequired; }
      set { _AutorizationRequired = value; }
    }


    private SearchUserStatus _Status;
    public SearchUserStatus Status {
      get { return _Status; }
      set { _Status = value; }
    }

    private byte _Gender;
    public byte Gender {
      get { return _Gender; }
      set { _Gender = value; }
    }

    private int _Age;
    public int Age {
      get { return _Age; }
      set { _Age = value; }
    }


    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = MetaResponse.SizeFixPart + 2;
      int dataSize;

      Core.Kernel.Logger.Log("MetaSearchByUinResponse", TraceEventType.Information, "found at index {0}; total size: {1}", index, data.Count);

      if (data(index) == 0xa) {
        // Search succeeded
        index += 1;

        dataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
        index += 2;

        Core.Kernel.Logger.Log("MetaSearchByUinResponse", TraceEventType.Information, "succeeded; data size: {0}", dataSize);

        _FoundUserUin = (int)ByteConverter.ToUInt32LE(data.GetRange(index, 4));
        index += 4;

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

        _Status = (SearchUserStatus)Convert.ToInt32(ByteConverter.ToUInt16LE(data.GetRange(index, 2)));
        index += 2;

        _Gender = data(index);
        index += 1;

        _Age = ByteConverter.ToUInt16LE(data.GetRange(index, 2));
        index += 2;
      } else {
        Core.Kernel.Logger.Log("MetaSearchByUinResponse", TraceEventType.Information, "invalid data.");
      }
    }
  }

  public enum SearchUserStatus
  {
    Online = 0,
    Offline = 1,
    NonWebaware = 2
  }
}

