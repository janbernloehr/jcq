// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationService.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using Jcq.Ux.ViewModel;
using JCsTools.IdentityManager;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
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
        public IIdentity Identity { get; private set; }

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