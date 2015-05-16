// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqSearchService.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
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

                            var contact = Context.GetService<IStorageService>()
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