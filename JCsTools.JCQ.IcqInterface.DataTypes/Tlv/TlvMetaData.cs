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
  public class TlvMetaRequestData : Tlv
  {
    public TlvMetaRequestData() : base(0x1)
    {
    }

    public override int CalculateDataSize()
    {
      if (_MetaRequest != null)
        return _MetaRequest.CalculateTotalSize;
    }

    private MetaRequest _MetaRequest;
    public MetaRequest MetaRequest {
      get { return _MetaRequest; }
      set { _MetaRequest = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      List<byte> data = base.Serialize();
      data.AddRange(_MetaRequest.Serialize);
      return data;
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      throw new NotImplementedException();
    }
  }

  public class TlvMetaResponseData : Tlv
  {
    public TlvMetaResponseData() : base(0x1)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    private MetaResponse _MetaResponse;
    public MetaResponse MetaResponse {
      get { return _MetaResponse; }
      set { _MetaResponse = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Tlv.SizeFixPart;

      while (index < data.Count) {
        MetaResponseDescriptor desc = MetaResponseDescriptor.GetDescriptor(index, data);

        switch (desc.ResponseType) {
          case MetaResponseType.EndOfOfflineMessageResponse:
            EndOfOfflineMessagesResponse resp;
            resp = new EndOfOfflineMessagesResponse();
            resp.Deserialize(data.GetRange(index, desc.TotalSize));
            MetaResponse = resp;
            break;
          case MetaResponseType.OfflineMessageResponse:
            OfflineMessageResponse resp;
            resp = new OfflineMessageResponse();
            resp.Deserialize(data.GetRange(index, desc.TotalSize));
            MetaResponse = resp;
            break;
          case MetaResponseType.MetaInformationResponse:
            MetaResponse = DeserializeMetaResponse(desc, data.GetRange(index, desc.TotalSize));
            break;
        }

        index += desc.TotalSize;
      }
    }

    private MetaResponse DeserializeMetaResponse(MetaResponseDescriptor desc, List<byte> data)
    {
      switch (desc.ResponseSubType) {
        case MetaResponseSubType.ShortUserInformationReply:
          MetaShortUserInformationResponse resp = new MetaShortUserInformationResponse();
          resp.Deserialize(data);
          return resp;
          break;
        case MetaResponseSubType.SearchUserFoundReply:
          MetaSearchByUinResponse resp = new MetaSearchByUinResponse();
          resp.Deserialize(data);
          return resp;
          break;
        case MetaResponseSubType.SearchLastUserFoundReply:
          MetaSearchByUinResponse resp = new MetaSearchByUinResponse();
          resp.Deserialize(data);
          return resp;
          break;
        default:
          return null;
          break;
      }
    }
  }

  public class MetaResponseDescriptor
  {
    private int _TotalSize;
    public int TotalSize {
      get { return _TotalSize; }
    }

    private MetaResponseType _ResponseType;
    public MetaResponseType ResponseType {
      get { return _ResponseType; }
      set { _ResponseType = value; }
    }

    private MetaResponseSubType _ResponseSubType;
    public MetaResponseSubType ResponseSubType {
      get { return _ResponseSubType; }
      set { _ResponseSubType = value; }
    }

    public static MetaResponseDescriptor GetDescriptor(int offset, System.Collections.Generic.List<byte> bytes)
    {
      List<byte> data = bytes.GetRange(offset, bytes.Count - offset);
      MetaResponseDescriptor desc = new MetaResponseDescriptor();
      desc.Deserialize(data);
      return desc;
    }

    private void Deserialize(List<byte> data)
    {
      int index;

      _TotalSize = 2 + ByteConverter.ToUInt16LE(data.GetRange(index, 2));
      index += 2;

      index += 4;

      _ResponseType = (MetaResponseType)(int)ByteConverter.ToUInt16LE(data.GetRange(index, 2));
      index += 2;

      if (_ResponseType == MetaResponseType.MetaInformationResponse) {
        index += 2;

        _ResponseSubType = (MetaResponseSubType)(int)ByteConverter.ToUInt16LE(data.GetRange(index, 2));
      }
    }
  }
}

