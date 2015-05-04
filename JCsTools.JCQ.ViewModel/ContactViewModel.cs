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
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace JCsTools.JCQ.ViewModel
{
  public class ContactViewModel : System.Windows.Threading.DispatcherObject, INotifyPropertyChanged
  {
    private IcqInterface.Interfaces.IContact _Model;
    private GroupViewModel _GroupVm;

    private bool _ContactImageCreated;
    private ImageSource _ContactImage;

    public ContactViewModel(IcqInterface.Interfaces.IContact model)
    {
      _Model = model;

      _Model.PropertyChanged += HandlePropertyChanged;

      _Model.IconHashReceived += OnIconHashReceived;
      _Model.IconDataReceived += OnIconDataReceived;
    }

    public IcqInterface.Interfaces.IContact Model {
      get { return _Model; }
    }

    public System.Collections.Hashtable Attributes {
      get { return _Model.Attributes; }
    }

    public string Identifier {
      get { return _Model.Identifier; }
    }

    public string Name {
      get { return _Model.Name; }
    }

    public GroupViewModel Group {
      get {
        if (_Model.Group == null)
          return null;
        if (_GroupVm == null)
          _GroupVm = GroupViewModelCache.GetViewModel(_Model.Group);

        return _GroupVm;
      }
    }

    public System.DateTime MemberSince {
      get { return Model.MemberSince; }
    }

    public System.DateTime SignOnTime {
      get { return Model.SignOnTime; }
    }

    public string FirstName {
      get { return Model.FirstName; }
    }

    public string LastName {
      get { return Model.LastName; }
    }

    public string EmailAddress {
      get { return Model.EmailAddress; }
    }

    public IcqInterface.Interfaces.ContactGender Gender {
      get { return Model.Gender; }
    }

    public bool AuthorizationRequired {
      get { return Model.AuthorizationRequired; }
    }


    public IcqInterface.Interfaces.IStatusCode Status {
      get { return _Model.Status; }
    }

    public int StatusFlag {
      get {
        if (object.ReferenceEquals(Status, IcqInterface.IcqStatusCodes.Online)) {
          return 0;
        } else if (object.ReferenceEquals(Status, IcqInterface.IcqStatusCodes.Offline)) {
          return 2;
        } else {
          return 1;
        }
      }
    }

    public Brush StatusBrush {
      get {
        if (object.ReferenceEquals(Status, IcqInterface.IcqStatusCodes.Online)) {
          return (Brush)Application.Current.Resources("vbrOnline");
        } else if (object.ReferenceEquals(Status, IcqInterface.IcqStatusCodes.Offline)) {
          return (Brush)Application.Current.Resources("vbrOffline");
        } else {
          return (Brush)Application.Current.Resources("vbrAway");
        }
      }
    }

    protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      OnPropertyChanged(e);

      if (e.PropertyName == "Status") {
        OnPropertyChanged(new PropertyChangedEventArgs("StatusFlag"));
        OnPropertyChanged(new PropertyChangedEventArgs("StatusBrush"));
      }
    }

    protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (Group != null)
        Group.OnContactPropertyChanged(this, e);

      if (PropertyChanged != null) {
        PropertyChanged(this, e);
      }
    }

    private void CreateContactImage()
    {
      _ContactImageCreated = true;

      if (Model.IconData != null && Model.IconData.Count > 0) {
        System.Windows.Media.Imaging.BitmapDecoder decoder;
        System.IO.MemoryStream ms = new System.IO.MemoryStream(Model.IconData.ToArray);

        ms = new System.IO.MemoryStream(Model.IconData.ToArray);
        decoder = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

        if (decoder != null)
          _ContactImage = decoder.Frames(0);
      }
    }

    public ImageSource ContactImage {
      get {
        if (!_ContactImageCreated)
          CreateContactImage();

        return _ContactImage;
      }
    }

    public Visibility ContactImageVisibility {
      get {
        if (_ContactImage != null) {
          return Visibility.Visible;
        } else {
          return Visibility.Collapsed;
        }
      }
    }

    private void OnIconDataReceived(object sender, EventArgs e)
    {
      try {
        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, new Action(CreateContactImage));

        OnPropertyChanged("ContactImage");
        OnPropertyChanged("ContactImageVisibility");
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    private void OnIconHashReceived(object sender, EventArgs e)
    {
      IcqInterface.Interfaces.IIconService svAvatar;
      try {
        svAvatar = ApplicationService.Current.Context.GetService<IcqInterface.Interfaces.IIconService>();
        svAvatar.RequestContactIcon(Model);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected void OnPropertyChanged(string propertyName)
    {
      OnPropertyChanged(new ComponentModel.PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
  }
}

