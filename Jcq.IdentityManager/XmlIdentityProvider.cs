// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlIdentityProvider.cs" company="Jan-Cornelius Molnar">
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

using System;
using System.Data;
using System.IO;
using System.Linq;

namespace JCsTools.IdentityManager
{
    //public class XmlIdentityProvider : IdentityProvider
    //{
    //    private readonly FileInfo _xmlFile;

    //    public XmlIdentityProvider(FileInfo xmlFile)
    //    {
    //        _xmlFile = xmlFile;
    //    }

    //    private DataSet CreateDataSet()
    //    {
    //        var ds = default(DataSet);
    //        var dtIdentities = default(DataTable);
    //        var dtAttributes = default(DataTable);

    //        ds = new DataSet("IdentityData");
    //        dtIdentities = ds.Tables.Add("Identities");
    //        dtIdentities.Columns.Add("Id", typeof (string));
    //        dtIdentities.Columns.Add("Identifier", typeof (string));
    //        dtIdentities.Columns.Add("ImageUrl", typeof (string));
    //        dtIdentities.Columns.Add("Description", typeof (string));

    //        dtAttributes = ds.Tables.Add("Attributes");
    //        dtAttributes.Columns.Add("Id", typeof (string));
    //        dtAttributes.Columns.Add("IdentityId", typeof (string));
    //        dtAttributes.Columns.Add("Key", typeof (string));
    //        dtAttributes.Columns.Add("Value", typeof (object));

    //        return ds;
    //    }

    //    public void Load()
    //    {
    //        // if no file exists, exit :)
    //        if (!_xmlFile.Exists)
    //            return;

    //        var ds = default(DataSet);

    //        // create the dataset schema
    //        ds = CreateDataSet();

    //        // read the data
    //        ds.ReadXml(_xmlFile.FullName);

    //        // query identities
    //        var qTypedIdentities = from x in ds.Tables["Identities"].AsEnumerable()
    //            select new
    //            {
    //                Id = x.Field<string>("ID"),
    //                Identifier = x.Field<string>("Identifier"),
    //                Description = x.Field<string>("Description"),
    //                ImageUrl = x.Field<string>("ImageUrl")
    //            };

    //        // query attributes
    //        var qTypedAttributes = from x in ds.Tables["Attributes"].AsEnumerable()
    //            select new
    //            {
    //                IdentityId = x.Field<string>("IdentityId"),
    //                Key = x.Field<string>("Key"),
    //                Value = x.Field<object>("Value")
    //            };

    //        var qGroupedAttributes = from x in qTypedAttributes group x by x.IdentityId into g select g;


    //        // add attributes to identities
    //        foreach (var attributeGroup in qGroupedAttributes)
    //        {
    //            var ag = attributeGroup;

    //            var data = (from x in qTypedIdentities
    //                where x.Id == ag.Key
    //                select x).
    //                FirstOrDefault();

    //            var identity = new Identity(data.Identifier) {Description = data.Description, ImageUrl = data.ImageUrl};

    //            foreach (var a in attributeGroup)
    //            {
    //                //identity.SetAttribute(a.Key, a.Value);
    //            }

    //            _Identities.Add(identity);
    //        }
    //    }

    //    public void Save()
    //    {
    //        var ds = default(DataSet);

    //        if (!_xmlFile.Directory.Exists)
    //            _xmlFile.Directory.Create();

    //        // create the dataset schema
    //        ds = CreateDataSet();

    //        foreach (var id in _Identities)
    //        {
    //            var row = ds.Tables["Identities"].NewRow();

    //            row["Id"] = Guid.NewGuid().ToString();
    //            row["Identifier"] = id.Identifier;
    //            row["ImageUrl"] = id.ImageUrl;
    //            row["Description"] = id.Description;

    //            ds.Tables["Identities"].Rows.Add(row);

    //            //foreach (var attributeName in id.GetAttributeNames())
    //            //{
    //            //    var ra = ds.Tables["Attributes"].NewRow();

    //            //    ra["Id"] = Guid.NewGuid().ToString();
    //            //    ra["IdentityId"] = row["Id"];
    //            //    ra["Key"] = attributeName;
    //            //    ra["Value"] = id.GetAttribute(attributeName);

    //            //    ds.Tables["Attributes"].Rows.Add(ra);
    //            //}
    //        }

    //        ds.WriteXml(_xmlFile.FullName);
    //    }
    //}
}