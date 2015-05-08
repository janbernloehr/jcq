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
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        private TaskCompletionSource<bool> _signInTaskCompletionSource;
        private SemaphoreSlim _contactListActivated;

        public IcqConnector(IContext context)
            : base(context)
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

        //public void SignIn(ICredential credential)
        //{
        //    var password = credential as IPasswordCredential;
        //    if (password == null)
        //        throw new ArgumentException("Credential musst be of Type IPasswordCredential", "credential");

        //    try
        //    {
        //        _isSigningIn = true;

        //        // Connect to the icq server and get a bos server address and and authentication cookie.
        //        InnerConnect();

        //        var requestAuthCookieTask = new RequestAuthenticationCookieTask(this, password);

        //        requestAuthCookieTask.Run();

        //        // When the task is run, we can exspect a disconnect ...
        //        TcpContext.SetCloseExpected();

        //        requestAuthCookieTask.WaitCompleted();

        //        if (!requestAuthCookieTask.State.AuthenticationSucceeded)
        //        {
        //            // The authentication attempt was not successfull. There are many reasons for this
        //            // to occur for example wrong password, to many connections etc.
        //            // The client needs to be informed that the sign in failed.

        //            OnSignInFailed(requestAuthCookieTask.State.AuthenticationError.ToString());
        //            _isSigningIn = false;
        //            return;
        //        }

        //        // if the authentication attempt was successfull we can connect to the bos server
        //        // and send the just received authentication cookie to begin the sign in procedure.

        //        var serverEndpoint = ConvertServerAddressToEndPoint(requestAuthCookieTask.State.BosServerAddress);

        //        InnerConnect(serverEndpoint);

        //        SendAuthenticationCookie(requestAuthCookieTask.State.AuthCookie);
        //    }
        //    catch
        //    {
        //        _isSigningIn = false;

        //        throw;
        //    }
        //}

        public async Task<bool> SignInAsync(ICredential credential)
        {
            var password = credential as IPasswordCredential;

            if (password == null)
                throw new ArgumentException(@"Credential musst be of Type IPasswordCredential", "credential");

            try
            {
                _isSigningIn = true;
                _signInTaskCompletionSource = new TaskCompletionSource<bool>();
                _contactListActivated = new SemaphoreSlim(0, 1);

                // Connect to the icq server and get a bos server address and and authentication cookie.
                InnerConnect();

                var authenticationCookieUnitOfWork = new RequestAuthenticationCookieUnitOfWork(this);

                var authenticationCookieTask = authenticationCookieUnitOfWork.SendRequest(password);

                // When the task is run, we can exspect a disconnect ...
                TcpContext.SetCloseExpected();

                var authenticationCookieState = await authenticationCookieTask;

                if (!authenticationCookieState.AuthenticationSucceeded)
                {
                    // The authentication attempt was not successfull. There are many reasons for this
                    // to occur for example wrong password, too many connections etc.
                    // The client needs to be informed that the sign in failed.

                    OnSignInFailed(authenticationCookieState.AuthenticationError.ToString());
                    return false;
                }

                // if the authentication attempt was successfull we can connect to the bos server
                // and send the just received authentication cookie to begin the sign in procedure.

                var serverEndpoint = ConvertServerAddressToEndPoint(authenticationCookieState.BosServerAddress);

                InnerConnect(serverEndpoint);

                Context.GetService<IStorageService>().ContactListActivated += (s, e) => _contactListActivated.Release();

                await SendAuthenticationCookie(authenticationCookieState.AuthCookie);

                return await _signInTaskCompletionSource.Task;
            }
            finally
            {
                _isSigningIn = false;
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
        private Task<int> SendAuthenticationCookie(List<byte> authenticationCookie)
        {
            var flapSendCookie = new FlapSendSignInCookie();

            flapSendCookie.AuthorizationCookie.AuthorizationCookie.AddRange(authenticationCookie);

            return Send(flapSendCookie);
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
            if (!_isSigningIn)
                return;

            try
            {
                OnSignInFailed(string.Format("Error: {0}, Sub Code: {1}", dataIn.ErrorCode,
                    dataIn.SubError.ErrorSubCode));

                throw new IcqException(dataIn.ServiceId, dataIn.ErrorCode, dataIn.SubError.ErrorSubCode);
            }
            catch (Exception ex)
            {
                _signInTaskCompletionSource.SetException(ex);
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0103(Snac0103 dataIn)
        {
            if (!_isSigningIn)
                return;

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

                var notSupported = requiredVersions.Where(x => !dataIn.ServerSupportedFamilyIds.Contains(x)).ToList();

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
                _signInTaskCompletionSource.SetException(ex);
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0118(Snac0118 dataIn)
        {
            if (!_isSigningIn)
                return;

            try
            {
                var dataOut = new Snac0106();

                Send(dataOut);
            }
            catch (Exception ex)
            {
                _signInTaskCompletionSource.SetException(ex);
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac0107(Snac0107 dataIn)
        {
            if (!_isSigningIn)
                return;

            try
            {
                var serverRateGroupIds = dataIn.RateGroups.Select(x => x.GroupId).ToList();

                var dataOut = new Snac0108();

                dataOut.RateGroupIds.AddRange(serverRateGroupIds);

                var svStorage = Context.GetService<IStorageService>();

                Snac dataContactListCheckout;

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
                _signInTaskCompletionSource.SetException(ex);
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void AnalyseSnac1306(Snac1306 dataIn)
        {
            if (!_isSigningIn)
                return;

            try
            {
                var dataOut = new Snac1307();

                Send(dataOut);

                Task.Run(async () =>
                {
                    try
                    {
                        await CompleteInitialization();
                    }
                    catch (Exception iex)
                    {
                        _signInTaskCompletionSource.SetException(iex);
                    }
                });
            }
            catch (Exception ex)
            {
                _signInTaskCompletionSource.SetException(ex);
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private async Task CompleteInitialization()
        {
            // In the following the client capabilities are propageted to the server. This is
            // used to show other clients which features of the Icq/Aim network this client
            // supports.
            // At the moment only the icq flag to show that this client is an icq client.
            // When more features are made available more flags have to be set.
            var snacUserInfo = new Snac0204();
            snacUserInfo.Capabilities.Capabilites.Add(IcqClientCapabilities.IcqFlag);

            // The following sets up the message channels the client understands.
            // Channel 1: Plain text messages
            // Channel 2: Rich text messages and other communications
            // Channel 4: obsolete

            var confChannel01 = new Snac0402
            {
                Channel = 1,
                MessageFlags = 0xb,
                MaxMessageSnacSize = 0x1f40,
                MaxSenderWarningLevel = 0x3e7,
                MaxReceiverWarningLevel = 0x3e7,
                MinimumMessageInterval = 0
            };

            var confChannel02 = new Snac0402
            {
                Channel = 2,
                MessageFlags = 0x3,
                MaxMessageSnacSize = 0x1f40,
                MaxSenderWarningLevel = 0x3e7,
                MaxReceiverWarningLevel = 0x3e7,
                MinimumMessageInterval = 0
            };

            var confChannel04 = new Snac0402
            {
                Channel = 4,
                MessageFlags = 0x3,
                MaxMessageSnacSize = 0x1f40,
                MaxSenderWarningLevel = 0x3e7,
                MaxReceiverWarningLevel = 0x3e7,
                MinimumMessageInterval = 0
            };

            // Set up "DirectConnection" configuration of the client. This is a
            // peer to peer communication to allow file transfers etc.
            // At the moment it is set to not supported.

            var extendedStatusRequest = new Snac011e();
            extendedStatusRequest.DCInfo.DcProtocolVersion = 8;
            extendedStatusRequest.DCInfo.DcByte = DcType.DirectConnectionDisabledAuthRequired;

            // The user is not idle so set the idle time to zero.

            var setIdleTime = new Snac0111 { IdleTime = TimeSpan.FromSeconds(0) };

            // This is used to tell the server the understood services.
            // Since this is an Icq Client only icq services are listed.

            var supportedServices = new Snac0102();
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

            await Send(snacUserInfo, confChannel01, confChannel02, confChannel04, extendedStatusRequest, setIdleTime,
                supportedServices);

            //TODO: Is this still true?
            // It is required to run the completion asynchronous. otherwise this call would block the analyzation of
            // data in the analyzation pipe.
            //ThreadPool.QueueUserWorkItem(AsyncCompleteSignIn);

            // now we have to wait for the IcqStorage service to finish contact list analyzation
            // the easiest way to do so is blocking the current thread until the ContactListActivated
            // event is fired.

            if (!await _contactListActivated.WaitAsync(TimeSpan.FromSeconds(10)))
            {
                OnSignInFailed("Timeout while waiting for server response.");

                _signInTaskCompletionSource.SetResult(false);
            }
            else
            {
                Context.GetService<IUserInformationService>().RequestShortUserInfo(Context.Identity);

                OnSignInCompleted();

                _signInTaskCompletionSource.SetResult(true);
            }


        }
    }
}