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
/// <summary>
/// A Task is a well definied object which describes a specific behavior. While the behavior is
/// explicitly implemented in the Task, the actual execution is done from outside. Execution may
/// be synchrouns on the current thread, asynchronous using the ThreadPool or even synchronized
/// with a Dispatcher.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core.Interfaces
{
  public interface ITask
  {
    Guid Id { get; }
    bool IsCompleted { get; }
    bool IsAsync { get; }
    event EventHandler Completed;
    void Run();
    void WaitCompleted();
  }
}

