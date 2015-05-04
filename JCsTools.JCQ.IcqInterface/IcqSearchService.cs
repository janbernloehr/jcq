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
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqSearchService : ContextService, Interfaces.ISearchService
  {
    // When a user search is initiated the icq server sends the search results one by one as Snac1503.
    // The results need to be saved until the last one is received.
    private readonly List<Interfaces.IContact> _TempFoundUsers = new List<Interfaces.IContact>();

    public IcqSearchService(Interfaces.IContext context) : base(context)
    {

      IcqConnector connector = context.GetService<Interfaces.IConnector>() as IcqConnector;

      if (connector == null)
        throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

      connector.RegisterSnacHandler<DataTypes.Snac1503>(0x15, 0x3, new Action<DataTypes.Snac1503>(AnalyseSnac1503));
    }

    public void Interfaces.ISearchService.SearchByUin(string uin)
    {
      DataTypes.Snac1502 dataOut;
      dataOut = new DataTypes.Snac1502();

      DataTypes.MetaSearchByTlvRequest req;
      req = new DataTypes.MetaSearchByTlvRequest(DataTypes.MetaRequestSubType.SearchByUinRequestTlv);
      req.RequestSequenceNumber = DataTypes.MetaRequest.GetNextSequenceNumber;
      req.ClientUin = (long)Context.Identity.Identifier;

      DataTypes.SearchByUinTlv tlv;
      tlv = new DataTypes.SearchByUinTlv();
      tlv.Uin = (int)uin;

      req.SearchTlvs.Add(tlv);

      dataOut.MetaData.MetaRequest = req;

      IIcqDataTranferService transfer = (IIcqDataTranferService)Context.GetService<Interfaces.IConnector>();
      transfer.Send(dataOut);
    }

    private void AnalyseSnac1503(DataTypes.Snac1503 dataIn)
    {
      DataTypes.MetaInformationResponse info;
      DataTypes.MetaSearchByUinResponse resp;

      try {
        switch (dataIn.MetaData.MetaResponse.ResponseType) {
          case DataTypes.MetaResponseType.MetaInformationResponse:
            info = (DataTypes.MetaInformationResponse)dataIn.MetaData.MetaResponse;

            if (info.ResponseSubType == DataTypes.MetaResponseSubType.SearchUserFoundReply | info.ResponseSubType == DataTypes.MetaResponseSubType.SearchLastUserFoundReply) {
              resp = (DataTypes.MetaSearchByUinResponse)dataIn.MetaData.MetaResponse;

              Interfaces.IContact contact;

              contact = Context.GetService<Interfaces.IStorageService>.GetContactByIdentifier((string)resp.FoundUserUin);

              contact.Name = resp.Nickname;
              contact.FirstName = resp.FirstName;
              contact.LastName = resp.LastName;
              contact.EmailAddress = resp.EmailAddress;
              contact.Gender = (Interfaces.ContactGender)resp.Gender;
              contact.AuthorizationRequired = resp.AutorizationRequired;

              _TempFoundUsers.Add(contact);

              if (info.ResponseSubType == DataTypes.MetaResponseSubType.SearchLastUserFoundReply) {
                List<Interfaces.IContact> temp = new List<Interfaces.IContact>(_TempFoundUsers);
                _TempFoundUsers.Clear();

                if (SearchResult != null) {
                  SearchResult(this, new Interfaces.SearchResultEventArgs(temp));
                }
              }
            }
            break;
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public event SearchResultEventHandler SearchResult;
    public delegate void SearchResultEventHandler(object sender, Interfaces.SearchResultEventArgs e);
  }
}

