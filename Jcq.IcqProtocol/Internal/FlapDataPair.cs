// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlapDataPair.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;
using System.Threading.Tasks;
using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol.Internal
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