// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using JCsTools.Core;
using JCsTools.JCQ.IcqInterface;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class ContactViewModel : DispatcherObject, INotifyPropertyChanged
    {
        private ImageSource _ContactImage;
        private bool _ContactImageCreated;
        private GroupViewModel _GroupVm;

        public ContactViewModel(IContact model)
        {
            Model = model;

            Model.PropertyChanged += HandlePropertyChanged;

            Model.IconHashReceived += OnIconHashReceived;
            Model.IconDataReceived += OnIconDataReceived;
        }

        public IContact Model { get; private set; }

        public Hashtable Attributes
        {
            get { return Model.Attributes; }
        }

        public string Identifier
        {
            get { return Model.Identifier; }
        }

        public string Name
        {
            get { return Model.Name; }
        }

        public GroupViewModel Group
        {
            get
            {
                if (Model.Group == null)
                    return null;

                if (_GroupVm == null)
                    _GroupVm = GroupViewModelCache.GetViewModel(Model.Group);

                return _GroupVm;
            }
        }

        public DateTime MemberSince
        {
            get { return Model.MemberSince; }
        }

        public DateTime SignOnTime
        {
            get { return Model.SignOnTime; }
        }

        public string FirstName
        {
            get { return Model.FirstName; }
        }

        public string LastName
        {
            get { return Model.LastName; }
        }

        public string EmailAddress
        {
            get { return Model.EmailAddress; }
        }

        public ContactGender Gender
        {
            get { return Model.Gender; }
        }

        public bool AuthorizationRequired
        {
            get { return Model.AuthorizationRequired; }
        }

        public IStatusCode Status
        {
            get { return Model.Status; }
        }

        public int StatusFlag
        {
            get
            {
                if (Status == IcqStatusCodes.Online)
                {
                    return 0;
                }
                if (Status == IcqStatusCodes.Offline)
                {
                    return 2;
                }
                return 1;
            }
        }

        public Brush StatusBrush
        {
            get
            {
                if (ReferenceEquals(Status, IcqStatusCodes.Online))
                {
                    return (Brush) Application.Current.Resources["vbrOnline"];
                }
                if (ReferenceEquals(Status, IcqStatusCodes.Offline))
                {
                    return (Brush) Application.Current.Resources["vbrOffline"];
                }
                return (Brush) Application.Current.Resources["vbrAway"];
            }
        }

        public ImageSource ContactImage
        {
            get
            {
                if (!_ContactImageCreated)
                    CreateContactImage();

                return _ContactImage;
            }
        }

        public Visibility ContactImageVisibility
        {
            get
            {
                if (_ContactImage != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);

            if (e.PropertyName != "Status") return;

            OnPropertyChanged(new PropertyChangedEventArgs("StatusFlag"));
            OnPropertyChanged(new PropertyChangedEventArgs("StatusBrush"));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (Group != null)
                Group.OnContactPropertyChanged(this, e);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private void CreateContactImage()
        {
            _ContactImageCreated = true;

            if (Model.IconData == null || Model.IconData.Count == 0) return;

            var ms = new MemoryStream(Model.IconData.ToArray());
            var decoder = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

            if (decoder != null)
                _ContactImage = decoder.Frames.First();
        }

        private void OnIconDataReceived(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(CreateContactImage));

                OnPropertyChanged("ContactImage");
                OnPropertyChanged("ContactImageVisibility");
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        private void OnIconHashReceived(object sender, EventArgs e)
        {
            try
            {
                var svAvatar = ApplicationService.Current.Context.GetService<IIconService>();
                svAvatar.RequestContactIcon(Model);
            }
            catch (Exception ex)
            {
                Kernel.Exceptions.PublishException(ex);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}