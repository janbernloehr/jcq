// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewModelService.cs" company="Jan-Cornelius Molnar">
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
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;

namespace JCsTools.JCQ.Ux
{
    public class ContactWindowViewModelService : ContextService, IContactWindowViewModelService
    {
        private readonly Dictionary<ContactViewModel, MessageWindow> _Windows =
            new Dictionary<ContactViewModel, MessageWindow>();

        public ContactWindowViewModelService(IContext context) : base(context)
        {
        }

        public MessageWindowViewModel GetMessageWindowViewModel(ContactViewModel contact)
        {
            lock (_Windows)
            {
                if (!_Windows.ContainsKey(contact))
                {
                    _Windows.Add(contact, new MessageWindow(contact));
                }

                return _Windows[contact].ViewModel;
            }
        }

        public bool IsMessageWindowViewModelAvailable(ContactViewModel contact)
        {
            lock (_Windows)
            {
                return _Windows.ContainsKey(contact);
            }
        }

        public void RemoveMessageWindowViewModel(ContactViewModel contact)
        {
            lock (_Windows)
            {
                _Windows.Remove(contact);
            }
        }
    }
}