// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowDocumentBinding.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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