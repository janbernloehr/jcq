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
using System.Collections.ObjectModel;
using System.Linq;
namespace JCsTools.JCQ.ViewModel
{
  public class SignInPageViewModel : System.Windows.Threading.DispatcherObject
  {
    private ObservableCollection<IdentityManager.IIdentity> _Identities;
    private System.ComponentModel.ICollectionView _IdentitiesView;
    private Core.NotifyingCollectionBinding<IdentityManager.IIdentity> _binding;

    public SignInPageViewModel()
    {
      _Identities = new ObservableCollection<IdentityManager.IIdentity>(IdentityProvider.Identities.ToList);
      _binding = new Core.NotifyingCollectionBinding<IdentityManager.IIdentity>(IdentityProvider.Identities, _Identities);

      _IdentitiesView = CollectionViewSource.GetDefaultView(_Identities);
    }

    public IdentityManager.IIdentityProvider IdentityProvider {
      get { return ApplicationService.Current.IdentityProvider; }
    }

    public System.ComponentModel.ICollectionView Identities {
      get { return _IdentitiesView; }
    }

    public void SignIn(IdentityManager.IIdentity identity)
    {
      IcqInterface.Interfaces.IConnector svConnect;
      SignInTask task;

      ApplicationService.Current.CreateContext(identity);

      svConnect = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IConnector>();

      svConnect.SignInCompleted += OnSignInCompleted;
      svConnect.SignInFailed += OnSignInFailed;

      task = new SignInTask();
      task.Credential = new IcqInterface.PasswordCredential(identity.GetAttribute(IdentityAttributes.PasswordAttribute));
      Core.Kernel.TaskScheduler.RunAsync(task);

      NavigateToSigningInPage();
    }

    private void OnDisconnected(object sender, IcqInterface.Interfaces.DisconnectedEventArgs e)
    {
      MessageBox.Show(string.Format("Connection lost; Message: {0}; Expected: {1}", e.Message, e.IsExpected));
    }

    private void OnSignInCompleted(object sender, EventArgs e)
    {
      try {
        ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IConnector>.Disconnected += OnDisconnected;
        ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IOfflineMessageService>.AllOfflineMessagesReceived += OnAllOfflineMessagesReceived;

        MessageWindowViewModel.RegisterEventHandlers();

        Dispatcher.BeginInvoke(Windows.Threading.DispatcherPriority.Normal, new Action(NavigateToContactsPage));

        ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IOfflineMessageService>.RequestOfflineMessages();

        Core.Kernel.TaskScheduler.RunAsync(new UploadAvatarActivity());

        Core.Kernel.TaskScheduler.RunAsync(new RequestShortUserInfoTask());
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnSignInFailed(object sender, IcqInterface.Interfaces.SignInFailedEventArgs e)
    {
      try {
        // TODO: Implement propper Sign In failed handling ...
        MessageBox.Show(e.Message, "SignIn failed", MessageBoxButton.OK, MessageBoxImage.Error);

        NavigateToSignInPage();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnAllOfflineMessagesReceived(object sender, EventArgs e)
    {
      try {
        ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IOfflineMessageService>.DeleteOfflineMessages();
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    public void NavigateToContactsPage()
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToContactsPage();
    }

    public void NavigateToCreateIdentityPage()
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToCreateIdentityPage();
    }

    public void NavigateToSigningInPage()
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToSigningInPage();
    }

    public void NavigateToSignInPage()
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToSignInPage();
    }

    public void NavigateToEditIdentityPage(IdentityManager.IIdentity identity)
    {
      Core.Kernel.Services.GetService<INavigationService>.NavigateToEditIdentityPage(identity);
    }
  }
}

