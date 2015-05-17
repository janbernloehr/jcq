// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewModel.cs" company="Jan-Cornelius Molnar">
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
        //public Hashtable Attributes
        //{
        //    get { return Model.Attributes; }
        //}

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
                if ((IcqStatusCode) Status == IcqStatusCodes.Online)
                {
                    return 0;
                }
                if ((IcqStatusCode) Status == IcqStatusCodes.Offline)
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
                if ((IcqStatusCode) Status == IcqStatusCodes.Online)
                {
                    return (Brush) Application.Current.Resources["vbrOnline"];
                }
                if ((IcqStatusCode) Status == IcqStatusCodes.Offline)
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