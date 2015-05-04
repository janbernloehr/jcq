// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactHistoryService.cs" company="Jan-Cornelius Molnar">
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.JCQ.ViewModel;
using JCsTools.Xml.Formatter;

namespace JCsTools.JCQ.Ux
{
    public class ContactHistoryService : ContextService, IContactHistoryService
    {
        private static readonly DirectoryInfo _StorageLocation =
            new DirectoryInfo(Path.Combine(App.DataStorageDirectoryPath, "history"));

        private readonly Dictionary<ContactViewModel, ContactHistory> _Cache;

        public ContactHistoryService(IContext context) : base(context)
        {
            _Cache = new Dictionary<ContactViewModel, ContactHistory>();
        }

        private static DirectoryInfo StorageLocation
        {
            get { return _StorageLocation; }
        }

        void IContactHistoryService.AppendMessage(ContactViewModel contact, MessageViewModel message)
        {
            ContactHistory history = null;

            if (!_Cache.TryGetValue(contact, out history))
            {
                history = new ContactHistory(contact.Model);
                _Cache.Add(contact, history);
            }

            history.Messages.Add(message);
        }

        List<MessageViewModel> IContactHistoryService.GetHistory(ContactViewModel contact)
        {
            ContactHistory history = null;

            if (_Cache.TryGetValue(contact, out history))
            {
                return history.Messages;
            }
            return new List<MessageViewModel>();
        }

        List<MessageViewModel> IContactHistoryService.GetHistory(ContactViewModel contact, int maxItems)
        {
            ContactHistory history = null;
            List<MessageViewModel> messages;

            if (_Cache.TryGetValue(contact, out history))
            {
                messages = history.Messages;

                if (messages.Count > maxItems)
                {
                    return messages.Skip(messages.Count - maxItems).ToList();
                }
                return messages;
            }
            return new List<MessageViewModel>();
        }

        void IContactHistoryService.Load()
        {
            XmlSerializer serializer;

            Debug.WriteLine(string.Format("Loading history: {0}", StorageLocation.FullName), "Ux");

            if (!StorageLocation.Exists)
                return;

            serializer = new XmlSerializer();
            serializer.RegisterReferenceFormatter(typeof (ContactHistory), new ContactHistoryFormatter(serializer));
            serializer.RegisterReferenceFormatter(typeof (TextMessageViewModel),
                new TextMessageViewModelFormatter(serializer));
            serializer.RegisterReferenceFormatter(typeof (OfflineTextMessageViewModel),
                new OfflineTextMessageViewModelFormatter(serializer));

            foreach (var historyFile in StorageLocation.GetFiles("*.xml"))
            {
                string identifier;
                ContactViewModel contact;

                identifier = historyFile.Name.Substring(0, historyFile.Name.IndexOf("."));
                contact =
                    ContactViewModelCache.GetViewModel(
                        ApplicationService.Current.Context.GetService<IStorageService>()
                            .GetContactByIdentifier(identifier));

                using (var reader = new XmlTextReader(historyFile.FullName))
                {
                    ContactHistory history;

                    history = (ContactHistory) serializer.Deserialize(reader);

                    _Cache.Add(contact, history);
                }
            }
        }

        void IContactHistoryService.Save()
        {
            FileInfo fiHistoryFile;
            XmlSerializer serializer;

            Debug.WriteLine(string.Format("Saving history: {0}", StorageLocation.FullName), "Ux");

            if (!StorageLocation.Exists)
                StorageLocation.Create();

            serializer = new XmlSerializer();
            serializer.RegisterReferenceFormatter(typeof (ContactHistory), new ContactHistoryFormatter(serializer));
            serializer.RegisterReferenceFormatter(typeof (TextMessageViewModel),
                new TextMessageViewModelFormatter(serializer));
            serializer.RegisterReferenceFormatter(typeof (OfflineTextMessageViewModel),
                new OfflineTextMessageViewModelFormatter(serializer));

            foreach (var pair in _Cache)
            {
                fiHistoryFile =
                    new FileInfo(Path.Combine(StorageLocation.FullName, string.Format("{0}.xml", pair.Key.Identifier)));

                using (var writer = new XmlTextWriter(fiHistoryFile.FullName, Encoding.UTF8))
                {
                    serializer.Serialize(pair.Value, writer);
                }
            }
        }
    }
}