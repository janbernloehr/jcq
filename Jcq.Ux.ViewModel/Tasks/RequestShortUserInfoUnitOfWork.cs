// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestShortUserInfoTask.cs" company="Jan-Cornelius Molnar">
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

using System.Threading;
using System.Threading.Tasks;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class RequestShortUserInfoUnitOfWork
    {
        public static Task Execute()
        {
            var svShortUserInfo = ApplicationService.Current.Context.GetService<IUserInformationService>();

            var sem = new SemaphoreSlim(0, 1);

            svShortUserInfo.RequestShortUserInfoForAllUsers();

            svShortUserInfo.RequestShortUserInfoForAllUsersCompleted += (s, e) => sem.Release();

            return sem.WaitAsync();
        }
    }
}