// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationService.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using Jcq.IcqProtocol;
using Jcq.IcqProtocol.Contracts;
using Jcq.IdentityManager;
using Jcq.IdentityManager.Contracts;
using Jcq.Ux.ViewModel.Contracts;

namespace Jcq.Ux.ViewModel
{
    /// <summary>
    ///     Provides access to Services shared accross the application.
    /// </summary>
    public sealed class ApplicationService
    {
        private JsonIdentityProvider _identityProvider;

        private ApplicationService()
        {
        }

        /// <summary>
        ///     Gets the current ApplicationService.
        /// </summary>
        /// <remarks>Current is available after calling Initialize.</remarks>
        public static ApplicationService Current { get; private set; }

        /// <summary>
        ///     Gets the directory where application data is stored.
        /// </summary>
        public DirectoryInfo DataStorageDirectory { get; private set; }

        /// <summary>
        ///     Gets the current IContext.
        /// </summary>
        public IContext Context { get; private set; }

        public static IContext CurrentContext
        {
            get { return Current.Context; }
        }

        /// <summary>
        ///     Gets the current IIdentityProvider.
        /// </summary>
        public IIdentityProvider IdentityProvider
        {
            get { return _identityProvider; }
        }

        /// <summary>
        ///     Gets the current Identity.
        /// </summary>
        public IcqIdentity Identity { get; private set; }

        /// <summary>
        ///     Initializes an ApplicationService for the current application.
        /// </summary>
        /// <param name="dataStorage">The directory where application data is stored.</param>
        public static void Initialize(DirectoryInfo dataStorage)
        {
            var jsonIdentities = new FileInfo(Path.Combine(dataStorage.FullName, "identitystore.json"));

            Current = new ApplicationService
            {
                DataStorageDirectory = dataStorage,
                _identityProvider = new JsonIdentityProvider(jsonIdentities)
            };
        }

        public IContext CreateContext(IcqIdentity identity)
        {
            Identity = identity;
            Context = new IcqContext(identity.IcqUin);

            LoadContextData(Context);

            return Context;
        }

        private void LoadContextData(IContext ctx)
        {
            ctx.GetService<IDataWarehouseService>().Load(DataStorageDirectory);
            ctx.GetService<IContactHistoryService>().Load();
        }

        private void SaveContextData(IContext ctx)
        {
            ctx.GetService<IDataWarehouseService>().Save(DataStorageDirectory);
            ctx.GetService<IContactHistoryService>().Save();
        }

        public void LoadServiceData()
        {
            _identityProvider.Load();
        }

        public void SaveServiceData()
        {
            if (Context != null)
                SaveContextData(Context);

            _identityProvider.Save();
        }
    }
}