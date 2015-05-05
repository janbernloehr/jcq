// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqConnector.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.Net;
using System.Threading;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.DataTypes;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.IcqInterface.Internal;



namespace JCsTools.JCQ.IcqInterface
{
    /// <summary>
    /// Provides the state for an authentication cookie request.
    /// </summary>
    /// <remarks></remarks>
    public class IcqConnector : BaseConnector, IConnector
    {
        private bool _isSigningIn;

        public IcqConnector(IContext context) : base(context)
        {
            RegisterSnacHandler<Snac0101>(0x1, 0x1, AnalyseSnac0101);
            RegisterSnacHandler<Snac0103>(0x1, 0x3, AnalyseSnac0103);
            RegisterSnacHandler<Snac0107>(0x1, 0x7, AnalyseSnac0107);
            RegisterSnacHandler<Snac0118>(0x1, 0x18, AnalyseSnac0118);

            RegisterSnacHandler<Snac1306>(0x13, 0x6, AnalyseSnac1306);

            InternalDisconnected += BaseInternalDisconnected;
        }

        public event EventHandler SignInCompleted;
        public event EventHandler<SignInFailedEventArgs> SignInFailed;
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        public void SignIn(ICredential credential)
        {
            var password = credential as IPasswordCredential;
            if (password == null)
                throw new ArgumentException("Credential musst be of Type IPasswordCredential", "credential");

            try
            {
                _isSigningIn = true;

                // Connect to the icq server and get a bos server address and and authentication cookie.
                InnerConnect();

                var requestAuthCookieTask = new RequestAuthenticationCookieTask(this, password);

                requestAuthCookieTask.Run();

                // When the task is run, we can exspect a disconnect ...
                TcpContext.SetCloseExpected();

                requestAuthCookieTask.WaitCompleted();

                if (!requestAuthCookieTask.State.AuthenticationSucceeded)
                {
                    // The authentication attempt was not successfull. There are many reasons for this
                    // to occur for example wrong password, to many connections etc.
                    // The client needs to be informed that the sign in failed.

                    OnSignInFailed(requestAuthCookieTask.State.AuthenticationError.ToString());
                    _isSigningIn = false;
                    return;
                }

                // if the authentication attempt was successfull we can connect to the bos server
                // and send the just received authentication cookie to begin the sign in procedure.

                var serverEndpoint = ConvertServerAddressToEndPoint(requestAuthCookieTask.State.BosServerAddress);

                InnerConnect(serverEndpoint);

                SendAuthenticationCookie(requestAuthCookieTask.State.AuthCookie);
            }
            catch
            {
                _isSigningIn = false;

                throw;
            }
        }

        public void SignOut()
        {
            // TODO: Implement proper Sign out.
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Sends the cookie received from the authentication server to the bos server.
        ///     The server replies with SnacXXX and initiates the sign in procedure.
        /// </summary>
        /// <remarks></remarks>
        private void SendAuthenticationCookie(List<byte> authenticationCookie)
        {
            var flapSendCookie = new FlapSendSignInCookie();

            flapSendCookie.AuthorizationCookie.AuthorizationCookie.AddRange(authenticationCookie);

            Send(flapSendCookie);
        }

        private void BaseInternalDisconnected(object sender, DisconnectEventArgs e)
        {
            //TODO: Provide disconnect messages
            OnDisconnected("Server closed connection.", e.IsExpected);
        }

        private void OnDisconnected(string message, bool expected)
        {
            if (Disconnected != null)
            {
                Disconnected(this, new DisconnectedEventArgs(message, expected));
            }
        }

        private void OnSignInFailed(string message)
        {
            if (SignInFailed != null)
            {
                SignInFailed(this, new SignInFailedEventArgs(message));
            }
        }

        private void OnSignInCompleted()
        {
            if (SignInCompleted != null)
            {
                SignInCompleted(this, EventArgs.Empty);
            }
        }

        private void AnalyseSnac0101(Snac0101 dataIn)
        {
            try
            {
                if (_isSigningIn)
                    OnSignInFailed(string.Format("Error: {0}, Sub Code: {1}", dataIn.ErrorCode,
                        dataIn.SubError.ErrorSubCode));

                throw new IcqException(dataIn.ServiceId, dataIn.ErrorCode, dataIn.SubError.ErrorSubCode);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0103(Snac0103 dataIn)
        {
            try
            {
                var requiredVersions = new List<int>(new[]
                {
                    0x1,
                    0x2,
                    0x3,
                    0x4,
                    0x9,
                    0x13,
                    0x15
                });

                var notSupported = new List<int>();

                foreach (var x in requiredVersions)
                {
                    if (!dataIn.ServerSupportedFamilyIds.Contains(x))
                    {
                        notSupported.Add(x);
                    }
                }

                if (notSupported.Count == 0)
                {
                    var dataOut = new Snac0117();

                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x1, 0x4));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x13, 0x4));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x2, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x3, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x15, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x4, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x6, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0x9, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0xa, 0x1));
                    dataOut.FamilyNameVersionPairs.Add(new FamilyVersionPair(0xb, 0x1));

                    Send(dataOut);
                }
                else
                {
                    throw new NotSupportedException("This server does not support all required features!");
                }
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0118(Snac0118 dataIn)
        {
            try
            {
                var dataOut = new Snac0106();

                Send(dataOut);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0107(Snac0107 dataIn)
        {
            List<int> serverRateGroupIds;
            Snac0108 dataOut;
            Snac dataContactListCheckout;

            try
            {
                serverRateGroupIds = new List<int>();

                foreach (var x in dataIn.RateGroups)
                {
                    serverRateGroupIds.Add(x.GroupId);
                }

                dataOut = new Snac0108();

                dataOut.RateGroupIds.AddRange(serverRateGroupIds);

                var svStorage = Context.GetService<IStorageService>();

                if (svStorage.Info != null)
                {
                    Kernel.Logger.Log("IcqInterface.IcqConnector", TraceEventType.Information,
                        "Requesting Contact List Delta, Items: {0}, Changed: {1}", svStorage.Info.ItemCount,
                        svStorage.Info.DateChanged);
                    dataContactListCheckout = new Snac1305
                    {
                        ModificationDate = svStorage.Info.DateChanged,
                        NumberOfItems = svStorage.Info.ItemCount
                    };
                }
                else
                {
                    Kernel.Logger.Log("IcqInterface.IcqConnector", TraceEventType.Information,
                        "Requesting Complete Contact List");
                    dataContactListCheckout = new Snac1304();
                }

                Send(dataOut, new Snac010E(), new Snac0202(), new Snac0302(), new Snac0404(), new Snac0902(),
                    new Snac1302(), dataContactListCheckout);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac1306(Snac1306 dataIn)
        {
            try
            {
                var dataOut = new Snac1307();

                Send(dataOut);

                CompleteInitialization();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void CompleteInitialization()
        {
            Snac0204 snacUserInfo;

            // In the following the client capabilities are propageted to the server. This is
            // used to show other clients which features of the Icq/Aim network this client
            // supports.
            // At the moment only the icq flag to show that this client is an icq client.
            // When more features are made available more flags have to be set.
            snacUserInfo = new Snac0204();
            snacUserInfo.Capabilities.Capabilites.Add(IcqClientCapabilities.IcqFlag);

            // The following sets up the message channels the client understands.
            // Channel 1: Plain text messages
            // Channel 2: Rich text messages and other communications
            // Channel 4: obsolete

            Snac0402 confChannel01;

            confChannel01 = new Snac0402();
            confChannel01.Channel = 1;
            confChannel01.MessageFlags = 0xb;
            confChannel01.MaxMessageSnacSize = 0x1f40;
            confChannel01.MaxSenderWarningLevel = 0x3e7;
            confChannel01.MaxReceiverWarningLevel = 0x3e7;
            confChannel01.MinimumMessageInterval = 0;

            Snac0402 confChannel02;

            confChannel02 = new Snac0402();
            confChannel02.Channel = 2;
            confChannel02.MessageFlags = 0x3;
            confChannel02.MaxMessageSnacSize = 0x1f40;
            confChannel02.MaxSenderWarningLevel = 0x3e7;
            confChannel02.MaxReceiverWarningLevel = 0x3e7;
            confChannel02.MinimumMessageInterval = 0;

            Snac0402 confChannel04;

            confChannel04 = new Snac0402();
            confChannel04.Channel = 4;
            confChannel04.MessageFlags = 0x3;
            confChannel04.MaxMessageSnacSize = 0x1f40;
            confChannel04.MaxSenderWarningLevel = 0x3e7;
            confChannel04.MaxReceiverWarningLevel = 0x3e7;
            confChannel04.MinimumMessageInterval = 0;

            // Set up "DirectConnection" configuration of the client. This is a
            // peer to peer communication to allow file transfers etc.
            // At the moment it is set to not supported.

            Snac011e extendedStatusRequest;

            extendedStatusRequest = new Snac011e();
            extendedStatusRequest.DCInfo.DcProtocolVersion = 8;
            extendedStatusRequest.DCInfo.DcByte = DcType.DirectConnectionDisabledAuthRequired;

            // The user is not idle so set the idle time to zero.

            Snac0111 setIdleTime;

            setIdleTime = new Snac0111();
            setIdleTime.IdleTime = TimeSpan.FromSeconds(0);

            // This is used to tell the server the understood services.
            // Since this is an Icq Client only icq services are listed.

            Snac0102 supportedServices;
            supportedServices = new Snac0102();
            supportedServices.Families.Add(new FamilyIdToolPair(0x1, 0x4, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x13, 0x4, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x2, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x3, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x15, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x4, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x6, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0x9, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0xa, 0x1, 0x110, 0x8e4));
            supportedServices.Families.Add(new FamilyIdToolPair(0xb, 0x1, 0x110, 0x8e4));

            Send(snacUserInfo, confChannel01, confChannel02, confChannel04, extendedStatusRequest, setIdleTime,
                supportedServices);

            // It is required to run the completion asynchronous. otherwise this call would block the analyzation of
            // data in the analyzation pipe.
            ThreadPool.QueueUserWorkItem(AsyncCompleteSignIn);
        }

        private void AsyncCompleteSignIn(object state)
        {
            IStorageService svStorage;
            EventWaitHandle waitForContactList;

            // now we have to wait for the IcqStorage service to finish contact list analyzation
            // the easiest way to do so is blocking the current thread until the ContactListActivated
            // event is fired.

            svStorage = Context.GetService<IStorageService>();
            waitForContactList = new ManualResetEvent(false);

            // That's why I love linq :)
            svStorage.ContactListActivated += (object sender, EventArgs e) => waitForContactList.Set();

            if (!waitForContactList.WaitOne(TimeSpan.FromSeconds(10), true))
            {
                OnSignInFailed("Timeout while waiting for server response.");
            }
            else
            {
                Context.GetService<IUserInformationService>().RequestShortUserInfo(Context.Identity);

                OnSignInCompleted();
            }
        }
    }
}