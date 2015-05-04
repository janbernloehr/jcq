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
using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;
namespace JCsTools.Xml.Formatter
{
  public class DefaultIListReferenceFormatter : DefaultReferenceFormatter
  {
    public DefaultIListReferenceFormatter(ISerializer context, Type listType) : base(context, listType, false, false)
    {
    }

    public override void Serialize(object graph, System.Xml.XmlWriter writer)
    {
      writer.WriteStartElement("list");

      WriteIdAttribute(graph, writer);
      WriteTypeAttribute(graph, writer);

      SerializeItems((IList)graph, writer);

      writer.WriteEndElement();
    }

    public override object Deserialize(System.Xml.XmlReader reader)
    {
      int objectId;
      Type type;
      IList obj;

      reader.MoveToFirstAttribute();
      objectId = GetObjectId(reader);

      reader.MoveToNextAttribute();
      type = GetObjectType(reader);

      obj = (IList)CreateObject(type, reader);

      if (!reader.IsEmptyElement) {
        DeserializeItems(obj, reader);
      }

      Serializer.RegisterObject(objectId, obj);

      return obj;
    }

    protected virtual void DeserializeItems(IList list, XmlReader reader)
    {
      if (reader.IsEmptyElement)
        return;

      Type type;
      string value;

      while (reader.Read() && reader.NodeType == XmlNodeType.Element) {
        reader.MoveToFirstAttribute();
        type = GetObjectType(reader);

        reader.MoveToNextAttribute();
        value = GetObjectValue(reader);

        if (type.IsValueType) {
          // Value Type
          IValueFormatter formatter = Serializer.GetValueFormatter(type);

          list.Add(formatter.Deserialize(value));
        } else if (object.ReferenceEquals(type, typeof(string))) {
          // String
          list.Add(value);
        } else {
          // Reference Type
          ListItemFixUp @ref = new ListItemFixUp(int.Parse(value), list);

          Serializer.RegisterListItemFixUp(@ref);
        }
      }
    }

    protected virtual void SerializeItems(IList list, XmlWriter writer)
    {
      foreach (object x in list) {
        Type itemType = x.GetType();
        string itemValue;

        writer.WriteStartElement("item");
        WriteTypeAttribute(x, x.GetType(), writer);

        if (itemType.IsValueType) {
          IValueFormatter formatter = Serializer.GetValueFormatter(itemType);
          itemValue = formatter.Serialize(x);
        } else if (object.ReferenceEquals(itemType, typeof(string))) {
          itemValue = (string)x;
        } else {
          itemValue = Serializer.GetSerializeObjectId(x).ToString();
        }

        WriteValueAttribute(x, itemValue, writer);

        writer.WriteEndElement();
      }
    }
  }
}

