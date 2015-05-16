// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapDataPair.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Threading.Tasks;
using JCsTools.JCQ.IcqInterface.DataTypes;

namespace JCsTools.JCQ.IcqInterface.Internal
{
    public class FlapDataPair
    {
        private readonly List<byte> _data;
        private readonly Flap _flap;
        //private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);
        private readonly TaskCompletionSource<int> _taskCompletionSource = new TaskCompletionSource<int>();

        public FlapDataPair(Flap f)
        {
            _flap = f;
            _data = f.Serialize();
        }

        /// <summary>
        ///     Gets the Flap.
        /// </summary>
        public Flap Flap
        {
            get { return _flap; }
        }

        /// <summary>
        ///     Gets the Serialization of the Flap.
        /// </summary>
        public List<byte> Data
        {
            get { return _data; }
        }

        //public void Release()
        //{
        //    _semaphore.Release();
        //}
        //public Task WaitAsync()
        //{
        //    return _semaphore.WaitAsync();
        //}


        public TaskCompletionSource<int> TaskCompletionSource
        {
            get { return _taskCompletionSource; }
        }
    }
}