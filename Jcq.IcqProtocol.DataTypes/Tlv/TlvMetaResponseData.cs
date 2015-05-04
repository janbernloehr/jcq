// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvMetaResponseData.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class TlvMetaResponseData : Tlv
    {
        public TlvMetaResponseData() : base(0x1)
        {
        }

        public MetaResponse MetaResponse { get; set; }

        public override int CalculateDataSize()
        {
            throw new NotImplementedException();
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var desc = MetaResponseDescriptor.GetDescriptor(index, data);

                switch (desc.ResponseType)
                {
                    case MetaResponseType.EndOfOfflineMessageResponse:
                        EndOfOfflineMessagesResponse respa;
                        respa = new EndOfOfflineMessagesResponse();
                        respa.Deserialize(data.GetRange(index, desc.TotalSize));
                        MetaResponse = respa;
                        break;
                    case MetaResponseType.OfflineMessageResponse:
                        OfflineMessageResponse respb;
                        respb = new OfflineMessageResponse();
                        respb.Deserialize(data.GetRange(index, desc.TotalSize));
                        MetaResponse = respb;
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
            switch (desc.ResponseSubType)
            {
                case MetaResponseSubType.ShortUserInformationReply:
                    var resp = new MetaShortUserInformationResponse();
                    resp.Deserialize(data);
                    return resp;
                case MetaResponseSubType.SearchUserFoundReply:
                    var respb = new MetaSearchByUinResponse();
                    respb.Deserialize(data);
                    return respb;
                case MetaResponseSubType.SearchLastUserFoundReply:
                    var respc = new MetaSearchByUinResponse();
                    respc.Deserialize(data);
                    return respc;
                default:
                    return null;
            }
        }
    }
}