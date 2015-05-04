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
  public enum MetaResponseType : int
  {
    OfflineMessageResponse = 0x41,
    EndOfOfflineMessageResponse = 0x42,
    MetaInformationResponse = 0x7da
  }

  public enum MetaResponseSubType : int
  {
    MetaProcessingErrorServerReply = 0x1,
    SetUserHomeInfoServerAck = 0x64,
    SetUserWorkInfoServerAck = 0x6e,
    SetUserMoreInfoServerAck = 0x78,
    SetUserNotesInfoServerAck = 0x82,
    SetUserEmailSInfoServerAck = 0x87,
    SetUserInterestsInfoServerAck = 0x8c,
    SetUserAffilationsInfoServerAck = 0x96,
    ServerSMSResponseDeliveryReceipt = 0x96,
    SetUserPermissionsServerAck = 0xa0,
    SetUserPasswordServerAck = 0xaa,
    UnregisterAccountServerAck = 0xb4,
    SetUserHomepageCategoryServerAck = 0xbe,
    UserBasicInfoReply = 0xc8,
    UserWorkInfoReply = 0xd2,
    UserMoreInfoReply = 0xdc,
    UserNotesAboutInfoReply = 0xe6,
    UserExtendedEmailInfoReply = 0xeb,
    UserInterestsInfoReply = 0xf0,
    UserPastAffilationsInfoReply = 0xfa,
    ShortUserInformationReply = 0x104,
    UserHomepageCategoryInformationReply = 0x10e,
    SearchUserFoundReply = 0x1a4,
    SearchLastUserFoundReply = 0x1ae,
    RegistrationStatsAck = 0x302,
    RandomSearchServerReply = 0x366,
    ServerVariableRequestedViaXml = 0x8a2,
    ServerAckForSetFullinfoCommand = 0xc3f,
    ServerAckForUserSpamReport = 0x2012
  }

  public abstract class MetaResponse : ISerializable
  {
    public const int SizeFixPart = 10;

    public MetaResponse(MetaResponseType responseType)
    {
      _ResponseType = responseType;
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

    private MetaResponseType _ResponseType;
    public MetaResponseType ResponseType {
      get { return _ResponseType; }
      set { _ResponseType = value; }
    }

    private int _ResponseSequenceNumber;
    public int ResponseSequenceNumber {
      get { return _ResponseSequenceNumber; }
      set { _ResponseSequenceNumber = value; }
    }

    public virtual void ISerializable.Deserialize(System.Collections.Generic.List<byte> data)
    {
      int index;

      _DataSize = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2);
      index += 2;

      _ClientUin = ByteConverter.ToUInt32LE(data.GetRange(index, 4));
      index += 4;

      _ResponseType = (MetaResponseType)(int)ByteConverter.ToUInt16LE(data.GetRange(index, 2));
      index += 2;

      _ResponseSequenceNumber = ByteConverter.ToUInt16LE(data.GetRange(index, 2)) - (SizeFixPart - 2);
      index += 2;

      _HasData = true;
    }

    public virtual System.Collections.Generic.List<byte> ISerializable.Serialize()
    {
      throw new NotImplementedException();
      //Dim data As List(Of Byte) = New List(Of Byte)

      //data.AddRange(ByteConverter.GetBytesLE(CUShort(Me.CalculateDataSize + (SizeFixPart - 2))))
      //data.AddRange(ByteConverter.GetBytesLE(CUInt(_ClientUin)))
      //data.AddRange(ByteConverter.GetBytesLE(CUShort(_ResponseType)))
      //data.AddRange(ByteConverter.GetBytesLE(CUShort(_ResponseSequenceNumber)))

      //Return data
    }
  }
}

