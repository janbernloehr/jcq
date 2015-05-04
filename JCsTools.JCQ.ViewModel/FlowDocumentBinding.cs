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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
namespace JCsTools.JCQ.ViewModel
{
  public class RunBinding : DependencyObject
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RunBinding), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnThisPropertyChanged)));

    protected static void OnThisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue != null) {
        object text = (string)e.NewValue;
        object run = (Run)d;

        run.Text = text;
      }
    }

    public static string GetText(DependencyObject obj)
    {
      return (string)obj.GetValue(TextProperty);
    }

    public static void SetText(DependencyObject obj, string value)
    {
      obj.SetValue(TextProperty, value);
    }
  }

  public class FlowDocumentBinding : DependencyObject
  {
    public static readonly DependencyProperty CollectionProperty = DependencyProperty.RegisterAttached("Collection", typeof(System.Collections.Specialized.INotifyCollectionChanged), typeof(FlowDocumentBinding), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnThisPropertyChanged)));

    private static Dictionary<System.Collections.Specialized.INotifyCollectionChanged, DependencyObject> lookup = new Dictionary<System.Collections.Specialized.INotifyCollectionChanged, DependencyObject>();

    protected static void OnThisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue != null) {
        object collection = (System.Collections.Specialized.INotifyCollectionChanged)e.OldValue;

        collection.CollectionChanged -= OnCollectionChanged;
      }

      if (e.NewValue != null) {
        object collection = (System.Collections.Specialized.INotifyCollectionChanged)e.NewValue;

        lookup.Add(collection, d);

        FlowDocument document = (FlowDocument)d;

        foreach ( item in (IEnumerable)collection) {
          AppendItemToDocument(document, item);
        }

        collection.CollectionChanged += OnCollectionChanged;
      }
    }

    private static void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action) {
        case Specialized.NotifyCollectionChangedAction.Add:
          object item = e.NewItems(0);
          object collection = (System.Collections.Specialized.INotifyCollectionChanged)sender;
          FlowDocument document = (FlowDocument)lookup(collection);

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
      paragraph = (Paragraph)template.LoadContent;
      paragraph.DataContext = item;

      document.Blocks.Add(paragraph);
    }

    public static System.Collections.Specialized.INotifyCollectionChanged GetCollection(DependencyObject obj)
    {
      return (System.Collections.Specialized.INotifyCollectionChanged)obj.GetValue(CollectionProperty);
    }

    public static void SetCollection(DependencyObject obj, System.Collections.Specialized.INotifyCollectionChanged value)
    {
      obj.SetValue(CollectionProperty, value);
    }
  }

  internal class DataTemplateSelector
  {
    private class HelperPresenter : ContentPresenter
    {
      public DataTemplate SelectTemplate(object item, DependencyObject container)
      {
        System.Reflection.MethodInfo m;

        Content = item;

        m = typeof(ContentPresenter).GetMethod("ChangeLogicalParent", Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance);
        m.Invoke(this, new object[] { container });

        return ChooseTemplate();
      }
    }

    public static DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      object helper = new HelperPresenter();

      return helper.SelectTemplate(item, container);
    }
  }
}

