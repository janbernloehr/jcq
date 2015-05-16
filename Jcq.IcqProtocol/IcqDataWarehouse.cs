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

using System.IO;
using System.Linq;
using System.Reflection;
using JCsTools.JCQ.IcqInterface.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JCsTools.JCQ.IcqInterface
{
    public class IcqDataWarehouse : ContextService, IDataWarehouseService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new SisoJsonDefaultContractResolver()
        };

        public IcqDataWarehouse(IcqContext context)
            : base(context)
        {
        }

        void IDataWarehouseService.Load(DirectoryInfo path)
        {
            ContactListInfo info;

            var fiContactListData = new FileInfo(Path.Combine(path.FullName, "contactlistdata.json"));
            if (!fiContactListData.Exists)
                return;

            IcqGroup[] groups;

            using (var sr = new StreamReader(fiContactListData.FullName))
            {
                var json = sr.ReadToEnd();

                groups = JsonConvert.DeserializeObject<IcqGroup[]>(json, _settings);
            }

            foreach (var icqGroup in groups)
            {
                foreach (var contact in icqGroup.Contacts)
                {
                    contact.SetGroup(icqGroup);

                    Context.GetService<IStorageService>().AttachContact(contact, icqGroup, false);
                }
            }

            var fiContactListInfo = new FileInfo(Path.Combine(path.FullName, "contactlistinfo.json"));
            if (!fiContactListInfo.Exists)
                return;

            using (var sr = new StreamReader(fiContactListInfo.FullName))
            {
                var json = sr.ReadToEnd();

                info = JsonConvert.DeserializeObject<ContactListInfo>(json);
            }

            var svStorage = Context.GetService<IStorageService>();
            svStorage.RegisterLocalContactList(info.ItemCount, info.DateChanged);
        }

        void IDataWarehouseService.Save(DirectoryInfo path)
        {
            var svStorage = Context.GetService<IStorageService>();

            var fiContactListData = new FileInfo(Path.Combine(path.FullName, "contactlistdata.json"));

            using (var sw = new StreamWriter(fiContactListData.FullName))
            {
                var groups = svStorage.Groups.Cast<IcqGroup>().ToArray();

                var json = JsonConvert.SerializeObject(groups, Formatting.Indented, _settings);

                sw.Write(json);
            }

            if (svStorage.Info == null)
                return;

            var fiContactListInfo = new FileInfo(Path.Combine(path.FullName, "contactlistinfo.json"));

            using (var sw = new StreamWriter(fiContactListInfo.FullName))
            {
                var json = JsonConvert.SerializeObject(svStorage.Info);

                sw.Write(json);
            }
        }

        void IDataWarehouseService.Clear(DirectoryInfo path)
        {
            var items = path.GetFiles("contactlist*.json");

            foreach (var item in items)
            {
                item.Delete();
            }
        }
    }

    public class SisoJsonDefaultContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            //TODO: Maybe cache
            var prop = base.CreateProperty(member, memberSerialization);

            if (prop.Writable) return prop;

            var property = member as PropertyInfo;
            if (property == null) return prop;

            var hasPrivateSetter = property.GetSetMethod(true) != null;
            prop.Writable = hasPrivateSetter;

            return prop;
        }
    }
}