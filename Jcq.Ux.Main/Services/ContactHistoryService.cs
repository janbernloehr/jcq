// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactHistoryService.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;
using Newtonsoft.Json;

namespace JCsTools.JCQ.Ux
{
    public class ContactHistoryService : ContextService, IContactHistoryService
    {
        private static readonly DirectoryInfo _StorageLocation =
            new DirectoryInfo(Path.Combine(App.DataStorageDirectoryPath, "history"));

        private readonly Dictionary<ContactViewModel, ContactHistory> _cache;

        public ContactHistoryService(IContext context)
            : base(context)
        {
            _cache = new Dictionary<ContactViewModel, ContactHistory>();
        }

        private static DirectoryInfo StorageLocation
        {
            get { return _StorageLocation; }
        }

        void IContactHistoryService.AppendMessage(ContactViewModel contact, MessageViewModel message)
        {
            ContactHistory history;

            if (!_cache.TryGetValue(contact, out history))
            {
                history = new ContactHistory(contact.Model);
                _cache.Add(contact, history);
            }
            else if (history == null)
            {
                history = new ContactHistory(contact.Model);
                _cache[contact] = history;
            }

            history.Messages.Add(message);
        }

        List<MessageViewModel> IContactHistoryService.GetHistory(ContactViewModel contact)
        {
            ContactHistory history;

            return _cache.TryGetValue(contact, out history) ? history.Messages : new List<MessageViewModel>();
        }

        List<MessageViewModel> IContactHistoryService.GetHistory(ContactViewModel contact, int maxItems)
        {
            ContactHistory history;

            if (!_cache.TryGetValue(contact, out history)) return new List<MessageViewModel>();

            var messages = history.Messages;

            return messages.Count > maxItems ? messages.Skip(messages.Count - maxItems).ToList() : messages;
        }

        void IContactHistoryService.Load()
        {
            Debug.WriteLine(string.Format("Loading history: {0}", StorageLocation.FullName), "Ux");

            if (!StorageLocation.Exists)
                return;

            foreach (var historyFile in StorageLocation.GetFiles("*.json"))
            {
                var identifier = Path.GetFileNameWithoutExtension(historyFile.Name);

                // IcqStorageService always returns an IcqContact for any identifier. (If not existing it is created)

                Func<string, ContactViewModel> getContact = id => ContactViewModelCache.GetViewModel(
                    ApplicationService.Current.Context.GetService<IStorageService>()
                        .GetContactByIdentifier(id));

                var contact = ContactViewModelCache.GetViewModel(
                    ApplicationService.Current.Context.GetService<IStorageService>()
                        .GetContactByIdentifier(identifier));

                using (var reader = new StreamReader(historyFile.FullName))
                {
                    var json = reader.ReadToEnd();

                    var messages = JsonConvert.DeserializeObject<TextMessageEntity[]>(json);

                    var history = new ContactHistory(contact.Model);

                    foreach (var message in messages)
                    {
                        var sender = getContact(message.SenderIdentifier);
                        var recipient = getContact(message.RecipientIdentifier);

                        var foreground = sender.Identifier == ApplicationService.Current.Context.Identity.Identifier
                            ? MessageColors.IdentityColor
                            : MessageColors.Contact1Color;

                        history.Messages.Add(new TextMessageViewModel(message.Date, sender, recipient, message.Text,
                            foreground));
                    }

                    _cache.Add(contact, history);
                }
            }
        }

        public void Save()
        {
            //XmlSerializer serializer;

            Debug.WriteLine(string.Format("Saving history: {0}", StorageLocation.FullName), "Ux");

            if (!StorageLocation.Exists)
                StorageLocation.Create();

            foreach (var pair in _cache)
            {
                var fiHistoryFile =
                    new FileInfo(Path.Combine(StorageLocation.FullName, string.Format("{0}.json", pair.Key.Identifier)));

                using (var writer = new StreamWriter(fiHistoryFile.FullName))
                {
                    var list = (from messageViewModel in pair.Value.Messages
                        let textMessage = messageViewModel as TextMessageViewModel
                        where textMessage != null
                        select new TextMessageEntity
                        {
                            Date = textMessage.DateCreated,
                            Text = textMessage.Message,
                            SenderIdentifier = textMessage.Sender.Identifier,
                            RecipientIdentifier = textMessage.Recipient.Identifier
                        }).ToList();

                    var json = JsonConvert.SerializeObject(list.ToArray());

                    writer.Write(json);
                }
            }
        }
    }

    public class TextMessageEntity
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string SenderIdentifier { get; set; }
        public string RecipientIdentifier { get; set; }
    }
}