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
using JCsTools.Xml.Formatter;
using System.Xml;
namespace JCsTools.JCQ.IcqInterface
{
  public class BaseStorageItemFormatter : DefaultReferenceFormatter
  {
    private readonly Interfaces.IContext _Context;

    private readonly List<string> ignoredKeys = new List<string>(new string[] { "Status" });

    public BaseStorageItemFormatter(Interfaces.IContext context, JCsTools.Xml.Formatter.XmlSerializer parent, Type type) : base(parent, type)
    {
      _Context = context;
    }

    protected override void SerializeProperties(object graph, System.Xml.XmlWriter writer)
    {
      BaseStorageItem item;
      Hashtable attributes;

      item = (BaseStorageItem)graph;

      writer.WriteAttributeString("name", item.Name);
      writer.WriteAttributeString("identifier", item.Identifier);

      writer.WriteStartElement("Attributes");

      attributes = item.Attributes;

      foreach (string key in attributes.Keys) {
        if (ignoredKeys.Contains(key))
          continue;

        object value = attributes(key);
        if (value == null)
          continue;

        Type type = value.GetType;

        writer.WriteStartElement(key);

        WriteTypeAttribute(value, type, writer);

        writer.WriteAttributeString("key", key);

        if (type.IsValueType) {
          IValueFormatter formatter = Serializer.GetValueFormatter(type);

          writer.WriteAttributeString("value", formatter.Serialize(value));
        } else if (object.ReferenceEquals(type, typeof(string))) {
          writer.WriteAttributeString("value", (string)value);
        } else {
          writer.WriteAttributeString("value", Serializer.GetSerializeObjectId(value).ToString);
        }

        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    //Protected Overrides Sub SerializeProperties(ByVal graph As Object, ByVal element As System.Xml.XmlElement)
    //    ' name, identity, attributes

    //    Dim item As BaseStorageItem
    //    Dim document As XmlDocument = element.OwnerDocument

    //    item = DirectCast(graph, BaseStorageItem)

    //    Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(graph)

    //    Dim propertyAttribute As XmlAttribute

    //    Dim nameProperty As PropertyDescriptor = properties("Name")
    //    Dim identifierProperty As PropertyDescriptor = properties("Identifier")
    //    Dim attributesProperty As PropertyDescriptor = properties("Attributes")

    //    propertyAttribute = document.CreateAttribute("name")
    //    propertyAttribute.Value = CStr(nameProperty.GetValue(graph))
    //    element.Attributes.Append(propertyAttribute)

    //    propertyAttribute = document.CreateAttribute("identifier")
    //    propertyAttribute.Value = CStr(identifierProperty.GetValue(graph))
    //    element.Attributes.Append(propertyAttribute)

    //    Dim attributes As Hashtable = DirectCast(attributesProperty.GetValue(graph), Hashtable)

    //    Dim attributesElement As XmlElement = document.CreateElement("Attributes")

    //    For Each key As String In attributes.Keys
    //        If ignoredKeys.Contains(key) Then Continue For

    //        Dim attributeElement As XmlElement

    //        Dim value As Object = attributes(key)
    //        If value Is Nothing Then Continue For

    //        Dim type As Type = value.GetType

    //        attributeElement = document.CreateElement(key)

    //        Me.AppendTypeAttribute(value, attributeElement)

    //        If type.IsValueType Or type Is GetType(String) Then
    //            Dim f As IValueFormatter = Formatter.GetValueFormatter(type)

    //            Dim keyAttribute As XmlAttribute = document.CreateAttribute("key")
    //            keyAttribute.Value = key
    //            attributeElement.Attributes.Append(keyAttribute)

    //            Dim valueAttribute As XmlAttribute = document.CreateAttribute("value")
    //            valueAttribute.Value = f.Serialize(value)
    //            attributeElement.Attributes.Append(valueAttribute)

    //            'AppendValueAttribute(attributeElement, f.Serialize(value))
    //        Else
    //            Dim keyAttribute As XmlAttribute = document.CreateAttribute("key")
    //            keyAttribute.Value = key
    //            attributeElement.Attributes.Append(keyAttribute)

    //            Dim valueAttribute As XmlAttribute = document.CreateAttribute("value")
    //            valueAttribute.Value = CStr(Formatter.GetObjectId(value))
    //            attributeElement.Attributes.Append(valueAttribute)

    //            'AppendValueAttribute(attributeElement, CStr(_Context.GetObjectId(value)))
    //        End If

    //        attributesElement.AppendChild(attributeElement)
    //    Next

    //    element.AppendChild(attributesElement)
    //End Sub

    protected override object CreateObject(System.Type type, XmlReader reader)
    {
      string identifier = reader.GetAttribute("identifier");

      return _Context.GetService<Interfaces.IStorageService>.GetContactByIdentifier(identifier);
    }

    protected override void DeserializeProperties(object graph, XmlReader reader)
    {
      IcqContact contact = (IcqContact)graph;

      contact.Identifier = reader.GetAttribute("identifier");
      contact.Name = reader.GetAttribute("name");

      if (reader.IsEmptyElement)
        return;

      Type type;

      while (reader.Read && reader.NodeType == XmlNodeType.Element) {
        reader.MoveToFirstAttribute();
        type = GetObjectType(reader);

        if (type == null)
          continue;

        string key;
        string value;

        reader.MoveToNextAttribute();
        key = reader.Value;

        reader.MoveToNextAttribute();
        value = reader.Value;

        if (type.IsValueType) {
          IValueFormatter formatter = Serializer.GetValueFormatter(type);

          contact.Attributes(key) = formatter.Deserialize(value);
        } else if (object.ReferenceEquals(type, typeof(string))) {
          contact.Attributes(key) = value;
        } else {
          object fixup = new HashtableItemFixUp(key, int.Parse(value), contact.Attributes);

          Serializer.RegisterCustomFixUp(fixup);
        }
      }
    }
  }

  public class HashtableItemFixUp : IFixUp
  {
    private readonly Hashtable _Hashtable;
    private readonly int _ItemId;
    private readonly string _Key;

    public HashtableItemFixUp(string key, int itemId, Hashtable hashtable)
    {
      _Key = Key;
      _ItemId = itemId;
      _Hashtable = hashtable;
    }

    public Hashtable Hashtable {
      get { return _Hashtable; }
    }

    public string Key {
      get { return _Key; }
    }

    public int ItemId {
      get { return _ItemId; }
    }

    public void Xml.Formatter.IFixUp.Execute(Xml.Formatter.ISerializer serializer)
    {
      object item = serializer.GetDeserializeObjectById(ItemId);

      _Hashtable.Add(Key, item);
    }
  }
}

