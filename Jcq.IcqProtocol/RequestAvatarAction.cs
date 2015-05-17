// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestAvatarAction.cs" company="Jan-Cornelius Molnar">
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

using System.Diagnostics;
using Jcq.IcqProtocol.DataTypes;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    public class RequestAvatarAction : IAvatarServiceAction
    {
        private readonly IContact _contact;
        private readonly IcqIconService _service;

        public RequestAvatarAction(IcqIconService service, IContact contact)
        {
            _service = service;
            _contact = contact;
        }

        public IContact Contact
        {
            get { return _contact; }
        }

        public IcqIconService Service
        {
            get { return _service; }
        }

        public void Execute()
        {
            if (_contact.IconHash == null)
                return;

            var dataOut = new Snac1004();
            dataOut.IconHash.AddRange(_contact.IconHash);
            dataOut.Uin = _contact.Identifier;

            Debug.WriteLine(string.Format("Requesting Icon for {0}.", _contact.Identifier), "IcqIconService");

            Service.Send(dataOut);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", base.ToString(), _contact.Identifier);
        }
    }
}