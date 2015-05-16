// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShortUserInformationRequestManager.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Threading;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public class ShortUserInformationRequestManager
    {
        private readonly IcqContact _contact;
        private readonly IContext _context;
        private readonly int _retryDueMillisecond = Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds);
        private bool _requestSucceeded;
        private int _retryIteration;
        private Timer _retryTimer;

        public ShortUserInformationRequestManager(IContext context, IcqContact contact)
        {
            _context = context;
            _contact = contact;
        }

        public IcqContact Contact
        {
            get { return _contact; }
        }

        public IContext Context
        {
            get { return _context; }
        }

        public long RequestId { get; private set; }

        public void ProcessResponse(MetaShortUserInformationResponse response)
        {
            _requestSucceeded = true;

            if (response.SearchSucceeded)
            {
                Contact.Name = response.Nickname;
                Contact.FirstName = response.FirstName;
                Contact.LastName = response.LastName;
                Contact.EmailAddress = response.EmailAddress;
                Contact.Gender = (ContactGender) response.Gender;
                Contact.AuthorizationRequired = response.AuthorizationRequired;
                Contact.LastShortUserInfoRequest = DateTime.Now;

                Kernel.Logger.Log("IcqUserInformationService", TraceEventType.Information,
                    "RequestId: {0}; '{1}' ({2}) FirstName: {3}, LastName: {4}, Auth Req. {5}", RequestId, Contact.Name,
                    Contact.Identifier, Contact.FirstName, Contact.LastName, Contact.AuthorizationRequired);
            }
            else
            {
                Kernel.Logger.Log("IcqUserInformationService", TraceEventType.Error,
                    "RequestId: {0}; Lookup for contact '{1}' ({2}) failed.", RequestId, Contact.Name,
                    Contact.Identifier);
            }
        }

        private void OnTimerCallback(object state)
        {
            try
            {
                if (!_requestSucceeded & _retryIteration < 5)
                {
                    _retryIteration += 1;
                    SendRequest();
                }
                else
                {
                    _retryTimer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void SendRequest()
        {
            var dataOut = new Snac1502();

            var req = new MetaShortUserInformationRequest
            {
                RequestSequenceNumber = MetaRequest.GetNextSequenceNumber(),
                ClientUin = long.Parse(_context.Identity.Identifier),
                SearchUin = int.Parse(Contact.Identifier)
            };

            dataOut.MetaData.MetaRequest = req;

            var transfer = (IIcqDataTranferService) _context.GetService<IConnector>();
            transfer.Send(dataOut);

            RequestId = dataOut.RequestId;
        }

        public void Execute()
        {
            SendRequest();

            _retryTimer = new Timer(OnTimerCallback, null, 0, _retryDueMillisecond);
        }
    }
}