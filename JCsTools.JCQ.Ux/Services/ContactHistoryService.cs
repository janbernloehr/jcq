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
//Public Class ContactHistoryService
//    Inherits Core.Service
//    Implements IContactHistoryService
//    Private ReadOnly _HistoryCache As New Dictionary(Of ContactViewModel, FlowDocumentReader)
//    Private Shared ReadOnly _StorageLocation As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(System.IO.Path.Combine(App.DataStorageDirectoryPath, "history"))
//    Public Sub New()
//    End Sub
//    Private Shared ReadOnly Property StorageLocation() As System.IO.DirectoryInfo
//        Get
//            Return _StorageLocation
//        End Get
//    End Property
//    Public Sub Load() Implements IContactHistoryService.Load
//        Debug.WriteLine(String.Format("Loading history: {0}", StorageLocation.FullName), "Ux")
//        If Not StorageLocation.Exists Then Return
//        For Each historyFile As System.IO.FileInfo In StorageLocation.GetFiles("*.xaml")
//            Dim identifier As String
//            Dim contact As ContactViewModel
//            identifier = historyFile.Name.Substring(0, historyFile.Name.IndexOf("."))
//            contact = ContactViewModelCache.GetViewModel(ApplicationService.Current.Context.GetService(Of IcqInterface.Interfaces.IStorageService).GetContactByIdentifier(identifier))
//            Using reader As New System.Xml.XmlTextReader(historyFile.FullName)
//                Dim documentReader As FlowDocumentReader
//                documentReader = TryCast(System.Windows.Markup.XamlReader.Load(reader), FlowDocumentReader)
//                If documentReader IsNot Nothing Then
//                    _HistoryCache.Add(contact, documentReader)
//                End If
//            End Using
//        Next
//    End Sub
//    Public Sub AppendMessage(ByVal contact As ContactViewModel, ByVal message As IcqInterface.Interfaces.IMessage) Implements IContactHistoryService.AppendMessage
//        Dim reader As FlowDocumentReader
//        Dim document As FlowDocument
//        If _HistoryCache.ContainsKey(contact) Then
//            reader = _HistoryCache(contact)
//            document = reader.Document
//        Else
//            document = New FlowDocument
//            document.FontFamily = New Windows.Media.FontFamily("Calibri")
//            reader = New FlowDocumentReader
//            reader.Document = document
//            reader.Name = "reader"
//            reader.ViewingMode = FlowDocumentReaderViewingMode.Scroll
//            _HistoryCache.Add(contact, reader)
//        End If
//        Dim paragraph As Paragraph
//        'TODO: Find Sender Role
//        paragraph = Core.Kernel.Services.GetService(Of IMessageFormattingService).FormatMessage(MessageSenderRole.ContextIdentity, message)
//        document.Blocks.Add(paragraph)
//    End Sub
//    Public Sub Save() Implements IContactHistoryService.Save
//        Dim location As System.IO.DirectoryInfo
//        location = New System.IO.DirectoryInfo(System.IO.Path.Combine(App.DataStorageDirectoryPath, "history"))
//        Debug.WriteLine(String.Format("Saving history: {0}", location.FullName), "Ux")
//        If Not location.Exists Then location.Create()
//        For Each pair As KeyValuePair(Of ContactViewModel, FlowDocumentReader) In _HistoryCache
//            Dim fi As System.IO.FileInfo
//            fi = New System.IO.FileInfo(System.IO.Path.Combine(location.FullName, String.Format("{0}.xaml", pair.Key.Identifier)))
//            Using writer As New System.Xml.XmlTextWriter(fi.FullName, System.Text.Encoding.UTF8)
//                System.Windows.Markup.XamlWriter.Save(pair.Value, writer)
//            End Using
//        Next
//    End Sub
//    Public Function GetHistory(ByVal contact As ContactViewModel) As List(Of Paragraph) Implements IContactHistoryService.GetHistory
//        Dim list As New List(Of Paragraph)
//        If _HistoryCache.ContainsKey(contact) Then
//            Dim document As FlowDocument = _HistoryCache(contact).Document
//            Dim lines As String() = Core.Kernel.Services.GetService(Of IMessageFormattingService).GetTextLinesFromDocument(document)
//            For Each line As String In lines
//                Dim p As Paragraph = New Paragraph
//                Dim r As Run = New Run(line)
//                r.Foreground = Brushes.Gray
//                p.Margin = New System.Windows.Thickness(0)
//                p.Inlines.Add(r)
//                list.Add(p)
//            Next
//        End If
//        Return list
//    End Function
//    Public Function GetHistory(ByVal contact As ContactViewModel, ByVal maxItems As Integer) As List(Of Paragraph) Implements IContactHistoryService.GetHistory
//        Dim list As New List(Of Paragraph)
//        If _HistoryCache.ContainsKey(contact) Then
//            Dim document As FlowDocument = _HistoryCache(contact).Document
//            Dim lines As String() = Core.Kernel.Services.GetService(Of IMessageFormattingService).GetTextLinesFromDocument(document)
//            Dim length As Integer = lines.Length
//            Dim index As Integer = If(length > 5, length - 5, 0)
//            Do While index < length
//                Dim p As Paragraph = New Paragraph
//                Dim r As Run = New Run(lines(index))
//                r.Foreground = Brushes.Gray
//                p.Margin = New System.Windows.Thickness(0)
//                p.Inlines.Add(r)
//                list.Add(p)
//                index += 1
//            Loop
//        End If
//        Return list
//    End Function
//End Class
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Ux
{
  public class ContactHistoryService : IcqInterface.ContextService, IContactHistoryService
  {
    private Dictionary<ContactViewModel, ContactHistory> _Cache;
    private static readonly System.IO.DirectoryInfo _StorageLocation = new System.IO.DirectoryInfo(System.IO.Path.Combine(App.DataStorageDirectoryPath, "history"));

    public ContactHistoryService(IcqInterface.Interfaces.IContext context) : base(context)
    {

      _Cache = new Dictionary<ContactViewModel, ContactHistory>();
    }

    private static System.IO.DirectoryInfo StorageLocation {
      get { return _StorageLocation; }
    }

    public void ViewModel.IContactHistoryService.AppendMessage(ViewModel.ContactViewModel contact, ViewModel.MessageViewModel message)
    {
      ContactHistory history = null;

      if (!_Cache.TryGetValue(contact, history)) {
        history = new ContactHistory(contact.Model);
        _Cache.Add(contact, history);
      }

      history.Messages.Add(message);
    }

    public System.Collections.Generic.List<ViewModel.MessageViewModel> ViewModel.IContactHistoryService.GetHistory(ViewModel.ContactViewModel contact)
    {
      ContactHistory history = null;

      if (_Cache.TryGetValue(contact, history)) {
        return history.Messages;
      } else {
        return new List<MessageViewModel>();
      }
    }

    public System.Collections.Generic.List<ViewModel.MessageViewModel> ViewModel.IContactHistoryService.GetHistory(ViewModel.ContactViewModel contact, int maxItems)
    {
      ContactHistory history = null;
      List<MessageViewModel> messages;

      if (_Cache.TryGetValue(contact, history)) {
        messages = history.Messages;

        if (messages.Count > maxItems) {
          return messages.Skip(messages.Count - maxItems).ToList;
        } else {
          return messages;
        }
      } else {
        return new List<MessageViewModel>();
      }
    }

    public void ViewModel.IContactHistoryService.Load()
    {
      JCsTools.Xml.Formatter.XmlSerializer serializer;

      Debug.WriteLine(string.Format("Loading history: {0}", StorageLocation.FullName), "Ux");

      if (!StorageLocation.Exists)
        return;

      serializer = new JCsTools.Xml.Formatter.XmlSerializer();
      serializer.RegisterReferenceFormatter(typeof(ContactHistory), new ContactHistoryFormatter(serializer));
      serializer.RegisterReferenceFormatter(typeof(TextMessageViewModel), new TextMessageViewModelFormatter(serializer));
      serializer.RegisterReferenceFormatter(typeof(OfflineTextMessageViewModel), new OfflineTextMessageViewModelFormatter(serializer));

      foreach (System.IO.FileInfo historyFile in StorageLocation.GetFiles("*.xml")) {
        string identifier;
        ContactViewModel contact;

        identifier = historyFile.Name.Substring(0, historyFile.Name.IndexOf("."));
        contact = ContactViewModelCache.GetViewModel(ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>.GetContactByIdentifier(identifier));

        using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(historyFile.FullName)) {
          ContactHistory history;

          history = (ContactHistory)serializer.Deserialize(reader);

          _Cache.Add(contact, history);
        }
      }
    }

    public void ViewModel.IContactHistoryService.Save()
    {
      System.IO.FileInfo fiHistoryFile;
      JCsTools.Xml.Formatter.XmlSerializer serializer;

      Debug.WriteLine(string.Format("Saving history: {0}", StorageLocation.FullName), "Ux");

      if (!StorageLocation.Exists)
        StorageLocation.Create();

      serializer = new JCsTools.Xml.Formatter.XmlSerializer();
      serializer.RegisterReferenceFormatter(typeof(ContactHistory), new ContactHistoryFormatter(serializer));
      serializer.RegisterReferenceFormatter(typeof(TextMessageViewModel), new TextMessageViewModelFormatter(serializer));
      serializer.RegisterReferenceFormatter(typeof(OfflineTextMessageViewModel), new OfflineTextMessageViewModelFormatter(serializer));

      foreach ( pair in _Cache) {
        fiHistoryFile = new System.IO.FileInfo(System.IO.Path.Combine(StorageLocation.FullName, string.Format("{0}.xml", pair.Key.Identifier)));

        using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(fiHistoryFile.FullName, System.Text.Encoding.UTF8)) {
          serializer.Serialize(pair.Value, writer);
        }
      }
    }
  }

  public class ContactHistoryFormatter : JCsTools.Xml.Formatter.DefaultIListReferenceFormatter
  {
    public ContactHistoryFormatter(JCsTools.Xml.Formatter.ISerializer context) : base(context, typeof(ContactHistory))
    {
    }

    protected override object CreateObject(System.Type type, System.Xml.XmlReader reader)
    {
      string identifier;
      IcqInterface.Interfaces.IContact contact;

      identifier = reader.GetAttribute("contact");
      contact = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>.GetContactByIdentifier(identifier);

      return new ContactHistory(contact);
    }

    public override void Serialize(object graph, System.Xml.XmlWriter writer)
    {
      ContactHistory history;

      history = (ContactHistory)graph;

      writer.WriteStartElement("list");

      WriteIdAttribute(graph, writer);
      WriteTypeAttribute(graph, writer);

      writer.WriteAttributeString("contact", history.Contact.Identifier);

      SerializeItems((Collections.IList)history.Messages, writer);

      writer.WriteEndElement();
    }

    public override object Deserialize(System.Xml.XmlReader reader)
    {
      int objectId;
      Type type;
      ContactHistory history;

      reader.MoveToFirstAttribute();
      objectId = GetObjectId(reader);

      reader.MoveToNextAttribute();
      type = GetObjectType(reader);

      history = (ContactHistory)CreateObject(type, reader);

      if (!reader.IsEmptyElement) {
        DeserializeItems(history.Messages, reader);
      }

      Serializer.RegisterObject(objectId, history);

      return history;
    }
  }

  public class TextMessageViewModelFormatter : JCsTools.Xml.Formatter.DefaultReferenceFormatter
  {
    public TextMessageViewModelFormatter(JCsTools.Xml.Formatter.ISerializer parent) : base(parent, typeof(TextMessageViewModel), false, false)
    {
    }

    protected override object CreateObject(System.Type type, System.Xml.XmlReader reader)
    {
      System.DateTime dateCreated;
      string senderIdentifier;
      string recipientIdentifier;
      string message;

      IcqInterface.Interfaces.IContact senderModel;
      IcqInterface.Interfaces.IContact recipientModel;

      ContactViewModel sender;
      ContactViewModel recipient;

      object svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();

      dateCreated = System.Xml.XmlConvert.ToDateTime(reader.GetAttribute("created"), System.Xml.XmlDateTimeSerializationMode.Utc);
      senderIdentifier = reader.GetAttribute("sender");
      recipientIdentifier = reader.GetAttribute("recipient");
      message = reader.GetAttribute("message");

      senderModel = svStorage.GetContactByIdentifier(senderIdentifier);
      recipientModel = svStorage.GetContactByIdentifier(recipientIdentifier);

      sender = ContactViewModelCache.GetViewModel(senderModel);
      recipient = ContactViewModelCache.GetViewModel(recipientModel);

      return new TextMessageViewModel(dateCreated, sender, recipient, message, MessageColors.HistoryColor);
    }

    protected override void DeserializeProperties(object graph, System.Xml.XmlReader reader)
    {
      // Nothing to do here
    }

    protected override void SerializeProperties(object graph, System.Xml.XmlWriter writer)
    {
      TextMessageViewModel entity = (TextMessageViewModel)graph;

      writer.WriteAttributeString("created", System.Xml.XmlConvert.ToString(entity.DateCreated, System.Xml.XmlDateTimeSerializationMode.Utc));
      writer.WriteAttributeString("sender", entity.Sender.Identifier);
      writer.WriteAttributeString("recipient", entity.Recipient.Identifier);
      writer.WriteAttributeString("message", entity.Message);
    }
  }

  public class OfflineTextMessageViewModelFormatter : JCsTools.Xml.Formatter.DefaultReferenceFormatter
  {
    public OfflineTextMessageViewModelFormatter(JCsTools.Xml.Formatter.ISerializer parent) : base(parent, typeof(OfflineTextMessageViewModel), false, false)
    {
    }

    protected override object CreateObject(System.Type type, System.Xml.XmlReader reader)
    {
      System.DateTime dateCreated;
      System.DateTime dateSent;
      string senderIdentifier;
      string recipientIdentifier;
      string message;

      IcqInterface.Interfaces.IContact senderModel;
      IcqInterface.Interfaces.IContact recipientModel;

      ContactViewModel sender;
      ContactViewModel recipient;

      object svStorage = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IStorageService>();

      dateCreated = System.Xml.XmlConvert.ToDateTime(reader.GetAttribute("created"), System.Xml.XmlDateTimeSerializationMode.Utc);
      dateSent = System.Xml.XmlConvert.ToDateTime(reader.GetAttribute("sent"), System.Xml.XmlDateTimeSerializationMode.Utc);
      senderIdentifier = reader.GetAttribute("sender");
      recipientIdentifier = reader.GetAttribute("recipient");
      message = reader.GetAttribute("message");

      senderModel = svStorage.GetContactByIdentifier(senderIdentifier);
      recipientModel = svStorage.GetContactByIdentifier(recipientIdentifier);

      sender = ContactViewModelCache.GetViewModel(senderModel);
      recipient = ContactViewModelCache.GetViewModel(recipientModel);

      return new OfflineTextMessageViewModel(dateCreated, sender, recipient, message, dateSent, MessageColors.HistoryColor);
    }

    protected override void DeserializeProperties(object graph, System.Xml.XmlReader reader)
    {
      // Nothing to do here
    }

    protected override void SerializeProperties(object graph, System.Xml.XmlWriter writer)
    {
      OfflineTextMessageViewModel entity = (OfflineTextMessageViewModel)graph;

      writer.WriteAttributeString("created", System.Xml.XmlConvert.ToString(entity.DateCreated, System.Xml.XmlDateTimeSerializationMode.Utc));
      writer.WriteAttributeString("sent", System.Xml.XmlConvert.ToString(entity.DateSent, System.Xml.XmlDateTimeSerializationMode.Utc));
      writer.WriteAttributeString("sender", entity.Sender.Identifier);
      writer.WriteAttributeString("recipient", entity.Recipient.Identifier);
      writer.WriteAttributeString("message", entity.Message);
    }
  }

  public class ContactHistory
  {
    private IcqInterface.Interfaces.IContact _Contact;
    private List<MessageViewModel> _Messages;

    public ContactHistory(IcqInterface.Interfaces.IContact contact)
    {
      _Contact = contact;
      _Messages = new List<MessageViewModel>();
    }

    public IcqInterface.Interfaces.IContact Contact {
      get { return _Contact; }
    }

    public List<MessageViewModel> Messages {
      get { return _Messages; }
    }
  }
}

