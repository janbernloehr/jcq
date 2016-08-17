// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignInPageViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Jcq.Core;
using Jcq.Core.Collections;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.IdentityManager.Contracts;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.ViewModel
{
    public class SignInPageViewModel : ViewModelBase
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

        public async Task SignIn(IcqIdentity identity)
        {
            ApplicationService.Current.CreateContext(identity);

            var svConnect = ApplicationService.Current.Context.GetService<IConnector>();

            svConnect.SignInCompleted += OnSignInCompleted;
            svConnect.SignInFailed += OnSignInFailed;

            var credentail = new PasswordCredential(identity.IcqPassword);

            var signInTask = svConnect.SignInAsync(credentail);

            NavigateToSigningInPage();

            await signInTask;
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            MessageBox.Show(string.Format("Connection lost. Message: {0}. Expected: {1}", e.Message, e.IsExpected));
        }

        private void OnSignInCompleted(object sender, EventArgs e)
        {
            try
            {
                ApplicationService.Current.Context.GetService<IConnector>().Disconnected += OnDisconnected;
                ApplicationService.Current.Context.GetService<IOfflineMessageService>().AllOfflineMessagesReceived +=
                    OnAllOfflineMessagesReceived;

                MessageWindowViewModel.RegisterEventHandlers();

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(NavigateToContactsPage));

                ApplicationService.Current.Context.GetService<IOfflineMessageService>().RequestOfflineMessages();

                //Task.Run(() => UploadAvatarUnitOfWork.Execute());

                Task.Run(() => RequestShortUserInfoUnitOfWork.Execute());
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

        public void NavigateToEditIdentityPage(IcqIdentity identity)
        {
            Kernel.Services.GetService<INavigationService>().NavigateToEditIdentityPage(identity);
        }
    }
}