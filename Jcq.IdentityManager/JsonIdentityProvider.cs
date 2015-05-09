using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JCsTools.IdentityManager
{
    public class JsonIdentityProvider : IdentityProvider
    {
        private readonly FileInfo _jsonFile;

        public JsonIdentityProvider(FileInfo jsonFile)
        {
            _jsonFile = jsonFile;
        }

        public void Load()
        {
            // if no file exists, exit :)
            if (!_jsonFile.Exists)
                return;

            string json;

            using (var sr = new StreamReader(_jsonFile.FullName))
            {
                json = sr.ReadToEnd();
            }

            var identities = JsonConvert.DeserializeObject<Identity[]>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });

            foreach (var identity in identities)
            {
                _Identities.Add(identity);
            }


            //var json = JsonConvert.SerializeObject(_Identities);

            //if (!_jsonFile.Directory.Exists)
            //    _jsonFile.Directory.Create();

            //using (var sw = new StreamWriter(_jsonFile.FullName))
            //{
            //    sw.Write(json);
            //}

            //// if no file exists, exit :)
            //if (!_jsonFile.Exists)
            //    return;

            //var ds = default(DataSet);

            //// create the dataset schema
            //ds = CreateDataSet();

            //// read the data
            //ds.ReadXml(_jsonFile.FullName);

            //// query identities
            //var qTypedIdentities = from x in ds.Tables["Identities"].AsEnumerable()
            //                       select new
            //                       {
            //                           Id = x.Field<string>("ID"),
            //                           Identifier = x.Field<string>("Identifier"),
            //                           Description = x.Field<string>("Description"),
            //                           ImageUrl = x.Field<string>("ImageUrl")
            //                       };

            //// query attributes
            //var qTypedAttributes = from x in ds.Tables["Attributes"].AsEnumerable()
            //                       select new
            //                       {
            //                           IdentityId = x.Field<string>("IdentityId"),
            //                           Key = x.Field<string>("Key"),
            //                           Value = x.Field<object>("Value")
            //                       };

            //var qGroupedAttributes = from x in qTypedAttributes group x by x.IdentityId into g select g;


            //// add attributes to identities
            //foreach (var attributeGroup in qGroupedAttributes)
            //{
            //    var ag = attributeGroup;

            //    var data = (from x in qTypedIdentities
            //                where x.Id == ag.Key
            //                select x).
            //        FirstOrDefault();

            //    var identity = new Identity(data.Identifier) { Description = data.Description, ImageUrl = data.ImageUrl };

            //    foreach (var a in attributeGroup)
            //    {
            //        identity.SetAttribute(a.Key, a.Value);
            //    }

            //    _Identities.Add(identity);
            //}
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_Identities.ToArray(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });

            if (!_jsonFile.Directory.Exists)
                _jsonFile.Directory.Create();

            using (var sw = new StreamWriter(_jsonFile.FullName))
            {
                sw.Write(json);
            }

            //var ds = default(DataSet);

            //if (!_jsonFile.Directory.Exists)
            //    _jsonFile.Directory.Create();

            //// create the dataset schema
            //ds = CreateDataSet();

            //foreach (var id in _Identities)
            //{
            //    var row = ds.Tables["Identities"].NewRow();

            //    row["Id"] = Guid.NewGuid().ToString();
            //    row["Identifier"] = id.Identifier;
            //    row["ImageUrl"] = id.ImageUrl;
            //    row["Description"] = id.Description;

            //    ds.Tables["Identities"].Rows.Add(row);

            //    foreach (var attributeName in id.GetAttributeNames())
            //    {
            //        var ra = ds.Tables["Attributes"].NewRow();

            //        ra["Id"] = Guid.NewGuid().ToString();
            //        ra["IdentityId"] = row["Id"];
            //        ra["Key"] = attributeName;
            //        ra["Value"] = id.GetAttribute(attributeName);

            //        ds.Tables["Attributes"].Rows.Add(ra);
            //    }
            //}

            //ds.WriteXml(_jsonFile.FullName);
        }
    }
}
