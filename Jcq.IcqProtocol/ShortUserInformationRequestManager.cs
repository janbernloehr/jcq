// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShortUserInformationRequestManager.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Threading;
using Jcq.Core;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
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