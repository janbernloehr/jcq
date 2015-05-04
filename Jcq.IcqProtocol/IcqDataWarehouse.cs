// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqDataWarehouse.cs" company="Jan-Cornelius Molnar">
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
using System.IO;
using System.Text;
using System.Xml;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface.Interfaces;
using JCsTools.Xml.Formatter;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqDataWarehouse : ContextService, IDataWarehouseService
    {
        public IcqDataWarehouse(IcqContext context) : base(context)
        {
        }

        void IDataWarehouseService.Load(DirectoryInfo path)
        {
            FileInfo fiContactListData;
            FileInfo fiContactListInfo;
            XmlSerializer serializer;
            XmlTextReader dataReader;
            XmlTextReader infoReader;

            IStorageService svStorage;

            ContactListInfo info;

            fiContactListData = new FileInfo(Path.Combine(path.FullName, "contactlistdata.xml"));
            if (!fiContactListData.Exists)
                return;

            serializer = new XmlSerializer();
            serializer.RegisterReferenceFormatter(typeof (IcqContact),
                new BaseStorageItemFormatter(Context, serializer, typeof (IcqContact)));
            serializer.RegisterReferenceFormatter(typeof (KeyedNotifiyingCollection<string, IContact>),
                new ContactKeyedNotifiyingCollectionFormatter(serializer));
            serializer.RegisterReferenceFormatter(typeof (List<byte>), new ListOfByteFormatter(serializer));

            IEnumerable<IContact> items;

            using (var fs = fiContactListData.OpenRead())
            {
                dataReader = new XmlTextReader(fs);
                dataReader.WhitespaceHandling = WhitespaceHandling.None;

                items = (IEnumerable<IContact>) serializer.Deserialize(dataReader);
            }

            foreach (IcqContact x in items)
            {
                Context.GetService<IStorageService>().AttachContact(x, x.Group, false);
            }

            fiContactListInfo = new FileInfo(Path.Combine(path.FullName, "contactlistinfo.xml"));
            if (!fiContactListInfo.Exists)
                return;

            serializer = new XmlSerializer();
            serializer.RegisterReferenceFormatter(typeof (ContactListInfo), new ContactListInfoFormatter(serializer));

            using (var fs = fiContactListInfo.OpenRead())
            {
                infoReader = new XmlTextReader(fs);
                infoReader.WhitespaceHandling = WhitespaceHandling.None;

                info = (ContactListInfo) serializer.Deserialize(infoReader);
            }

            svStorage = Context.GetService<IStorageService>();
            svStorage.RegisterLocalContactList(info.ItemCount, info.DateChanged);
        }

        void IDataWarehouseService.Save(DirectoryInfo path)
        {
            FileInfo fiContactListData;
            FileInfo fiContactListInfo;
            XmlSerializer serializer;

            XmlTextWriter dataWriter;
            XmlTextWriter infoWriter;

            IStorageService svStorage;

            svStorage = Context.GetService<IStorageService>();

            fiContactListData = new FileInfo(Path.Combine(path.FullName, "contactlistdata.xml"));

            serializer = new XmlSerializer();
            serializer.RegisterReferenceFormatter(typeof (IcqContact),
                new BaseStorageItemFormatter(Context, serializer, typeof (IcqContact)));
            serializer.RegisterReferenceFormatter(typeof (List<byte>), new ListOfByteFormatter(serializer));

            using (var fs = new FileStream(fiContactListData.FullName, FileMode.Create, FileAccess.Write))
            {
                dataWriter = new XmlTextWriter(fs, Encoding.UTF8);

                serializer.Serialize(svStorage.Contacts, dataWriter);

                dataWriter.Flush();
            }

            if (svStorage.Info == null)
                return;

            fiContactListInfo = new FileInfo(Path.Combine(path.FullName, "contactlistinfo.xml"));

            serializer = new XmlSerializer();
            serializer.RegisterReferenceFormatter(typeof (ContactListInfo), new ContactListInfoFormatter(serializer));

            using (var fs = new FileStream(fiContactListInfo.FullName, FileMode.Create, FileAccess.Write))
            {
                infoWriter = new XmlTextWriter(fs, Encoding.UTF8);

                serializer.Serialize(svStorage.Info, infoWriter);

                infoWriter.Flush();
            }
        }

        void IDataWarehouseService.Clear(DirectoryInfo path)
        {
            FileInfo[] items;

            items = path.GetFiles("contactlist*.xml");

            foreach (var item in items)
            {
                item.Delete();
            }
        }
    }
}