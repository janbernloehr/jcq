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
namespace JCsTools.JCQ.ViewModel
{
  public class SignInTask : Core.BasicAsyncTask
  {
    private IcqInterface.Interfaces.IPasswordCredential _Credential;
    public IcqInterface.Interfaces.IPasswordCredential Credential {
      get { return _Credential; }
      set { _Credential = value; }
    }

    private void OnSignInFailed(object sender, IcqInterface.Interfaces.SignInFailedEventArgs e)
    {
      SetException(new ApplicationException(e.Message));
      SetCompleted();
    }

    private void OnSignInCompleted(object sender, EventArgs e)
    {
      SetCompleted();
    }

    protected override void PerformOperation()
    {
      IcqInterface.Interfaces.IConnector svSignIn;

      svSignIn = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IConnector>();

      svSignIn.SignIn(Credential);

      svSignIn.SignInFailed += OnSignInFailed;
      svSignIn.SignInCompleted += OnSignInCompleted;
    }
  }
}

