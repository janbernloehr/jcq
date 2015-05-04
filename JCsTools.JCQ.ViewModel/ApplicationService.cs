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
using System.IO;
/// <summary>
/// Provides access to Services shared accross the application.
/// </summary>
namespace JCsTools.JCQ.ViewModel
{
  public sealed class ApplicationService
  {
    private ApplicationService()
    {

    }

    private static ApplicationService _Current;

    private DirectoryInfo _DataStorageDirectory;
    private IcqInterface.Interfaces.IContext _Context;
    private IdentityManager.XmlIdentityProvider _IdentityProvider;
    private IdentityManager.IIdentity _Identity;

    /// <summary>
    /// Gets the current ApplicationService.
    /// </summary>
    /// <remarks>Current is available after calling Initialize.</remarks>
    public static ApplicationService Current {
      get { return _Current; }
    }

    /// <summary>
    /// Gets the directory where application data is stored.
    /// </summary>
    public DirectoryInfo DataStorageDirectory {
      get { return _DataStorageDirectory; }
    }

    /// <summary>
    /// Gets the current IContext.
    /// </summary>
    public IcqInterface.Interfaces.IContext Context {
      get { return _Context; }
    }

    public static IcqInterface.Interfaces.IContext CurrentContext {
      get { return Current.Context; }
    }

    /// <summary>
    /// Gets the current IIdentityProvider.
    /// </summary>
    public IdentityManager.IIdentityProvider IdentityProvider {
      get { return _IdentityProvider; }
    }

    /// <summary>
    /// Gets the current Identity.
    /// </summary>
    public IdentityManager.IIdentity Identity {
      get { return _Identity; }
    }

    /// <summary>
    /// Initializes an ApplicationService for the current application.
    /// </summary>
    /// <param name="dataStorage">The directory where application data is stored.</param>
    public static void Initialize(System.IO.DirectoryInfo dataStorage)
    {
      System.IO.FileInfo xmlIdentities;

      xmlIdentities = new System.IO.FileInfo(System.IO.Path.Combine(dataStorage.FullName, "identitystore.xml"));

      _Current = new ApplicationService();
      _Current._DataStorageDirectory = dataStorage;
      _Current._IdentityProvider = new IdentityManager.XmlIdentityProvider(xmlIdentities);
    }

    public IcqInterface.Interfaces.IContext CreateContext(IdentityManager.IIdentity identity)
    {
      _Identity = identity;
      _Context = new IcqInterface.IcqContext(identity.GetAttribute(IdentityAttributes.UinAttribute));

      LoadContextData(_Context);

      return _Context;
    }

    private void LoadContextData(IcqInterface.Interfaces.IContext ctx)
    {
      ctx.GetService<IcqInterface.Interfaces.IDataWarehouseService>.Load(DataStorageDirectory);
      ctx.GetService<IContactHistoryService>.Load();
    }

    private void SaveContextData(IcqInterface.Interfaces.IContext ctx)
    {
      ctx.GetService<IcqInterface.Interfaces.IDataWarehouseService>.Save(DataStorageDirectory);
      ctx.GetService<IContactHistoryService>.Save();
    }

    public void LoadServiceData()
    {
      _IdentityProvider.Load();
    }

    public void SaveServiceData()
    {
      if (Context != null)
        SaveContextData(Context);

      _IdentityProvider.Save();
    }
  }
}

