// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqDataWarehouse.cs" company="Jan-Cornelius Molnar">
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