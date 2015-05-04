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
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqPrivacyService : ContextService, Interfaces.IPrivacyService
  {
    private readonly Core.Interfaces.IReadOnlyNotifyingCollection<Interfaces.IContact> _IgnoreList;
    private readonly Core.Interfaces.IReadOnlyNotifyingCollection<Interfaces.IContact> _InvisibleList;
    private readonly Core.Interfaces.IReadOnlyNotifyingCollection<Interfaces.IContact> _VisibleList;

    public Core.Interfaces.IReadOnlyNotifyingCollection<Interfaces.IContact> Interfaces.IPrivacyService.IgnoreList {
      get { return _IgnoreList; }
    }

    public Core.Interfaces.IReadOnlyNotifyingCollection<Interfaces.IContact> Interfaces.IPrivacyService.InvisibleList {
      get { return _InvisibleList; }
    }

    public Core.Interfaces.IReadOnlyNotifyingCollection<Interfaces.IContact> Interfaces.IPrivacyService.VisibleList {
      get { return _VisibleList; }
    }

    public IcqPrivacyService(Interfaces.IContext context) : base(context)
    {

      IcqConnector connector = context.GetService<Interfaces.IConnector>() as IcqConnector;

      if (connector == null)
        throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

      IcqStorageService svStorage = context.GetService<Interfaces.IStorageService>() as IcqStorageService;

      _IgnoreList = svStorage.IgnoreList;
      _InvisibleList = svStorage.InvisibleList;
      _VisibleList = svStorage.VisibleList;
    }

    public void Interfaces.IPrivacyService.AddContactToIgnoreList(Interfaces.IContact contact)
    {
      AddContactToIgnoreListTransaction trans;
      IcqStorageService svStorage;
      IcqContact icontact;

      svStorage = (IcqStorageService)Context.GetService<Interfaces.IStorageService>();
      icontact = (IcqContact)contact;

      trans = new AddContactToIgnoreListTransaction(svStorage, icontact);

      svStorage.CommitSSITransaction(trans);
    }

    public void Interfaces.IPrivacyService.AddContactToInvisibleList(Interfaces.IContact contact)
    {
      AddContactToInvisibleListTransaction trans;
      IcqStorageService svStorage;
      IcqContact icontact;

      svStorage = (IcqStorageService)Context.GetService<Interfaces.IStorageService>();
      icontact = (IcqContact)contact;

      trans = new AddContactToInvisibleListTransaction(svStorage, icontact);

      svStorage.CommitSSITransaction(trans);
    }

    public void Interfaces.IPrivacyService.AddContactToVisibleList(Interfaces.IContact contact)
    {
      AddContactToVisibleListTransaction trans;
      IcqStorageService svStorage;
      IcqContact icontact;

      svStorage = (IcqStorageService)Context.GetService<Interfaces.IStorageService>();
      icontact = (IcqContact)contact;

      trans = new AddContactToVisibleListTransaction(svStorage, icontact);

      svStorage.CommitSSITransaction(trans);
    }

    public void Interfaces.IPrivacyService.RemoveContactFromIgnoreList(Interfaces.IContact contact)
    {
      RemoveContactFromIgnoreListTransaction trans;
      IcqStorageService svStorage;
      IcqContact icontact;

      svStorage = (IcqStorageService)Context.GetService<Interfaces.IStorageService>();
      icontact = (IcqContact)contact;

      trans = new RemoveContactFromIgnoreListTransaction(svStorage, icontact);

      svStorage.CommitSSITransaction(trans);
    }

    public void Interfaces.IPrivacyService.RemoveContactFromInvisibleList(Interfaces.IContact contact)
    {
      RemoveContactFromInvisibleListTransaction trans;
      IcqStorageService svStorage;
      IcqContact icontact;

      svStorage = (IcqStorageService)Context.GetService<Interfaces.IStorageService>();
      icontact = (IcqContact)contact;

      trans = new RemoveContactFromInvisibleListTransaction(svStorage, icontact);

      svStorage.CommitSSITransaction(trans);
    }

    public void Interfaces.IPrivacyService.RemoveContactFromVisibleList(Interfaces.IContact contact)
    {
      RemoveContactFromVisibleListTransaction trans;
      IcqStorageService svStorage;
      IcqContact icontact;

      svStorage = (IcqStorageService)Context.GetService<Interfaces.IStorageService>();
      icontact = (IcqContact)contact;

      trans = new RemoveContactFromVisibleListTransaction(svStorage, icontact);

      svStorage.CommitSSITransaction(trans);
    }

  }
}

