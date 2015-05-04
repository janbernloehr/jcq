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
using JCsTools.JCQ.IcqInterface.DataTypes;
/// <summary>
/// Provides the state for an authentication cookie request.
/// </summary>
/// <remarks></remarks>
namespace JCsTools.JCQ.IcqInterface
{
  public class RequestAuthenticationCookieState
  {
    private string _BosServerAddress;

    /// <summary>
    /// Gets or sets the Address of the Sign In Server.
    /// </summary>
    public string BosServerAddress {
      get { return _BosServerAddress; }
      set { _BosServerAddress = value; }
    }

    private List<byte> _AuthCookie;

    /// <summary>
    /// Gets or sets the Authentication Cookie.
    /// </summary>
    public List<byte> AuthCookie {
      get { return _AuthCookie; }
      set { _AuthCookie = value; }
    }

    private bool _AuthenticationSucceeded;

    /// <summary>
    /// Gets or sets a value indicating whether the sign in succeeded.
    /// </summary>
    public bool AuthenticationSucceeded {
      get { return _AuthenticationSucceeded; }
      set { _AuthenticationSucceeded = value; }
    }

    private IcqInterface.DataTypes.AuthFailedCode _AuthenticationError;

    /// <summary>
    /// Gets or sets the authentication error.
    /// </summary>
    public IcqInterface.DataTypes.AuthFailedCode AuthenticationError {
      get { return _AuthenticationError; }
      set { _AuthenticationError = value; }
    }
  }

  /// <summary>
  /// This task requests an authentication cookie for a username and a password.
  /// </summary>
  public class RequestAuthenticationCookieTask : Core.BasicAsyncTask, Core.Interfaces.ITaskWithState<RequestAuthenticationCookieState>
  {
    private readonly Interfaces.IPasswordCredential _PasswordCredential;
    private readonly RequestAuthenticationCookieState _State;
    private readonly IcqConnector _Connector;

    public RequestAuthenticationCookieTask(IcqConnector owner, Interfaces.IPasswordCredential credential)
    {
      _Connector = owner;
      _State = new RequestAuthenticationCookieState();
      _PasswordCredential = credential;

      Connector.FlapReceived += OnFlapReceived;
    }

    /// <summary>
    /// Gets the connector used to process this task.
    /// </summary>
    public IcqConnector Connector {
      get { return _Connector; }
    }

    /// <summary>
    /// Gets the password credential used to request the authentication cookie.
    /// </summary>
    public Interfaces.IPasswordCredential PasswordCredential {
      get { return _PasswordCredential; }
    }

    /// <summary>
    /// Gets the current state of the task.
    /// </summary>
    public RequestAuthenticationCookieState Core.Interfaces.ITaskWithState<RequestAuthenticationCookieState>.State {
      get { return _State; }
    }

    protected override void SetCompleted()
    {
      base.SetCompleted();

      // When the task is completed we don't have to listen for new
      // flaps anymore.
      Connector.FlapReceived -= OnFlapReceived;
    }

    protected override void PerformOperation()
    {
      // The only required operation is sending the cookie request
      SendAuthenticationCookieRequest(PasswordCredential.Password);
    }

    /// <summary>
    /// Filters FalpReceived events and passes the appropiate data to analyzation methods.
    /// </summary>
    private void OnFlapReceived(object sender, FlapTransportEventArgs e)
    {
      Flap flap = e.Flap;

      try {
        // we can ignore flaps other than connection closed negotiations
        if (flap.Channel != FlapChannel.CloseConnectionNegotiation)
          return;

        AnalyzeConnectionClosedFlap(flap);
      } catch (Exception ex) {
        JCsTools.Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    /// <summary>
    /// Analyzes connection closed negotiation flaps.
    /// </summary>
    private void AnalyzeConnectionClosedFlap(Flap flap)
    {
      Dictionary<int, Tlv> tlvsByTypeNumer;

      // we are only interested in tlvs and their type number.
      tlvsByTypeNumer = (from x in flap.DataItemswhere x is Tlv(Tlv)x).ToDictionary(tlv => tlv.TypeNumber);

      if (tlvsByTypeNumer.ContainsKey(0x5) & tlvsByTypeNumer.ContainsKey(0x6)) {
        // if these tlvs are present the authentication succeeded and everything is okay :)

        object bosServerTlv = (TlvBosServerAddress)tlvsByTypeNumer(0x5);
        object authCookieTlv = (TlvAuthorizationCookie)tlvsByTypeNumer(0x6);

        State.BosServerAddress = bosServerTlv.BosServerAddress;
        State.AuthCookie = authCookieTlv.AuthorizationCookie;
        State.AuthenticationSucceeded = true;

        SetCompleted();
      } else if (tlvsByTypeNumer.ContainsKey(0x8)) {
        // if this tlv is present the authentication has failed.

        object authFailedTlv = (TlvAuthFailed)tlvsByTypeNumer(0x8);

        Core.Kernel.Logger.Log("IcqConnector", TraceEventType.Error, "Connection to server failed. ErrorSubCode: {0}", authFailedTlv.ErrorSubCode.ToString);

        State.AuthenticationSucceeded = false;
        State.AuthenticationError = authFailedTlv.ErrorSubCode;

        SetCompleted();
      } else {
        // in all other cases something went wrong ...
        State.AuthenticationSucceeded = false;

        SetCompleted();
      }
    }

    /// <summary>
    /// Sends an authentication cookie request to the server.
    /// </summary>
    private void SendAuthenticationCookieRequest(string password)
    {
      FlapRequestSignInCookie flapRequestCookie;

      flapRequestCookie = new FlapRequestSignInCookie();

      // TODO: Supply correct client information.
      flapRequestCookie.ScreenName.Uin = Connector.Context.Identity.Identifier;
      flapRequestCookie.Password.Password = password;
      flapRequestCookie.ClientIdString.ClientIdString = "SomeClientSoftware";
      flapRequestCookie.ClientId.ClientId = 8123;
      flapRequestCookie.ClientMajorVersion.ClientMajorVersion = 3;
      flapRequestCookie.ClientMinorVersion.ClientMinorVersion = 9;
      flapRequestCookie.ClientLesserVersion.ClientLesserVersion = 7;
      flapRequestCookie.ClientBuildNumber.ClientBuildNumber = 8;
      flapRequestCookie.ClientDistributionNumber.ClientDistributionNumber = 1;
      flapRequestCookie.ClientLanguage.ClientLanguage = "en";
      flapRequestCookie.ClientCountry.ClientCountry = "us";

      Connector.Send(flapRequestCookie);
    }
  }

  public partial class IcqConnector : BaseConnector, Interfaces.IConnector
  {
    private bool isSigningIn;

    public event SignInCompletedEventHandler SignInCompleted;
    public delegate void SignInCompletedEventHandler(object sender, System.EventArgs e);

    public event SignInFailedEventHandler SignInFailed;
    public delegate void SignInFailedEventHandler(object sender, Interfaces.SignInFailedEventArgs e);

    public event DisconnectedEventHandler Disconnected;
    public delegate void DisconnectedEventHandler(object sender, Interfaces.DisconnectedEventArgs e);

    public IcqConnector(Interfaces.IContext context) : base(context)
    {

      RegisterSnacHandler<Snac0101>(0x1, 0x1, AnalyseSnac0101);
      RegisterSnacHandler<Snac0103>(0x1, 0x3, AnalyseSnac0103);
      RegisterSnacHandler<Snac0107>(0x1, 0x7, AnalyseSnac0107);
      RegisterSnacHandler<Snac0118>(0x1, 0x18, AnalyseSnac0118);

      RegisterSnacHandler<Snac1306>(0x13, 0x6, AnalyseSnac1306);
    }

    public void Interfaces.IConnector.SignIn(Interfaces.ICredential credential)
    {
      Interfaces.IPasswordCredential password;

      System.Net.IPEndPoint serverEndpoint;

      RequestAuthenticationCookieTask requestAuthCookieTask;

      password = credential as Interfaces.IPasswordCredential;
      if (password == null)
        throw new ArgumentException("Credential musst be of Type IPasswordCredential", "credential");

      try {
        isSigningIn = true;

        // Connect to the icq server and get a bos server address and and authentication cookie.
        InnerConnect();

        requestAuthCookieTask = new RequestAuthenticationCookieTask(this, password);

        requestAuthCookieTask.Run();

        // When the task is run, we can exspect a disconnect ...
        TcpContext.SetCloseExpected();

        requestAuthCookieTask.WaitCompleted();

        if (!requestAuthCookieTask.State.AuthenticationSucceeded) {
          // The authentication attempt was not successfull. There are many reasons for this
          // to occur for example wrong password, to many connections etc.
          // The client needs to be informed that the sign in failed.

          OnSignInFailed(requestAuthCookieTask.State.AuthenticationError.ToString);
          isSigningIn = false;
          return;
        }

        // if the authentication attempt was successfull we can connect to the bos server
        // and send the just received authentication cookie to begin the sign in procedure.

        serverEndpoint = ConvertServerAddressToEndPoint(requestAuthCookieTask.State.BosServerAddress);

        InnerConnect(serverEndpoint);

        SendAuthenticationCookie(requestAuthCookieTask.State.AuthCookie);
      } catch {
        isSigningIn = false;

        throw;
      }
    }

    public void Interfaces.IConnector.SignOut()
    {
      // TODO: Implement proper Sign out.
      throw new NotImplementedException();
    }

    /// <summary>
    /// Sends the cookie received from the authentication server to the bos server.
    /// The server replies with SnacXXX and initiates the sign in procedure.
    /// </summary>
    /// <remarks></remarks>
    private void SendAuthenticationCookie(List<byte> authenticationCookie)
    {
      FlapSendSignInCookie flapSendCookie;

      flapSendCookie = new FlapSendSignInCookie();
      flapSendCookie.AuthorizationCookie.AuthorizationCookie.AddRange(authenticationCookie);

      Send(flapSendCookie);
    }

    private void  // ERROR: Handles clauses are not supported in C#
BaseInternalDisconnected(object sender, DisconnectEventArgs e)
    {
      //TODO: Provide disconnect messages
      OnDisconnected("Server closed connection.", e.IsExpected);
    }

    private void OnDisconnected(string message, bool expected)
    {
      if (Disconnected != null) {
        Disconnected(this, new Interfaces.DisconnectedEventArgs(message, expected));
      }
    }

    private void OnSignInFailed(string message)
    {
      if (SignInFailed != null) {
        SignInFailed(this, new Interfaces.SignInFailedEventArgs(message));
      }
    }

    private void OnSignInCompleted()
    {
      if (SignInCompleted != null) {
        SignInCompleted(this, EventArgs.Empty);
      }
    }

    private void AnalyseSnac0101(Snac0101 dataIn)
    {
      try {
        if (isSigningIn)
          OnSignInFailed(string.Format("Error: {0}, Sub Code: {1}", dataIn.ErrorCode.ToString, dataIn.SubError.ErrorSubCode));

        throw new IcqException(dataIn.ServiceId, dataIn.ErrorCode, dataIn.SubError.ErrorSubCode);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void AnalyseSnac0103(Snac0103 dataIn)
    {
      List<int> requiredVersions;
      List<int> notSupported;

      try {
        requiredVersions = new List<int>(new int[] {
          0x1,
          0x2,
          0x3,
          0x4,
          0x9,
          0x13,
          0x15
        });
        notSupported = new List<int>();

        foreach (int x in requiredVersions) {
          if (!dataIn.ServerSupportedFamilyIds.Contains(x)) {
            notSupported.Add(x);
          }
        }

        if (notSupported.Count == 0) {
          Snac0117 dataOut = new Snac0117();

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
        } else {
          throw new NotSupportedException("This server does not support all required features!");
        }
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void AnalyseSnac0118(Snac0118 dataIn)
    {
      try {
        Snac0106 dataOut = new Snac0106();

        Send(dataOut);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void AnalyseSnac0107(Snac0107 dataIn)
    {
      List<int> serverRateGroupIds;
      Snac0108 dataOut;
      Snac dataContactListCheckout;

      try {
        serverRateGroupIds = new List<int>();

        foreach (RateGroup x in dataIn.RateGroups) {
          serverRateGroupIds.Add(x.GroupId);
        }

        dataOut = new Snac0108();

        dataOut.RateGroupIds.AddRange(serverRateGroupIds);

        object svStorage = Context.GetService<Interfaces.IStorageService>();

        if (svStorage.Info != null) {
          Core.Kernel.Logger.Log("IcqInterface.IcqConnector", TraceEventType.Information, "Requesting Contact List Delta, Items: {0}, Changed: {1}", svStorage.Info.ItemCount, svStorage.Info.DateChanged);
          dataContactListCheckout = new Snac1305 {
            ModificationDate = svStorage.Info.DateChanged,
            NumberOfItems = svStorage.Info.ItemCount
          };
        } else {
          Core.Kernel.Logger.Log("IcqInterface.IcqConnector", TraceEventType.Information, "Requesting Complete Contact List");
          dataContactListCheckout = new Snac1304();
        }

        Send(dataOut, new Snac010E(), new Snac0202(), new Snac0302(), new Snac0404(), new Snac0902(), new Snac1302(), dataContactListCheckout);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void AnalyseSnac1306(Snac1306 dataIn)
    {
      Snac1307 dataOut;

      try {
        dataOut = new Snac1307();

        Send(dataOut);

        CompleteInitialization();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
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
      snacUserInfo.Capabilities.Capabilites.Add(DataTypes.IcqClientCapabilities.IcqFlag);

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

      Send(snacUserInfo, confChannel01, confChannel02, confChannel04, extendedStatusRequest, setIdleTime, supportedServices);

      // It is required to run the completion asynchronous. otherwise this call would block the analyzation of
      // data in the analyzation pipe.
      Threading.ThreadPool.QueueUserWorkItem(new Threading.WaitCallback(AsyncCompleteSignIn));
    }

    private void AsyncCompleteSignIn(object state)
    {
      Interfaces.IStorageService svStorage;
      Threading.EventWaitHandle waitForContactList;

      // now we have to wait for the IcqStorage service to finish contact list analyzation
      // the easiest way to do so is blocking the current thread until the ContactListActivated
      // event is fired.

      svStorage = Context.GetService<Interfaces.IStorageService>();
      waitForContactList = new Threading.ManualResetEvent(false);

      // That's why I love linq :)
      svStorage.ContactListActivated += (object sender, EventArgs e) => waitForContactList.Set;

      if (!waitForContactList.WaitOne(TimeSpan.FromSeconds(10), true)) {
        OnSignInFailed("Timeout while waiting for server response.");
      } else {
        Context.GetService<Interfaces.IUserInformationService>.RequestShortUserInfo(Context.Identity);

        OnSignInCompleted();
      }
    }

  }

  public class PasswordCredential : Interfaces.IPasswordCredential
  {
    private string _Password;

    public PasswordCredential(string pwd)
    {
      _Password = pwd;
    }

    public string Interfaces.IPasswordCredential.Password {
      get { return _Password; }
    }
  }

  public class IcqException : ApplicationException
  {
    public IcqException(int serviceId, IcqInterface.DataTypes.ErrorCode code, int subcode) : base(string.Format("Service: {0} ErrorCode: {1} ErrorSubCode: {2}", serviceId, code, subcode))
    {
      _ServiceId = serviceId;
      _ErrorCode = code;
      _ErrorSubCode = subcode;
    }

    private IcqInterface.DataTypes.ErrorCode _ErrorCode;
    public IcqInterface.DataTypes.ErrorCode ErrorCode {
      get { return _ErrorCode; }
    }

    private int _ErrorSubCode;
    public int ErrorSubCode {
      get { return _ErrorSubCode; }
    }

    private int _ServiceId;
    public int ServiceId {
      get { return _ServiceId; }
    }
  }
}

