//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;
namespace JCsTools.JCQ.IcqInterface
{
  public class IcqDataWarehouse : ContextService, Interfaces.IDataWarehouseService
  {
    public IcqDataWarehouse(IcqContext context) : base(context)
    {
    }

    public void Interfaces.IDataWarehouseService.Load(System.IO.DirectoryInfo path)
    {
      System.IO.FileInfo fiContactListData;
      System.IO.FileInfo fiContactListInfo;
      JCsTools.Xml.Formatter.XmlSerializer serializer;
      XmlTextReader dataReader;
      XmlTextReader infoReader;

      Interfaces.IStorageService svStorage;

      ContactListInfo info;

      fiContactListData = new System.IO.FileInfo(System.IO.Path.Combine(path.FullName, "contactlistdata.xml"));
      if (!fiContactListData.Exists)
        return;

      serializer = new JCsTools.Xml.Formatter.XmlSerializer();
      serializer.RegisterReferenceFormatter(typeof(IcqContact), new BaseStorageItemFormatter(Context, serializer, typeof(IcqContact)));
      serializer.RegisterReferenceFormatter(typeof(Core.KeyedNotifiyingCollection<string, Interfaces.IContact>), new ContactKeyedNotifiyingCollectionFormatter(serializer));
      serializer.RegisterReferenceFormatter(typeof(List<byte>), new JCsTools.Xml.Formatter.ListOfByteFormatter(serializer));

      IEnumerable<Interfaces.IContact> items;

      using (fs == fiContactListData.OpenRead) {
        dataReader = new System.Xml.XmlTextReader(fs);
        dataReader.WhitespaceHandling = WhitespaceHandling.None;

        items = (IEnumerable<Interfaces.IContact>)serializer.Deserialize(dataReader);
      }

      foreach (IcqContact x in items) {
        Context.GetService<Interfaces.IStorageService>.AttachContact(x, x.Group, false);
      }

      fiContactListInfo = new System.IO.FileInfo(System.IO.Path.Combine(path.FullName, "contactlistinfo.xml"));
      if (!fiContactListInfo.Exists)
        return;

      serializer = new JCsTools.Xml.Formatter.XmlSerializer();
      serializer.RegisterReferenceFormatter(typeof(ContactListInfo), new ContactListInfoFormatter(serializer));

      using (fs == fiContactListInfo.OpenRead) {
        infoReader = new System.Xml.XmlTextReader(fs);
        infoReader.WhitespaceHandling = WhitespaceHandling.None;

        info = (ContactListInfo)serializer.Deserialize(infoReader);
      }

      svStorage = Context.GetService<Interfaces.IStorageService>();
      svStorage.RegisterLocalContactList(info.ItemCount, info.DateChanged);
    }

    public void Interfaces.IDataWarehouseService.Save(System.IO.DirectoryInfo path)
    {
      System.IO.FileInfo fiContactListData;
      System.IO.FileInfo fiContactListInfo;
      JCsTools.Xml.Formatter.XmlSerializer serializer;

      XmlTextWriter dataWriter;
      XmlTextWriter infoWriter;

      Interfaces.IStorageService svStorage;

      svStorage = Context.GetService<Interfaces.IStorageService>();

      fiContactListData = new System.IO.FileInfo(System.IO.Path.Combine(path.FullName, "contactlistdata.xml"));

      serializer = new JCsTools.Xml.Formatter.XmlSerializer();
      serializer.RegisterReferenceFormatter(typeof(IcqContact), new BaseStorageItemFormatter(Context, serializer, typeof(IcqContact)));
      serializer.RegisterReferenceFormatter(typeof(List<byte>), new JCsTools.Xml.Formatter.ListOfByteFormatter(serializer));

      using (fs == new System.IO.FileStream(fiContactListData.FullName, IO.FileMode.Create, IO.FileAccess.Write)) {
        dataWriter = new System.Xml.XmlTextWriter(fs, System.Text.Encoding.UTF8);

        serializer.Serialize(svStorage.Contacts, dataWriter);

        dataWriter.Flush();
      }

      if (svStorage.Info == null)
        return;

      fiContactListInfo = new System.IO.FileInfo(System.IO.Path.Combine(path.FullName, "contactlistinfo.xml"));

      serializer = new JCsTools.Xml.Formatter.XmlSerializer();
      serializer.RegisterReferenceFormatter(typeof(ContactListInfo), new ContactListInfoFormatter(serializer));

      using (fs == new System.IO.FileStream(fiContactListInfo.FullName, IO.FileMode.Create, IO.FileAccess.Write)) {
        infoWriter = new System.Xml.XmlTextWriter(fs, System.Text.Encoding.UTF8);

        serializer.Serialize(svStorage.Info, infoWriter);

        infoWriter.Flush();
      }
    }

    public void Interfaces.IDataWarehouseService.Clear(System.IO.DirectoryInfo path)
    {
      IO.FileInfo[] items;

      items = path.GetFiles("contactlist*.xml");

      foreach (IO.FileInfo item in items) {
        item.Delete();
      }
    }
  }

  public class ContactKeyedNotifiyingCollectionFormatter : JCsTools.Xml.Formatter.DefaultIListReferenceFormatter
  {
    private readonly Core.NotifyingCollection<Interfaces.IContact> _Contacts;
    private readonly Core.NotifyingCollection<Interfaces.IGroup> _Groups;

    public ContactKeyedNotifiyingCollectionFormatter(JCsTools.Xml.Formatter.XmlSerializer parent) : base(parent, typeof(ContactKeyedNotifiyingCollectionFormatter))
    {

      _Contacts = new Core.NotifyingCollection<Interfaces.IContact>();
      _Groups = new Core.NotifyingCollection<Interfaces.IGroup>();
    }

    protected override object CreateObject(System.Type type, XmlReader reader)
    {
      object coll = new Core.KeyedNotifiyingCollection<string, Interfaces.IContact>(c => c.Identifier);

      return coll;
    }
  }

  public class ContactListInfoFormatter : JCsTools.Xml.Formatter.DefaultReferenceFormatter
  {
    public ContactListInfoFormatter(JCsTools.Xml.Formatter.XmlSerializer parent) : base(parent, typeof(ContactListInfo))
    {
    }

    protected override object CreateObject(System.Type type, XmlReader reader)
    {
      string itemCountValue;
      string dateChangedValue;

      int itemCount;
      System.DateTime dateChanged;

      itemCountValue = reader.GetAttribute("itemCount");
      dateChangedValue = reader.GetAttribute("dateChanged");

      itemCount = XmlConvert.ToInt32(itemCountValue);
      dateChanged = XmlConvert.ToDateTime(dateChangedValue, XmlDateTimeSerializationMode.Utc);

      return new ContactListInfo(itemCount, dateChanged);
    }

    protected override void SerializeProperties(object graph, System.Xml.XmlWriter writer)
    {
      ContactListInfo info;

      info = (ContactListInfo)graph;

      writer.WriteAttributeString("itemCount", XmlConvert.ToString(info.ItemCount));
      writer.WriteAttributeString("dateChanged", XmlConvert.ToString(info.DateChanged, XmlDateTimeSerializationMode.Utc));
    }

    protected override void DeserializeProperties(object graph, System.Xml.XmlReader reader)
    {
      // We need to override this method since the base implementation does deserialize
      // the properties and tries to assign them to the graph.
      // Since this object has only ReadOnly properties which already were set by the
      // CreateObject method, there is nothing to do here.
    }
  }

  public enum FormatTarget
  {
    Attribute,
    Element
  }
}

