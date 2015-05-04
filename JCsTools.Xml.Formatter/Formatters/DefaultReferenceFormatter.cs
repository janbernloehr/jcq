// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultReferenceFormatter.cs" company="Jan-Cornelius Molnar">
//  Copyright (c) 2015-2015 Jan-Cornelius Molnar
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace JCsTools.Xml.Formatter
{
    public class DefaultReferenceFormatter : IReferenceFormatter
    {
        private readonly Dictionary<string, PropertyDescriptor> _CachedProperites;
        private readonly List<IPropertySerializationAction> _PropertySerializationActions;
        private readonly Type _ReferenceType;
        private readonly ISerializer _Serializer;

        protected DefaultReferenceFormatter(ISerializer parent, Type type, bool createPropertyCache,
            bool createSerializationActions)
        {
            _Serializer = parent;
            _ReferenceType = type;

            List<PropertyDescriptor> propertyDescriptors;

            if (createPropertyCache | createSerializationActions)
            {
                propertyDescriptors = (from prop in type.GetProperties() select new PropertyDescriptor(prop)).ToList();
                propertyDescriptors.TrimExcess();
            }

            if (createPropertyCache)
            {
                _CachedProperites = propertyDescriptors.ToDictionary(prop => prop.PropertyName);
            }

            if (createSerializationActions)
            {
                _PropertySerializationActions = new List<IPropertySerializationAction>();

                foreach (var descriptor in propertyDescriptors)
                {
                    if (!descriptor.IsReadOnly)
                    {
                        if (descriptor.IsValueType)
                        {
                            var formatter = Serializer.GetValueFormatter(descriptor.PropertyType);

                            _PropertySerializationActions.Add(new ValueSerializationAction(descriptor, formatter));
                        }
                        else if (descriptor.IsString)
                        {
                            _PropertySerializationActions.Add(new StringSerializationAction(descriptor));
                        }
                        else
                        {
                            _PropertySerializationActions.Add(new ReferenceSerializationAction(descriptor, Serializer));
                        }
                    }
                    else if (descriptor.IsIListDescendant)
                    {
                        _PropertySerializationActions.Add(new ReferenceSerializationAction(descriptor, Serializer));
                    }
                }

                _PropertySerializationActions.TrimExcess();
            }
        }

        public DefaultReferenceFormatter(ISerializer parent, Type type) : this(parent, type, true, true)
        {
        }

        protected ISerializer Serializer
        {
            get { return _Serializer; }
        }

        protected Dictionary<string, PropertyDescriptor> CachedProperties
        {
            get { return _CachedProperites; }
        }

        Type IReferenceFormatter.ReferenceType
        {
            get { return _ReferenceType; }
        }

        #region  Serialization Methods 

        protected virtual void WriteIdAttribute(object graph, XmlWriter writer)
        {
            string objectId;

            objectId = Serializer.GetSerializeObjectId(graph).ToString();

            writer.WriteAttributeString("__id", objectId);
        }

        protected virtual void WriteTypeAttribute(object graph, XmlWriter writer)
        {
            WriteTypeAttribute(graph, _ReferenceType, writer);
        }

        protected virtual void WriteTypeAttribute(object graph, Type type, XmlWriter writer)
        {
            string typeName;

            typeName = type.AssemblyQualifiedName;

            writer.WriteAttributeString("__type", typeName);
        }

        protected virtual void WriteValueAttribute(object graph, string value, XmlWriter writer)
        {
            writer.WriteAttributeString("__value", value);
        }

        protected virtual void SerializeProperties(object graph, XmlWriter writer)
        {
            foreach (var action in _PropertySerializationActions)
            {
                action.SerializeProperty(graph, writer);
            }
        }

        void IReferenceFormatter.Serialize(object graph, XmlWriter writer)
        {
            writer.WriteStartElement("object");

            WriteIdAttribute(graph, writer);
            WriteTypeAttribute(graph, writer);

            SerializeProperties(graph, writer);

            writer.WriteEndElement();
        }

        #endregion

        #region  Deserialization Methods 

        protected virtual object CreateObject(Type type, XmlReader reader)
        {
            return Activator.CreateInstance(type);
        }

        protected virtual void DeserializeProperties(object graph, XmlReader reader)
        {
            if (!reader.MoveToNextAttribute())
                return;

            do
            {
                var propertyName = reader.Name;
                PropertyDescriptor descriptor = null;

                _CachedProperites.TryGetValue(propertyName, out descriptor);
                if (descriptor == null)
                    continue;

                if (!descriptor.IsReadOnly)
                {
                    if (descriptor.IsValueType)
                    {
                        var formatter = Serializer.GetValueFormatter(descriptor.PropertyType);
                        var value = formatter.Deserialize(reader.Value);

                        descriptor.SetValue(graph, value);
                    }
                    else if (descriptor.IsString)
                    {
                        descriptor.SetValue(graph, reader.Value);
                    }
                    else
                    {
                        var @ref = new PropertyValueFixUp(int.Parse(reader.Value), descriptor, graph);
                        Serializer.RegisterPropertyValueFixUp(@ref);
                    }
                }
                else if (descriptor.IsIListDescendant)
                {
                    var list = (IList) descriptor.GetValue(graph);

                    if (list == null)
                        continue;

                    var @ref = new ReadOnlyListPropertyFixUp(int.Parse(reader.Value), list);
                    Serializer.RegisterReadOnlyListPropertyFixUp(@ref);
                }
            } while (reader.MoveToNextAttribute());
        }

        protected virtual int GetObjectId(XmlReader reader)
        {
            var objectId = int.Parse(reader.Value);

            return objectId;
        }

        protected virtual string GetObjectValue(XmlReader reader)
        {
            var value = reader.Value;

            return value;
        }

        protected virtual Type GetObjectType(XmlReader reader)
        {
            var typeName = reader.Value;

            return Type.GetType(typeName);
        }

        object IReferenceFormatter.Deserialize(XmlReader reader)
        {
            int objectId;
            Type type;
            object obj;

            reader.MoveToFirstAttribute();
            objectId = GetObjectId(reader);

            reader.MoveToNextAttribute();
            type = GetObjectType(reader);

            obj = CreateObject(type, reader);

            DeserializeProperties(obj, reader);

            Serializer.RegisterObject(objectId, obj);

            return obj;
        }

        #endregion
    }

    public interface IPropertySerializationAction
    {
        PropertyDescriptor PropertyDescriptor { get; }
        void SerializeProperty(object graph, XmlWriter writer);
    }

    public class ValueSerializationAction : IPropertySerializationAction
    {
        private readonly PropertyDescriptor _PropertyDescriptor;
        private readonly IValueFormatter _ValueFormatter;

        public ValueSerializationAction(PropertyDescriptor descriptor, IValueFormatter formatter)
        {
            _PropertyDescriptor = descriptor;
            _ValueFormatter = formatter;
        }

        void IPropertySerializationAction.SerializeProperty(object graph, XmlWriter writer)
        {
            string serializedValue;
            object propertyValue;

            
            propertyValue = _PropertyDescriptor.GetValue(graph);
            serializedValue = _ValueFormatter.Serialize(propertyValue);

            writer.WriteAttributeString(_PropertyDescriptor.PropertyName, serializedValue);
        }

        PropertyDescriptor IPropertySerializationAction.PropertyDescriptor
        {
            get { return _PropertyDescriptor; }
        }
    }

    public class StringSerializationAction : IPropertySerializationAction
    {
        private readonly PropertyDescriptor _PropertyDescriptor;

        public StringSerializationAction(PropertyDescriptor descriptor)
        {
            _PropertyDescriptor = descriptor;
        }

        void IPropertySerializationAction.SerializeProperty(object graph, XmlWriter writer)
        {
            string serializedValue;

            serializedValue = (string) _PropertyDescriptor.GetValue(graph);

            writer.WriteAttributeString(_PropertyDescriptor.PropertyName, serializedValue);
        }

        PropertyDescriptor IPropertySerializationAction.PropertyDescriptor
        {
            get { return _PropertyDescriptor; }
        }
    }

    public class ReferenceSerializationAction : IPropertySerializationAction
    {
        private readonly PropertyDescriptor _PropertyDescriptor;
        private readonly ISerializer _Serializer;

        public ReferenceSerializationAction(PropertyDescriptor descriptor, ISerializer formatter)
        {
            _PropertyDescriptor = descriptor;
            _Serializer = formatter;
        }

        PropertyDescriptor IPropertySerializationAction.PropertyDescriptor
        {
            get { return _PropertyDescriptor; }
        }

        void IPropertySerializationAction.SerializeProperty(object graph, XmlWriter writer)
        {
            string objectId;
            object propertyValue;

            propertyValue = _PropertyDescriptor.GetValue(graph);
            objectId = _Serializer.GetSerializeObjectId(propertyValue).ToString();

            writer.WriteAttributeString(_PropertyDescriptor.PropertyName, objectId);
        }
    }
}