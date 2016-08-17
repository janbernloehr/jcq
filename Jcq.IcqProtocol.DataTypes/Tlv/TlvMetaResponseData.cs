// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TlvMetaResponseData.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Jcq.IcqProtocol.DataTypes
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

            int index = SizeFixPart;

            while (index < data.Count)
            {
                MetaResponseDescriptor desc = MetaResponseDescriptor.GetDescriptor(index, data);

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