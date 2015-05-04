// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageSelectorViewModel.cs" company="Jan-Cornelius Molnar">
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