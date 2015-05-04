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
namespace JCsTools.Core.Exceptions
{
  public class ExceptionInformation : Interfaces.Exceptions.IExceptionInformation
  {
    private bool mHandled;
    private bool mDisplayed;
    private Exception mException;

    public ExceptionInformation(Exception ex)
    {
      mException = ex;
    }

    public ExceptionInformation(Exception ex, bool handled, bool displayed) : this(ex)
    {

      mDisplayed = displayed;
      mHandled = handled;
    }

    public Exception Interfaces.Exceptions.IExceptionInformation.Exception {
      get { return mException; }
    }

    public bool Interfaces.Exceptions.IExceptionInformation.Displayed {
      get { return mDisplayed; }
      set { mDisplayed = value; }
    }

    public bool Interfaces.Exceptions.IExceptionInformation.Handled {
      get { return mHandled; }
      set { mHandled = value; }
    }
  }
}

