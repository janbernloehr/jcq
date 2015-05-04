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
  public class FlapRequestSignInCookie : Flap
  {
    public FlapRequestSignInCookie() : base(FlapChannel.NewConnectionNegotiation)
    {
    }

    private TlvScreenName _ScreenName = new TlvScreenName();
    public TlvScreenName ScreenName {
      get { return _ScreenName; }
    }

    private TlvPassword _Password = new TlvPassword();
    public TlvPassword Password {
      get { return _Password; }
    }

    private TlvClientIdString _ClientIdString = new TlvClientIdString();
    public TlvClientIdString ClientIdString {
      get { return _ClientIdString; }
    }

    private TlvClientId _ClientId = new TlvClientId();
    public TlvClientId ClientId {
      get { return _ClientId; }
    }

    private TlvClientMajorVersion _ClientMajorVersion = new TlvClientMajorVersion();
    public TlvClientMajorVersion ClientMajorVersion {
      get { return _ClientMajorVersion; }
    }

    private TlvClientMinorVersion _ClientMinorVersion = new TlvClientMinorVersion();
    public TlvClientMinorVersion ClientMinorVersion {
      get { return _ClientMinorVersion; }
    }

    private TlvClientLesserVersion _ClientLesserVersion = new TlvClientLesserVersion();
    public TlvClientLesserVersion ClientLesserVersion {
      get { return _ClientLesserVersion; }
    }

    private TlvClientBuildNumber _ClientBuildNumber = new TlvClientBuildNumber();
    public TlvClientBuildNumber ClientBuildNumber {
      get { return _ClientBuildNumber; }
    }

    private TlvClientDistributionNumber _ClientDistributionNumber = new TlvClientDistributionNumber();
    public TlvClientDistributionNumber ClientDistributionNumber {
      get { return _ClientDistributionNumber; }
    }

    private TlvClientLanguage _ClientLanguage = new TlvClientLanguage();
    public TlvClientLanguage ClientLanguage {
      get { return _ClientLanguage; }
    }

    private TlvClientCountry _ClientCountry = new TlvClientCountry();
    public TlvClientCountry ClientCountry {
      get { return _ClientCountry; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      return 4 + _ScreenName.CalculateTotalSize + _Password.CalculateTotalSize + _ClientIdString.CalculateTotalSize + _ClientId.CalculateTotalSize + _ClientMajorVersion.CalculateTotalSize + _ClientMinorVersion.CalculateTotalSize + _ClientLesserVersion.CalculateTotalSize + _ClientBuildNumber.CalculateTotalSize + _ClientDistributionNumber.CalculateTotalSize + _ClientLanguage.CalculateTotalSize + _ClientCountry.CalculateTotalSize;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)1));
      data.AddRange(_ScreenName.Serialize);
      data.AddRange(_Password.Serialize);
      data.AddRange(_ClientIdString.Serialize);
      data.AddRange(_ClientId.Serialize);
      data.AddRange(_ClientMajorVersion.Serialize);
      data.AddRange(_ClientMinorVersion.Serialize);
      data.AddRange(_ClientLesserVersion.Serialize);
      data.AddRange(_ClientBuildNumber.Serialize);
      data.AddRange(_ClientDistributionNumber.Serialize);
      data.AddRange(_ClientLanguage.Serialize);
      data.AddRange(_ClientCountry.Serialize);

      return data;
    }
  }

  public class TlvScreenName : Tlv
  {
    public TlvScreenName() : base(0x1)
    {
    }

    private string _Uin;
    public string Uin {
      get { return _Uin; }
      set { _Uin = value; }
    }

    public override int CalculateDataSize()
    {
      return _Uin.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_Uin));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _Uin = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvClientIdString : Tlv
  {
    public TlvClientIdString() : base(0x3)
    {
    }

    private string _ClientIdString;
    public string ClientIdString {
      get { return _ClientIdString; }
      set { _ClientIdString = value; }
    }

    public override int CalculateDataSize()
    {
      return _ClientIdString.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_ClientIdString));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientIdString = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvClientLanguage : Tlv
  {
    public TlvClientLanguage() : base(0xf)
    {
    }

    private string _ClientLanguage;
    public string ClientLanguage {
      get { return _ClientLanguage; }
      set { _ClientLanguage = value; }
    }

    public override int CalculateDataSize()
    {
      return _ClientLanguage.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_ClientLanguage));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientLanguage = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvClientCountry : Tlv
  {
    public TlvClientCountry() : base(0xe)
    {
    }

    private string _ClientCountry;
    public string ClientCountry {
      get { return _ClientCountry; }
      set { _ClientCountry = value; }
    }

    public override int CalculateDataSize()
    {
      return _ClientCountry.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes(_ClientCountry));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientCountry = ByteConverter.ToString(data.GetRange(index, DataSize));
    }
  }

  public class TlvClientId : Tlv
  {
    public TlvClientId() : base(0x16)
    {
    }

    private int _ClientId;
    public int ClientId {
      get { return _ClientId; }
      set { _ClientId = value; }
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientId));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientId = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }
  }

  public class TlvClientMajorVersion : Tlv
  {
    public TlvClientMajorVersion() : base(0x17)
    {
    }

    private int _ClientMajorVersion;
    public int ClientMajorVersion {
      get { return _ClientMajorVersion; }
      set { _ClientMajorVersion = value; }
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientMajorVersion));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientMajorVersion = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }
  }

  public class TlvClientMinorVersion : Tlv
  {
    public TlvClientMinorVersion() : base(0x18)
    {
    }

    private int _ClientMinorVersion;
    public int ClientMinorVersion {
      get { return _ClientMinorVersion; }
      set { _ClientMinorVersion = value; }
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientMinorVersion));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientMinorVersion = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }
  }

  public class TlvClientLesserVersion : Tlv
  {
    public TlvClientLesserVersion() : base(0x19)
    {
    }

    private int _ClientLesserVersion;
    public int ClientLesserVersion {
      get { return _ClientLesserVersion; }
      set { _ClientLesserVersion = value; }
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientLesserVersion));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientLesserVersion = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }
  }

  public class TlvClientBuildNumber : Tlv
  {
    public TlvClientBuildNumber() : base(0x1a)
    {
    }

    private int _ClientBuildNumber;
    public int ClientBuildNumber {
      get { return _ClientBuildNumber; }
      set { _ClientBuildNumber = value; }
    }

    public override int CalculateDataSize()
    {
      return 2;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((ushort)_ClientBuildNumber));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientBuildNumber = ByteConverter.ToUInt16(data.GetRange(index, 2));
    }
  }

  public class TlvClientDistributionNumber : Tlv
  {
    public TlvClientDistributionNumber() : base(0x14)
    {
    }

    private long _ClientDistributionNumber;
    public long ClientDistributionNumber {
      get { return _ClientDistributionNumber; }
      set { _ClientDistributionNumber = value; }
    }

    public override int CalculateDataSize()
    {
      return 4;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetBytes((uint)_ClientDistributionNumber));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      _ClientDistributionNumber = ByteConverter.ToUInt32(data.GetRange(index, 4));
    }
  }

  public class TlvPassword : Tlv
  {
    public TlvPassword() : base(0x2)
    {
    }

    private string _Password;
    public string Password {
      get { return _Password; }
      set { _Password = value; }
    }

    public override int CalculateDataSize()
    {
      return _Password.Length;
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();

      data.AddRange(ByteConverter.GetXorHashFromPassword(_Password.ToCharArray));

      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }
  }
}

