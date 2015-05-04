// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultValueFormatter.cs" company="Jan-Cornelius Molnar">
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
using System.Xml;

namespace JCsTools.Xml.Formatter
{
    //Public Class DefaultValueFormatter
    //    Implements IValueFormatter
    //    Private _Context As ISerializer
    //    Private _ValueType As Type
    //    Private _Converter As TypeConverter
    //    Public Sub New(ByVal context As ISerializer, ByVal type As Type)
    //        _Context = context
    //        _ValueType = type
    //        _Converter = TypeDescriptor.GetConverter(type)
    //    End Sub
    //    Public ReadOnly Property ValueType() As Type
    //        Get
    //            Return _ValueType
    //        End Get
    //    End Property
    //    Public Function Serialize(ByVal graph As Object) As String Implements IValueFormatter.Serialize
    //        Return _Converter.ConvertToString(graph)
    //    End Function
    //    Public Function Deserialize(ByVal type As System.Type, ByVal value As String) As Object Implements IValueFormatter.Deserialize
    //        Return _Converter.ConvertFromString(value)
    //    End Function
    //End Class

    public class XmlValueFormatter : IValueFormatter
    {
        private readonly ISerializer _Context;
        private readonly Type _ValueType;
        private readonly TypeCode _ValueTypeCode;

        public XmlValueFormatter(ISerializer context, Type type)
        {
            _Context = context;
            _ValueType = type;
            _ValueTypeCode = Type.GetTypeCode(_ValueType);
        }

        string IValueFormatter.Serialize(object graph)
        {
            switch (_ValueTypeCode)
            {
                case TypeCode.Boolean:
                    return XmlConvert.ToString((bool) graph);
                    break;
                case TypeCode.Byte:
                    return XmlConvert.ToString((byte) graph);
                    break;
                case TypeCode.Char:
                    return XmlConvert.ToString((char) graph);
                    break;
                case TypeCode.DateTime:
                    return XmlConvert.ToString((DateTime) graph, XmlDateTimeSerializationMode.Utc);
                    break;
                case TypeCode.Decimal:
                    return XmlConvert.ToString((decimal) graph);
                    break;
                case TypeCode.Double:
                    return XmlConvert.ToString((double) graph);
                    break;
                case TypeCode.Int16:
                    return XmlConvert.ToString((Int16) graph);
                    break;
                case TypeCode.Int32:
                    return XmlConvert.ToString((Int32) graph);
                    break;
                case TypeCode.Int64:
                    return XmlConvert.ToString((Int64) graph);
                    break;
                case TypeCode.SByte:
                    return XmlConvert.ToString((sbyte) graph);
                    break;
                case TypeCode.Single:
                    return XmlConvert.ToString((Single) graph);
                    break;
                case TypeCode.UInt16:
                    return XmlConvert.ToString((UInt16) graph);
                    break;
                case TypeCode.UInt32:
                    return XmlConvert.ToString((UInt32) graph);
                    break;
                case TypeCode.UInt64:
                    return XmlConvert.ToString((UInt64) graph);
                    break;
                default:
                    throw new ArgumentException("Type of this TypeCode cannot be serialized.");
                    break;
            }
        }

        object IValueFormatter.Deserialize(string value)
        {
            switch (_ValueTypeCode)
            {
                case TypeCode.Boolean:
                    return XmlConvert.ToBoolean(value);
                    break;
                case TypeCode.Byte:
                    return XmlConvert.ToByte(value);
                    break;
                case TypeCode.Char:
                    return XmlConvert.ToChar(value);
                    break;
                case TypeCode.DateTime:
                    return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc);
                    break;
                case TypeCode.Decimal:
                    return XmlConvert.ToDecimal(value);
                    break;
                case TypeCode.Double:
                    return XmlConvert.ToDouble(value);
                    break;
                case TypeCode.Int16:
                    return XmlConvert.ToInt16(value);
                    break;
                case TypeCode.Int32:
                    return XmlConvert.ToInt32(value);
                    break;
                case TypeCode.Int64:
                    return XmlConvert.ToInt64(value);
                    break;
                case TypeCode.SByte:
                    return XmlConvert.ToSByte(value);
                    break;
                case TypeCode.Single:
                    return XmlConvert.ToSingle(value);
                    break;
                case TypeCode.UInt16:
                    return XmlConvert.ToUInt16(value);
                    break;
                case TypeCode.UInt32:
                    return XmlConvert.ToUInt32(value);
                    break;
                case TypeCode.UInt64:
                    return XmlConvert.ToUInt64(value);
                    break;
                default:
                    throw new ArgumentException("Type of this TypeCode cannot be serialized.");
                    break;
            }
        }

        public Type ValueType
        {
            get { return _ValueType; }
        }
    }
}