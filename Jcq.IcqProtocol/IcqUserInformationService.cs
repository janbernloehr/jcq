// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqUserInformationService.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Jcq.Core;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    public class IcqUserInformationService : ContextService, IUserInformationService
    {
        private static readonly Dictionary<long, ShortUserInformationRequestManager> PendingRequests =
            new Dictionary<long, ShortUserInformationRequestManager>();

        public IcqUserInformationService(IContext context)
            : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            connector.RegisterSnacHandler<Snac010F>(0x1, 0xf, AnalyseSnac010F);
            connector.RegisterSnacHandler<Snac030B>(0x3, 0xb, AnalyseSnac030B);
            connector.RegisterSnacHandler<Snac030C>(0x3, 0xc, AnalyseSnac030C);
            connector.RegisterSnacHandler<Snac1501>(0x15, 0x1, AnalyseSnac1501);
            connector.RegisterSnacHandler<Snac1503>(0x15, 0x3, AnalyseSnac1503);
        }

        public void RequestShortUserInfo(IContact contact)
        {
            RequestShortUserInfo(contact, false);
        }

        public void RequestShortUserInfo(IContact contact, bool force)
        {
            var icqContact = contact as IcqContact;

            if (icqContact == null)
                return;

            if (!force & icqContact.LastShortUserInfoRequest > DateTime.MinValue)
                return;

            var manager = new ShortUserInformationRequestManager(Context, icqContact);
            manager.Execute();

            lock (PendingRequests)
            {
                PendingRequests.Add(manager.RequestId, manager);
            }

            Kernel.Logger.Log("IcqUserInformationService", TraceEventType.Error,
                "Request Manager created for request {0}", manager.RequestId);
        }

        public void RequestShortUserInfoForAllUsers()
        {
            // a copy of the actual list is required since the contact
            // list may change during the following operation causing the
            // enumeration to fail.
            var contacts = Context.GetService<IStorageService>().Contacts.ToList();

            foreach (var x in contacts)
            {
                RequestShortUserInfo(x);

                // sleep for 1 second to avoid server spaming
                // which may result in a disconnect.
                Thread.Sleep(ComputeSleepInterval());
            }

            RequestShortUserInfoForAllUsersCompleted?.Invoke(this, EventArgs.Empty);
        }

        private DateTime _lastRateLimitExcession = DateTime.MinValue;


        private TimeSpan ComputeSleepInterval()
        {
            var diff = _lastRateLimitExcession.AddMinutes(5) - DateTime.Now;

            if (diff.TotalSeconds < 1)
                 diff = TimeSpan.FromSeconds(1);

            Debug.WriteLine("IcqUserInformationService: Sleep Interval is {0}", diff);

            return diff;
        }

        public event EventHandler RequestShortUserInfoForAllUsersCompleted;
        internal event EventHandler<ShortUserInformationTransportEventArgs> ShortUserInformationReceived;

        internal void AnalyseSnac010F(Snac010F dataIn)
        {
            foreach (var x in dataIn.UserInfos)
            {
                var c = Context.GetService<IStorageService>().GetContactByIdentifier(x.Uin);
                if (c == null)
                    continue;

                if (x.MemberSince.HasData)
                {
                    c.MemberSince = x.MemberSince.MemberSince;
                }

                if (x.OnlineTime.HasData)
                {
                }

                if (x.SignOnTime.HasData)
                {
                    c.SignOnTime = x.SignOnTime.SignOnTime;
                }

                if (x.UserCapabilities.HasData)
                {
                }

                if (x.UserClass.HasData)
                {
                }

                if (x.UserIconIdAndHash.HasData)
                {
                    c.SetIconHash(x.UserIconIdAndHash.IconMD5Hash);

                    Debug.WriteLine(string.Format("User {0} has an Icon.", c.Identifier), "IcqUserInformationService");
                }

                if (x.UserStatus.HasData)
                {
                    var oldStatus = c.Status;
                    IStatusCode newStatus = IcqStatusCodes.GetStatusCode(x.UserStatus.UserStatus);

                    Debug.WriteLine(string.Format("User {0} changed Status to {1}.", c.Identifier, newStatus),
                        "IcqUserInformationService");

                    if (!ReferenceEquals(oldStatus, newStatus))
                    {
                        c.Status = newStatus;

                        //RaiseEvent ContactStatusChanged(Me, New Interfaces.StatusChangedEventArgs(oldStatus, newStatus, c))
                    }
                }
                else
                {
                    var oldStatus = c.Status;
                    IStatusCode newStatus = IcqStatusCodes.Online;

                    if (!ReferenceEquals(oldStatus, newStatus))
                    {
                        c.Status = newStatus;

                        //RaiseEvent ContactStatusChanged(Me, New Interfaces.StatusChangedEventArgs(oldStatus, newStatus, c))
                    }
                }
            }
        }

        internal void AnalyseSnac030B(Snac030B dataIn)
        {
            foreach (var x in dataIn.UserInfos)
            {
                var c = Context.GetService<IStorageService>().GetContactByIdentifier(x.Uin);
                if (c == null)
                    continue;

                if (x.MemberSince.HasData)
                {
                    c.MemberSince = x.MemberSince.MemberSince;
                }

                if (x.OnlineTime.HasData)
                {
                }

                if (x.SignOnTime.HasData)
                {
                    c.SignOnTime = x.SignOnTime.SignOnTime;
                }

                if (x.UserCapabilities.HasData)
                {
                }

                if (x.UserClass.HasData)
                {
                }

                if (x.UserIconIdAndHash.HasData)
                {
                    c.SetIconHash(x.UserIconIdAndHash.IconMD5Hash);

                    Debug.WriteLine(string.Format("User {0} has an Icon.", c.Identifier), "IcqUserInformationService");
                }

                if (x.UserStatus.HasData)
                {
                    var oldStatus = c.Status;
                    IStatusCode newStatus = IcqStatusCodes.GetStatusCode(x.UserStatus.UserStatus);

                    Debug.WriteLine(string.Format("User {0} changed Status to {1}.", c.Identifier, newStatus),
                        "IcqUserInformationService");

                    if (!ReferenceEquals(oldStatus, newStatus))
                    {
                        c.Status = newStatus;

                        //RaiseEvent ContactStatusChanged(Me, New Interfaces.StatusChangedEventArgs(oldStatus, newStatus, c))
                    }
                }
                else
                {
                    var oldStatus = c.Status;
                    IStatusCode newStatus = IcqStatusCodes.Online;

                    if (!ReferenceEquals(oldStatus, newStatus))
                    {
                        c.Status = newStatus;

                        //RaiseEvent ContactStatusChanged(Me, New Interfaces.StatusChangedEventArgs(oldStatus, newStatus, c))
                    }
                }
            }
        }

        internal void AnalyseSnac030C(Snac030C dataIn)
        {
            foreach (var x in dataIn.UserInfos)
            {
                var c = Context.GetService<IStorageService>().GetContactByIdentifier(x.Uin);

                c.Status = IcqStatusCodes.GetStatusCode(UserStatus.Offline);
            }
        }

        private void AnalyseSnac1501(Snac1501 dataIn)
        {
            try
            {
                _lastRateLimitExcession = DateTime.Now;

                //OnServerError(string.Format("Error: {0}, Sub Code: {1}", dataIn.ErrorCode, dataIn.SubError.ErrorSubCode));

                if (dataIn.ErrorCode == ErrorCode.ServerRateLimitExceeded)
                {
                    lock (PendingRequests)
                    {
                        if (PendingRequests.ContainsKey(dataIn.RequestId))
                        {
                            PendingRequests[dataIn.RequestId].ServerRateLimitExceeded();
                        }
                        else
                        {
                            Kernel.Logger.Log("IcqUserInformationService", TraceEventType.Error,
                                "No Request Manager for request {0}", dataIn.RequestId);
                        }
                    }

                    //Context.GetService<IRateLimitsService>().EmergencyThrottle();

                }
            }
            catch (Exception ex)
            {

                Kernel.Exceptions.PublishException(ex);
            }
        }

        internal void AnalyseSnac1503(Snac1503 dataIn)
        {
            try
            {
                if (dataIn.MetaData.MetaResponse.ResponseType == MetaResponseType.MetaInformationResponse)
                {
                    if (((MetaInformationResponse) dataIn.MetaData.MetaResponse).ResponseSubType ==
                        MetaResponseSubType.ShortUserInformationReply)
                    {
                        var resp = (MetaShortUserInformationResponse) dataIn.MetaData.MetaResponse;

                        ShortUserInformationReceived?.Invoke(this,
                            new ShortUserInformationTransportEventArgs(dataIn.RequestId, resp));

                        lock (PendingRequests)
                        {
                            if (PendingRequests.ContainsKey(dataIn.RequestId))
                            {
                                PendingRequests[dataIn.RequestId].ProcessResponse(resp);
                                PendingRequests.Remove(dataIn.RequestId);
                            }
                            else
                            {
                                Kernel.Logger.Log("IcqUserInformationService", TraceEventType.Error,
                                    "No Request Manager for request {0}", dataIn.RequestId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }
    }
}