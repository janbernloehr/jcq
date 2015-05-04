// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlFormatter.cs" company="Jan-Cornelius Molnar">
//  Copyright (c) 2015-2015 Jan-Cornelius Molnar
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace JCsTools.Xml.Formatter
{
    public class XmlSerializer : ISerializer
    {
        private readonly Dictionary<Type, IReferenceFormatter> _ReferenceFormatters =
            new Dictionary<Type, IReferenceFormatter>();

        private readonly Dictionary<Type, IValueFormatter> _ValueFormatters = new Dictionary<Type, IValueFormatter>();
        private List<IFixUp> _CustomFixUps;
        private Dictionary<int, object> _DeserializeObjectIdDictionary;
        private List<ListItemFixUp> _ListItemFixUps;
        private List<object> _ObjectsSerialized;
        private Queue<object> _ObjectsToSerialize;
        private List<PropertyValueFixUp> _PropertyValueFixUps;
        private List<ReadOnlyListPropertyFixUp> _ReadOnlyListPropertyFixUps;
        private int _SerializeObjectId;
        private Dictionary<object, int> _SerializeObjectIdDictionary;

        int ISerializer.GetSerializeObjectId(object graph)
        {
            object objectId;

            if (!_SerializeObjectIdDictionary.TryGetValue(graph, out objectId))
            {
                _ObjectsToSerialize.Enqueue(graph);

                objectId = Interlocked.Increment(_SerializeObjectId);

                _SerializeObjectIdDictionary.Add(graph, objectId);
            }

            return objectId;
        }

        //Public Function Serialize(ByVal graph As Object) As System.Xml.XmlDocument Implements ISerializer.Serialize
        //    Dim type As Type = graph.GetType

        //    _document = New XmlDocument
        //    _pendingObjects = New Queue(Of Object)
        //    _doneObjects = New List(Of Object)
        //    _ids = New Dictionary(Of Object, Integer)
        //    _idIterator = 1

        //    Dim root As XmlElement = _document.CreateElement("root")
        //    Dim rootId As Integer = GetObjectId(graph)

        //    Dim rootIdAttribute As XmlAttribute = _document.CreateAttribute("__rootId")
        //    rootIdAttribute.Value = CStr(rootId)
        //    root.Attributes.Append(rootIdAttribute)

        //    Do Until _pendingObjects.Count = 0
        //        Dim obj As Object = _pendingObjects.Dequeue
        //        _doneObjects.Add(obj)

        //        Dim f As IReferenceFormatter = Me.GetReferenceFormatter(obj.GetType)

        //        Dim element As XmlElement

        //        element = f.Serialize(obj)

        //        root.AppendChild(element)
        //    Loop

        //    _document.AppendChild(root)

        //    Return _document
        //End Function

        object ISerializer.GetDeserializeObjectById(int id)
        {
            return _DeserializeObjectIdDictionary(id);
        }

        void ISerializer.RegisterValueFormatter(Type type, IValueFormatter f)
        {
            _ValueFormatters.Add(type, f);
        }

        void ISerializer.RegisterReferenceFormatter(Type type, IReferenceFormatter f)
        {
            _ReferenceFormatters.Add(type, f);
        }

        IValueFormatter ISerializer.GetValueFormatter(Type type)
        {
            IValueFormatter formatter = null;

            if (!_ValueFormatters.TryGetValue(type, formatter))
            {
                formatter = new XmlValueFormatter(this, type);
                _ValueFormatters.Add(type, formatter);
            }

            return formatter;
        }

        IReferenceFormatter ISerializer.GetReferenceFormatter(Type type)
        {
            IReferenceFormatter formatter = null;

            if (!_ReferenceFormatters.TryGetValue(type, formatter))
            {
                if (type.GetInterface(typeof (IList).ToString) != null)
                {
                    formatter = new DefaultIListReferenceFormatter(this, type);
                    _ReferenceFormatters.Add(type, formatter);
                }
                else
                {
                    formatter = new DefaultReferenceFormatter(this, type);
                    _ReferenceFormatters.Add(type, formatter);
                }
            }

            return formatter;
        }

        //Public Function Deserialize(ByVal doc As XmlDocument) As Object Implements ISerializer.Deserialize
        //    _objects = New Dictionary(Of Integer, Object)
        //    _valueReferences = New List(Of ValueReference)
        //    _itemReferences = New List(Of ItemReference)

        //    _ReadOnlyIListReferences = New List(Of ReadOnlyIListReference)

        //    _FixupCallbacks = New List(Of Action(Of ISerializer))

        //    Dim root As XmlElement = DirectCast(doc.ChildNodes(0), XmlElement)
        //    Dim rootId As Integer = CInt(root.Attributes("__rootId").Value)

        //    For Each element As XmlElement In root.ChildNodes
        //        Dim typeName As String = element.Attributes("__type").Value
        //        Dim type As Type = type.GetType(typeName)

        //        Dim f As IReferenceFormatter = Me.GetReferenceFormatter(type)

        //        f.Deserialize(element)
        //    Next

        //    For Each ref As ValueReference In _valueReferences
        //        Dim value As Object = _objects(ref.Value)

        //        ref.PropertyDescriptor.SetValue(ref.Graph, value)
        //    Next

        //    For Each ref As ItemReference In _itemReferences
        //        Dim value As Object = _objects(ref.Value)

        //        ref.Graph.Add(value)
        //    Next

        //    For Each ref As ReadOnlyIListReference In _ReadOnlyIListReferences
        //        Dim value As IList = DirectCast(_objects(ref.ObjectId), IList)

        //        For Each item In value
        //            ref.List.Add(item)
        //        Next
        //    Next

        //    For Each cl As Action(Of ISerializer) In _FixupCallbacks
        //        cl.DynamicInvoke(Me)
        //    Next

        //    Dim objroot As Object = _objects(rootId)

        //    _objects = Nothing
        //    _valueReferences = Nothing
        //    _itemReferences = Nothing
        //    _FixupCallbacks = Nothing

        //    Return objroot
        //End Function

        void ISerializer.RegisterObject(int id, object graph)
        {
            _DeserializeObjectIdDictionary.Add(id, graph);
        }

        void ISerializer.RegisterPropertyValueFixUp(PropertyValueFixUp fixup)
        {
            _PropertyValueFixUps.Add(fixup);
        }

        void ISerializer.RegisterListItemFixUp(ListItemFixUp fixup)
        {
            _ListItemFixUps.Add(fixup);
        }

        void ISerializer.RegisterReadOnlyListPropertyFixUp(ReadOnlyListPropertyFixUp fixup)
        {
            _ReadOnlyListPropertyFixUps.Add(fixup);
        }

        void ISerializer.RegisterCustomFixUp(IFixUp fixup)
        {
            _CustomFixUps.Add(fixup);
        }

        void ISerializer.Serialize(object graph, XmlWriter writer)
        {
            int rootId;

            _ObjectsToSerialize = new Queue<object>();
            _ObjectsSerialized = new List<object>();
            _SerializeObjectIdDictionary = new Dictionary<object, int>();
            _SerializeObjectId = 0;

            rootId = GetSerializeObjectId(graph);

            writer.WriteStartElement("root");
            writer.WriteAttributeString("__rootId", rootId.ToString);

            object obj;
            IReferenceFormatter formatter;

            while (!(_ObjectsToSerialize.Count == 0))
            {
                obj = _ObjectsToSerialize.Dequeue;
                _ObjectsSerialized.Add(obj);

                formatter = this.GetReferenceFormatter(obj.GetType);
                formatter.Serialize(obj, writer);
            }

            writer.WriteEndElement();

            _ObjectsToSerialize.Clear();
            _ObjectsToSerialize = null;

            _ObjectsSerialized.Clear();
            _ObjectsSerialized = null;

            _SerializeObjectIdDictionary.Clear();
            _SerializeObjectIdDictionary = null;

            _SerializeObjectId = null;
        }

        object ISerializer.Deserialize(XmlReader reader)
        {
            int rootId;

            _DeserializeObjectIdDictionary = new Dictionary<int, object>();
            _PropertyValueFixUps = new List<PropertyValueFixUp>();
            _ListItemFixUps = new List<ListItemFixUp>();
            _ReadOnlyListPropertyFixUps = new List<ReadOnlyListPropertyFixUp>();
            _CustomFixUps = new List<IFixUp>();

            //_FixupCallbacks = New List(Of Action(Of ISerializer))

            // Check for data
            if (!reader.Read)
                return null;

            reader.MoveToFirstAttribute();
            rootId = int.Parse(reader.Value);

            while (reader.Read)
            {
                if (!reader.NodeType == XmlNodeType.Element)
                    continue;

                var typeName = reader.GetAttribute("__type");
                Type type = type.GetType(typeName);
                var formatter = this.GetReferenceFormatter(type);

                formatter.Deserialize(reader);
            }

            foreach (var fixup in _PropertyValueFixUps)
            {
                fixup.Execute(this);
            }

            foreach (var fixup in _ListItemFixUps)
            {
                fixup.Execute(this);
            }

            foreach (var fixup in _ReadOnlyListPropertyFixUps)
            {
                fixup.Execute(this);
            }

            foreach (var fixup in _CustomFixUps)
            {
                fixup.Execute(this);
            }

            var objroot = GetDeserializeObjectById(rootId);

            _DeserializeObjectIdDictionary.Clear();
            _DeserializeObjectIdDictionary = null;

            _PropertyValueFixUps.Clear();
            _PropertyValueFixUps = null;

            _ListItemFixUps.Clear();
            _ListItemFixUps = null;

            _ReadOnlyListPropertyFixUps.Clear();
            _ReadOnlyListPropertyFixUps = null;

            _CustomFixUps.Clear();
            _CustomFixUps = null;

            return objroot;
        }

        public void InitializeForUnitTest()
        {
            _SerializeObjectId = 0;
            _SerializeObjectIdDictionary = new Dictionary<object, int>();
            _ObjectsToSerialize = new Queue<object>();
        }
    }
}