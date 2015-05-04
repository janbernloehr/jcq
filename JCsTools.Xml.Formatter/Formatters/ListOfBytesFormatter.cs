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

using System.Diagnostics;


using System.Linq;
namespace JCsTools.Xml.Formatter
{
  public class ListOfByteFormatter : JCsTools.Xml.Formatter.DefaultReferenceFormatter
  {
    public ListOfByteFormatter(JCsTools.Xml.Formatter.ISerializer parent) : base(parent, typeof(List<byte>))
    {
    }

    protected override object CreateObject(System.Type type, System.Xml.XmlReader reader)
    {
      return new List<byte>();
    }

    public override object Deserialize(System.Xml.XmlReader reader)
    {
      int objectId;
      Type type;
      object obj;
      List<byte> list;

      reader.MoveToFirstAttribute();
      objectId = GetObjectId(reader);

      reader.MoveToNextAttribute();
      type = GetObjectType(reader);

      obj = CreateObject(type, reader);
      list = (List<byte>)obj;

      reader.Read();

      byte[] buffer;
      int bufferSize = 1024;
      int bytesRead;

      do {
        buffer = new byte[bufferSize - 1] {
          
        };
        bytesRead = reader.ReadElementContentAsBase64(buffer, 0, bufferSize);
        if (bytesRead == 0)
          break; // TODO: might not be correct. Was : Exit Do

        list.AddRange(buffer.Take(bytesRead));
      } while (true);

      // The last call of ReadElementContentAsBase64 moves the reader to </listofbyte>
      // which is already the end of the current element. no further movement is required.

      Serializer.RegisterObject(objectId, obj);

      return obj;
    }

    public override void Serialize(object graph, System.Xml.XmlWriter writer)
    {
      List<byte> list = (List<byte>)graph;

      writer.WriteStartElement("listofbyte");

      WriteIdAttribute(graph, writer);
      WriteTypeAttribute(graph, writer);

      writer.WriteStartElement("data");
      writer.WriteBase64(list.ToArray, 0, list.Count);
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

  }
}

