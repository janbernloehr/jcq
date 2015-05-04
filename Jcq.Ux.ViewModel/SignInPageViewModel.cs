// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignInPageViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using JCsTools.Core;
using JCsTools.IdentityManager;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class SignInPageViewModel : DispatcherObject
    {
        private readonly ObservableCollection<IIdentity> _identities;
        private NotifyingCollectionBinding<IIdentity> _binding;

        public SignInPageViewModel()
        {
            _identities = new ObservableCollection<IIdentity>(IdentityProvider.Identities.ToList());
            _binding = new NotifyingCollectionBinding<IIdentity>(IdentityProvider.Identities, _identities);

            Identities = CollectionViewSource.GetDefaultView(_identities);
        }

        public IIdentityProvider IdentityProvider
        {
            get { return ApplicationService.Current.IdentityProvider; }
        }

        public ICollectionView Identities { get; private set; }

        public void SignIn(IIdentity identity)
        {
            ApplicationService.Current.CreateContext(identity);

            var svConnect = ApplicationService.Current.Context.GetService<IConnector>();

            svConnect.SignInCompleted += OnSignInCompleted;
            svConnect.SignInFailed += OnSignInFailed;

            var task = new SignInTask();
            task.Credential = new PasswordCredential(identity.GetAttribute(IdentityAttributes.PasswordAttribute));
            Kernel.TaskScheduler.RunAsync(task);

            NavigateToSigningInPage();
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            MessageBox.Show(string.Format("Connection lost; Message: {0}; Expected: {1}", e.Message, e.IsExpected));
        }

        private void OnSignInCompleted(object sender, EventArgs e)
        {
            try
            {
                ApplicationService.Current.Context.GetService<IConnector>().Disconnected += OnDisconnected;
                ApplicationService.Current.Context.GetService<IOfflineMessageService>().AllOfflineMessagesReceived +=
                    OnAllOfflineMessagesReceived;

                MessageWindowViewModel.RegisterEventHandlers();

                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(NavigateToContactsPage));

                ApplicationService.Current.Context.GetService<IOfflineMessageService>().RequestOfflineMessages();

                Kernel.TaskScheduler.RunAsync(new UploadAvatarActivity());

                Kernel.TaskScheduler.RunAsync(new RequestShortUserInfoTask());
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnSignInFailed(object sender, SignInFailedEventArgs e)
        {
            try
            {
                // TODO: Implement propper Sign In failed handling ...
                MessageBox.Show(e.Message, "SignIn failed", MessageBoxButton.OK, MessageBoxImage.Error);

                NavigateToSignInPage();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnAllOfflineMessagesReceived(object sender, EventArgs e)
        {
            try
            {
                ApplicationService.Current.Context.GetService<IOfflineMessageService>().DeleteOfflineMessages();
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        public void NavigateToContactsPage()
        {
            Kernel.Services.GetService<INavigationService>().NavigateToContactsPage();
        }

        public void NavigateToCreateIdentityPage()
        {
            Kernel.Services.GetService<INavigationService>().NavigateToCreateIdentityPage();
        }

        public void NavigateToSigningInPage()
        {
            Kernel.Services.GetService<INavigationService>().NavigateToSigningInPage();
        }

        public void NavigateToSignInPage()
        {
            Kernel.Services.GetService<INavigationService>().NavigateToSignInPage();
        }

        public void NavigateToEditIdentityPage(IIdentity identity)
        {
            Kernel.Services.GetService<INavigationService>().NavigateToEditIdentityPage(identity);
        }
    }
}