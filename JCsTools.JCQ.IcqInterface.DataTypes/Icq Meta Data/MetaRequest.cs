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
  public enum MetaRequestType : int
  {
    OfflineMessageRequest = 0x3c,
    MetaInformationRequest = 0x7d0,
    DeleteOfflineMessage = 0x3e
  }

  public enum MetaRequestSubType : int
  {
    SetUserBasicInfoRequest = 0x3ea,
    SetUserWorkInfoRequest = 0x3f3,
    SetUserMoreInfoRequest = 0x3fd,
    SetUserNotesInfoRequest = 0x406,
    SetUserExtendedEmailInfoRequest = 0x40b,
    SetUserInterestsInfoRequest = 0x410,
    SetUserAffilationsInfoRequest = 0x41a,
    SetUserPermissionsInfoRequest = 0x424,
    ChangeUserPasswordRequest = 0x42e,
    SetUserHomepageCategoryInfoRequest = 0x442,
    RequestFullUserInfo = 0x4b2,
    RequestShortUserInfo = 0x4ba,
    UnregisterUserRequest = 0x4c4,
    RequestFullUserInfo2 = 0x4d0,
    SearchByDetailsRequestPlain = 0x515,
    SearchByUinRequestPlain = 0x51f,
    SearchByEmailRequestPlain = 0x529,
    WhitePagesSearchRequestPlainSimple = 0x533,
    SearchByDetailsRequestPlainWildcard = 0x53d,
    SearchByEmailRequestPlainWildcard = 0x547,
    WhitePagesSearchRequestPlainWildcard = 0x551,
    SearchByUinRequestTlv = 0x569,
    WhitepagesSearchRequestTlv = 0x55f,
    SearchByEmailRequestTlv = 0x573,
    RandomChatUserSearchRequest = 0x74e,
    RequestServerVariableViaXml = 0x898,
    SendRegistrationStatsReport = 0xaa5,
    SendShortcutBarStatsReport = 0xaaf,
    SaveInfoTlvBasedRequest = 0xc3a,
    ClientSendSMSRequest = 0x1482,
    ClientSpamReportRequest = 0x2008
  }

  public abstract class MetaRequest : ISerializable
  {
    public const int SizeFixPart = 10;

    public MetaRequest(MetaRequestType requestType)
    {
      _RequestType = requestType;
    }

    private static int reqestSequenceNumber = 0;

    public static int GetNextSequenceNumber()
    {
      reqestSequenceNumber += 1;
      return reqestSequenceNumber;
    }

    public abstract int ISerializable.CalculateDataSize();

    public int ISerializable.CalculateTotalSize()
    {
      return SizeFixPart + CalculateDataSize();
    }

    private int _DataSize;
    public int ISerializable.DataSize {
      get { return _DataSize; }
    }

    protected void SetDataSize(int index)
    {
      _DataSize = index - SizeFixPart;
    }

    public int ISerializable.TotalSize {
      get { return DataSize; }
    }

    private bool _HasData;
    public bool ISerializable.HasData {
      get { return _HasData; }
    }

    private long _ClientUin;
    public long ClientUin {
      get { return _ClientUin; }
      set { _ClientUin = value; }
    }

    private MetaRequestType _RequestType;
    public MetaRequestType RequestType {
      get { return _RequestType; }
      set { _RequestType = value; }
    }

    private int _RequestSequenceNumber;
    public int RequestSequenceNumber {
      get { return _RequestSequenceNumber; }
      set { _RequestSequenceNumber = value; }
    }

    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      //Dim index As Integer

      //_DataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2)
      //index += 2

      //_ClientUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4))
      //index += 4

      //_RequestType = DirectCast(CInt(ByteConverter.ToUInt16LE(data.GetRange(index, 2))), MetaRequestType)
      //index += 2

      //_RequestSequenceNumber = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2)
      //index += 2

      //_HasData = True
      throw new NotImplementedException();
    }

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      List<byte> data = new List<byte>();

      data.AddRange(ByteConverter.GetBytesLE((ushort)this.CalculateDataSize + (SizeFixPart - 2)));
      data.AddRange(ByteConverter.GetBytesLE((uint)_ClientUin));
      data.AddRange(ByteConverter.GetBytesLE((ushort)_RequestType));
      data.AddRange(ByteConverter.GetBytesLE((ushort)_RequestSequenceNumber));

      return data;
    }
  }
}

