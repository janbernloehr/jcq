// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestAuthenticationCookieTask.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using System.Threading.Tasks;
using Jcq.IcqProtocol.Internal;
using Jcq.Core;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    ///// <summary>
    /////     This task requests an authentication cookie for a username and a password.
    ///// </summary>
    //public class RequestAuthenticationCookieTask : BasicAsyncTask, ITaskWithState<RequestAuthenticationCookieState>
    //{
    //    private readonly IcqConnector _connector;
    //    private readonly IPasswordCredential _passwordCredential;
    //    private readonly RequestAuthenticationCookieState _state;

    //    public RequestAuthenticationCookieTask(IcqConnector owner, IPasswordCredential credential)
    //    {
    //        _connector = owner;
    //        _state = new RequestAuthenticationCookieState();
    //        _passwordCredential = credential;

    //        Connector.FlapReceived += OnFlapReceived;
    //    }

    //    /// <summary>
    //    ///     Gets the connector used to process this task.
    //    /// </summary>
    //    public IcqConnector Connector
    //    {
    //        get { return _connector; }
    //    }

    //    /// <summary>
    //    ///     Gets the password credential used to request the authentication cookie.
    //    /// </summary>
    //    public IPasswordCredential PasswordCredential
    //    {
    //        get { return _passwordCredential; }
    //    }

    //    /// <summary>
    //    ///     Gets the current state of the task.
    //    /// </summary>
    //    public RequestAuthenticationCookieState State
    //    {
    //        get { return _state; }
    //    }

    //    protected override void SetCompleted()
    //    {
    //        // When the task is completed we don't have to listen for new
    //        // flaps anymore.
    //        Connector.FlapReceived -= OnFlapReceived;

    //        base.SetCompleted();
    //    }

    //    protected override void PerformOperation()
    //    {
    //        // The only required operation is sending the cookie request
    //        SendAuthenticationCookieRequest(PasswordCredential.Password);
    //    }

    //    /// <summary>
    //    ///     Filters FalpReceived events and passes the appropiate data to analyzation methods.
    //    /// </summary>
    //    private void OnFlapReceived(object sender, FlapTransportEventArgs e)
    //    {
    //        var flap = e.Flap;

    //        try
    //        {
    //            // we can ignore flaps other than connection closed negotiations
    //            if (flap.Channel != FlapChannel.CloseConnectionNegotiation)
    //                return;

    //            AnalyzeConnectionClosedFlap(flap);
    //        }
    //        catch (Exception ex)
    //        {
    //            Kernel.Exceptions.PublishException(ex);
    //        }
    //    }

    //    /// <summary>
    //    ///     Analyzes connection closed negotiation flaps.
    //    /// </summary>
    //    private void AnalyzeConnectionClosedFlap(Flap flap)
    //    {
    //        // we are only interested in tlvs and their type number.
    //        var tlvsByTypeNumer =
    //            (from x in flap.DataItems where x is Tlv select (Tlv)x).ToDictionary(tlv => tlv.TypeNumber);

    //        if (tlvsByTypeNumer.ContainsKey(0x5) & tlvsByTypeNumer.ContainsKey(0x6))
    //        {
    //            // if these tlvs are present the authentication succeeded and everything is okay :)

    //            var bosServerTlv = (TlvBosServerAddress)tlvsByTypeNumer[0x5];
    //            var authCookieTlv = (TlvAuthorizationCookie)tlvsByTypeNumer[0x6];

    //            State.BosServerAddress = bosServerTlv.BosServerAddress;
    //            State.AuthCookie = authCookieTlv.AuthorizationCookie;
    //            State.AuthenticationSucceeded = true;

    //            SetCompleted();
    //        }
    //        else if (tlvsByTypeNumer.ContainsKey(0x8))
    //        {
    //            // if this tlv is present the authentication has failed.

    //            var authFailedTlv = (TlvAuthFailed)tlvsByTypeNumer[0x8];

    //            Kernel.Logger.Log("IcqConnector", TraceEventType.Error, "Connection to server failed. ErrorSubCode: {0}",
    //                authFailedTlv.ErrorSubCode);

    //            State.AuthenticationSucceeded = false;
    //            State.AuthenticationError = authFailedTlv.ErrorSubCode;

    //            SetCompleted();
    //        }
    //        else
    //        {
    //            // in all other cases something went wrong ...
    //            State.AuthenticationSucceeded = false;

    //            SetCompleted();
    //        }
    //    }

    //    /// <summary>
    //    ///     Sends an authentication cookie request to the server.
    //    /// </summary>
    //    private void SendAuthenticationCookieRequest(string password)
    //    {
    //        var flapRequestCookie = new FlapRequestSignInCookie();

    //        // TODO: Supply correct client information.
    //        flapRequestCookie.ScreenName.Uin = Connector.Context.Identity.Identifier;
    //        flapRequestCookie.Password.Password = password;
    //        flapRequestCookie.ClientIdString.ClientIdString = "SomeClientSoftware";
    //        flapRequestCookie.ClientId.ClientId = 8123;
    //        flapRequestCookie.ClientMajorVersion.ClientMajorVersion = 3;
    //        flapRequestCookie.ClientMinorVersion.ClientMinorVersion = 9;
    //        flapRequestCookie.ClientLesserVersion.ClientLesserVersion = 7;
    //        flapRequestCookie.ClientBuildNumber.ClientBuildNumber = 8;
    //        flapRequestCookie.ClientDistributionNumber.ClientDistributionNumber = 1;
    //        flapRequestCookie.ClientLanguage.ClientLanguage = "en";
    //        flapRequestCookie.ClientCountry.ClientCountry = "us";

    //        Connector.Send(flapRequestCookie);
    //    }
    //}

    public class RequestAuthenticationCookieUnitOfWork
    {
        private readonly IcqConnector _connector;
        private readonly RequestAuthenticationCookieState _state;

        private readonly TaskCompletionSource<RequestAuthenticationCookieState> _taskCompletionSource =
            new TaskCompletionSource<RequestAuthenticationCookieState>();

        public RequestAuthenticationCookieUnitOfWork(IcqConnector owner)
        {
            _connector = owner;
            _state = new RequestAuthenticationCookieState();

            Connector.FlapReceived += OnFlapReceived;
        }

        /// <summary>
        ///     Gets the connector used to process this task.
        /// </summary>
        public IcqConnector Connector
        {
            get { return _connector; }
        }

        protected void SetCompleted(RequestAuthenticationCookieState state)
        {
            // When the task is completed we don't have to listen for new
            // flaps anymore.
            Connector.FlapReceived -= OnFlapReceived;

            _taskCompletionSource.SetResult(state);
        }

        public async Task<RequestAuthenticationCookieState> SendRequest(IPasswordCredential credential)
        {
            var flapRequestCookie = new FlapRequestSignInCookie();

            // TODO: Supply correct client information.
            flapRequestCookie.ScreenName.Uin = Connector.Context.Identity.Identifier;
            flapRequestCookie.Password.Password = credential.Password;
            flapRequestCookie.ClientIdString.ClientIdString = "SomeClientSoftware";
            flapRequestCookie.ClientId.ClientId = 8123;
            flapRequestCookie.ClientMajorVersion.ClientMajorVersion = 3;
            flapRequestCookie.ClientMinorVersion.ClientMinorVersion = 9;
            flapRequestCookie.ClientLesserVersion.ClientLesserVersion = 7;
            flapRequestCookie.ClientBuildNumber.ClientBuildNumber = 8;
            flapRequestCookie.ClientDistributionNumber.ClientDistributionNumber = 1;
            flapRequestCookie.ClientLanguage.ClientLanguage = "en";
            flapRequestCookie.ClientCountry.ClientCountry = "us";

            await Connector.Send(flapRequestCookie);

            return await _taskCompletionSource.Task;
        }

        /// <summary>
        ///     Filters FalpReceived events and passes the appropiate data to analyzation methods.
        /// </summary>
        private void OnFlapReceived(object sender, FlapTransportEventArgs e)
        {
            var flap = e.Flap;

            try
            {
                // we can ignore flaps other than connection closed negotiations
                if (flap.Channel != FlapChannel.CloseConnectionNegotiation)
                    return;

                AnalyzeConnectionClosedFlap(flap);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        /// <summary>
        ///     Analyzes connection closed negotiation flaps.
        /// </summary>
        private void AnalyzeConnectionClosedFlap(Flap flap)
        {
            var state = new RequestAuthenticationCookieState();

            // we are only interested in tlvs and their type number.
            var tlvsByTypeNumer =
                (from x in flap.DataItems where x is Tlv select (Tlv) x).ToDictionary(tlv => tlv.TypeNumber);

            if (tlvsByTypeNumer.ContainsKey(0x5) & tlvsByTypeNumer.ContainsKey(0x6))
            {
                // if these tlvs are present the authentication succeeded and everything is okay :)

                var bosServerTlv = (TlvBosServerAddress) tlvsByTypeNumer[0x5];
                var authCookieTlv = (TlvAuthorizationCookie) tlvsByTypeNumer[0x6];

                state.BosServerAddress = bosServerTlv.BosServerAddress;
                state.AuthCookie = authCookieTlv.AuthorizationCookie;
                state.AuthenticationSucceeded = true;

                SetCompleted(state);
            }
            else if (tlvsByTypeNumer.ContainsKey(0x8))
            {
                // if this tlv is present the authentication has failed.

                var authFailedTlv = (TlvAuthFailed) tlvsByTypeNumer[0x8];

                Kernel.Logger.Log("IcqConnector", TraceEventType.Error, "Connection to server failed. ErrorSubCode: {0}",
                    authFailedTlv.ErrorSubCode);

                state.AuthenticationSucceeded = false;
                state.AuthenticationError = authFailedTlv.ErrorSubCode;

                SetCompleted(state);
            }
            else
            {
                // in all other cases something went wrong ...
                state.AuthenticationSucceeded = false;

                SetCompleted(state);
            }
        }
    }
}