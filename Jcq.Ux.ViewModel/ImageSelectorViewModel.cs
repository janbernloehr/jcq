// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageSelectorViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace JCsTools.JCQ.ViewModel
{
    public class ImageSelectorViewModel : INotifyPropertyChanged
    {
        private readonly DirectoryInfo _DataDirectory;
        private string _SelectedImageFile;

        public ImageSelectorViewModel(DirectoryInfo dataDirectory)
        {
            _DataDirectory = dataDirectory;
            LoadImageFiles();
        }

        /// <summary>
        ///     Gets a list of image file paths which a user can pick from.
        /// </summary>
        public List<string> ImageFiles { get; private set; }

        /// <summary>
        ///     Gets or sets the currently selected image file path.
        /// </summary>
        public string SelectedImageFile
        {
            get { return _SelectedImageFile; }
            set
            {
                _SelectedImageFile = value;
                OnPropertyChanged("SelectedImageFile");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Finds image files in the data directory and adds them to ImageFiles.
        /// </summary>
        public void LoadImageFiles()
        {
            string avatarDirectoryPath;
            DirectoryInfo avatarDirectory;

            avatarDirectoryPath = Path.Combine(_DataDirectory.FullName, "avatars");
            avatarDirectory = new DirectoryInfo(avatarDirectoryPath);

            if (!avatarDirectory.Exists)
                avatarDirectory.Create();

            ImageFiles = (from file in avatarDirectory.GetFiles() select file.FullName).ToList();

            OnPropertyChanged("ImageFiles");
        }

        /// <summary>
        ///     Launches an FileDialog allowing the user to specify a new image which will
        ///     be added to the data directory.
        /// </summary>
        /// <remarks></remarks>
        public void AddImageFile()
        {
            var dialog = new OpenFileDialog();

            dialog.CheckFileExists = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            var result = dialog.ShowDialog();

            if (!result.HasValue || !result.Value) return;

            FileInfo imageFile;
            string avatarDirectoryPath;
            DirectoryInfo avatarDirectory;

            imageFile = new FileInfo(dialog.FileName);
            avatarDirectoryPath = Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName, "avatars");
            avatarDirectory = new DirectoryInfo(avatarDirectoryPath);

            var newPath = Path.Combine(avatarDirectory.FullName, Guid.NewGuid() + imageFile.Extension);

            imageFile.CopyTo(newPath);

            LoadImageFiles();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}