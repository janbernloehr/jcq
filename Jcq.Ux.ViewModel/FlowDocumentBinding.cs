// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowDocumentBinding.cs" company="Jan-Cornelius Molnar">
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Documents;

namespace JCsTools.JCQ.ViewModel
{
    public class FlowDocumentBinding : DependencyObject
    {
        protected static void OnThisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                var collection = (INotifyCollectionChanged) e.OldValue;

                collection.CollectionChanged -= OnCollectionChanged;
            }

            if (e.NewValue != null)
            {
                var collection = (INotifyCollectionChanged) e.NewValue;

                lookup.Add(collection, d);

                var document = (FlowDocument) d;

                foreach (var item in (IEnumerable) collection)
                {
                    AppendItemToDocument(document, item);
                }

                collection.CollectionChanged += OnCollectionChanged;
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var item = e.NewItems[0];
                    var collection = (INotifyCollectionChanged) sender;
                    var document = (FlowDocument) lookup[collection];

                    AppendItemToDocument(document, item);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private static void AppendItemToDocument(FlowDocument document, object item)
        {
            DataTemplate template;
            Paragraph paragraph;

            template = DataTemplateSelector.SelectTemplate(item, document);
            paragraph = (Paragraph) template.LoadContent();
            paragraph.DataContext = item;

            document.Blocks.Add(paragraph);
        }

        public static INotifyCollectionChanged GetCollection(DependencyObject obj)
        {
            return (INotifyCollectionChanged) obj.GetValue(CollectionProperty);
        }

        public static void SetCollection(DependencyObject obj, INotifyCollectionChanged value)
        {
            obj.SetValue(CollectionProperty, value);
        }

        public static readonly DependencyProperty CollectionProperty = DependencyProperty.RegisterAttached(
            "Collection", typeof (INotifyCollectionChanged), typeof (FlowDocumentBinding),
            new FrameworkPropertyMetadata(null, OnThisPropertyChanged));

        private static readonly Dictionary<INotifyCollectionChanged, DependencyObject> lookup =
            new Dictionary<INotifyCollectionChanged, DependencyObject>();
    }
}