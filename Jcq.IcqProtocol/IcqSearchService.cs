// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqSearchService.cs" company="Jan-Cornelius Molnar">
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
using Jcq.Core;
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol
{
    public class IcqSearchService : ContextService, ISearchService
    {
        // When a user search is initiated the icq server sends the search results one by one as Snac1503.
        // The results need to be saved until the last one is received.
        private readonly List<IContact> _tempFoundUsers = new List<IContact>();

        public IcqSearchService(IContext context) : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            connector.RegisterSnacHandler(0x15, 0x3, new Action<Snac1503>(AnalyseSnac1503));
        }

        void ISearchService.SearchByUin(string uin)
        {
            var dataOut = new Snac1502();

            var req = new MetaSearchByTlvRequest(MetaRequestSubType.SearchByUinRequestTlv)
            {
                RequestSequenceNumber = MetaRequest.GetNextSequenceNumber(),
                ClientUin = long.Parse(Context.Identity.Identifier)
            };

            var tlv = new SearchByUinTlv {Uin = int.Parse(uin)};

            req.SearchTlvs.Add(tlv);

            dataOut.MetaData.MetaRequest = req;

            var transfer = (IIcqDataTranferService) Context.GetService<IConnector>();
            transfer.Send(dataOut);
        }

        public event EventHandler<SearchResultEventArgs> SearchResult;

        private void AnalyseSnac1503(Snac1503 dataIn)
        {
            try
            {
                switch (dataIn.MetaData.MetaResponse.ResponseType)
                {
                    case MetaResponseType.MetaInformationResponse:
                        var info = (MetaInformationResponse) dataIn.MetaData.MetaResponse;

                        if (info.ResponseSubType == MetaResponseSubType.SearchUserFoundReply |
                            info.ResponseSubType == MetaResponseSubType.SearchLastUserFoundReply)
                        {
                            var resp = (MetaSearchByUinResponse) dataIn.MetaData.MetaResponse;

                            IContact contact = Context.GetService<IStorageService>()
                                .GetContactByIdentifier(resp.FoundUserUin.ToString());

                            contact.Name = resp.Nickname;
                            contact.FirstName = resp.FirstName;
                            contact.LastName = resp.LastName;
                            contact.EmailAddress = resp.EmailAddress;
                            contact.Gender = (ContactGender) resp.Gender;
                            contact.AuthorizationRequired = resp.AutorizationRequired;

                            _tempFoundUsers.Add(contact);

                            if (info.ResponseSubType == MetaResponseSubType.SearchLastUserFoundReply)
                            {
                                var temp = new List<IContact>(_tempFoundUsers);
                                _tempFoundUsers.Clear();

                                if (SearchResult != null)
                                {
                                    SearchResult(this, new SearchResultEventArgs(temp));
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}