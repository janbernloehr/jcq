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
/// <summary>
/// This ViewModel features selecting an image from a list of images.
/// </summary>
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.ViewModel
{
  public class ImageSelectorViewModel : System.ComponentModel.INotifyPropertyChanged
  {
    private List<string> _ImageFiles;
    private string _SelectedImageFile;
    private System.IO.DirectoryInfo _DataDirectory;

    public ImageSelectorViewModel(System.IO.DirectoryInfo dataDirectory)
    {
      _DataDirectory = dataDirectory;
      LoadImageFiles();
    }

    /// <summary>
    /// Gets a list of image file paths which a user can pick from.
    /// </summary>
    public List<string> ImageFiles {
      get { return _ImageFiles; }
    }

    /// <summary>
    /// Gets or sets the currently selected image file path.
    /// </summary>
    public string SelectedImageFile {
      get { return _SelectedImageFile; }
      set {
        _SelectedImageFile = value;
        OnPropertyChanged("SelectedImageFile");
      }
    }

    /// <summary>
    /// Finds image files in the data directory and adds them to ImageFiles.
    /// </summary>
    public void LoadImageFiles()
    {
      string avatarDirectoryPath;
      System.IO.DirectoryInfo avatarDirectory;

      avatarDirectoryPath = System.IO.Path.Combine(_DataDirectory.FullName, "avatars");
      avatarDirectory = new System.IO.DirectoryInfo(avatarDirectoryPath);

      if (!avatarDirectory.Exists)
        avatarDirectory.Create();

      _ImageFiles = (from file in avatarDirectory.GetFilesfile.FullName).ToList;

      OnPropertyChanged("ImageFiles");
    }

    /// <summary>
    /// Launches an FileDialog allowing the user to specify a new image which will
    /// be added to the data directory.
    /// </summary>
    /// <remarks></remarks>
    public void AddImageFile()
    {
      object dialog = new Microsoft.Win32.OpenFileDialog();

      dialog.CheckFileExists = true;
      dialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

      if (dialog.ShowDialog) {
        System.IO.FileInfo imageFile;
        string avatarDirectoryPath;
        System.IO.DirectoryInfo avatarDirectory;

        imageFile = new System.IO.FileInfo(dialog.FileName);
        avatarDirectoryPath = System.IO.Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName, "avatars");
        avatarDirectory = new System.IO.DirectoryInfo(avatarDirectoryPath);

        string newPath = System.IO.Path.Combine(avatarDirectory.FullName, Guid.NewGuid.ToString + imageFile.Extension);

        imageFile.CopyTo(newPath);

        LoadImageFiles();
      }
    }

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) {
        PropertyChanged(this, new ComponentModel.PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
  }
}

