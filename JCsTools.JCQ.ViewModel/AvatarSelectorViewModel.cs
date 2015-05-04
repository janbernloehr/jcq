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
namespace JCsTools.JCQ.ViewModel
{
  public class AvatarSelectorViewModel
  {
    private ImageSelectorViewModel _ImageSelector;
    private IdentityManager.IIdentity _Identity;

    public AvatarSelectorViewModel(IdentityManager.IIdentity identity)
    {
      _ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);
      _Identity = identity;
    }

    public ImageSelectorViewModel ImageSelector {
      get { return _ImageSelector; }
    }

    public void SelectImageFile()
    {
      System.IO.FileInfo imageFile;

      BitmapImage sourceBitmap;
      Image visual;
      RenderTargetBitmap targetBitmap;
      JpegBitmapEncoder encoder;

      imageFile = new System.IO.FileInfo(ImageSelector.SelectedImageFile);

      if (imageFile.Exists) {
        sourceBitmap = new BitmapImage(new Uri(imageFile.FullName));

        visual = new Image();
        visual.Source = sourceBitmap;
        visual.Arrange(new Rect(0, 0, 48, 48));

        targetBitmap = new RenderTargetBitmap(48, 48, 96, 96, PixelFormats.Default);

        targetBitmap.Render(visual);

        encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

        string newfile = System.IO.Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName, string.Format("{0}.[default].jpg", _Identity.Identifier));

        using (fs == new System.IO.FileStream(newfile, IO.FileMode.Create, IO.FileAccess.Write)) {
          encoder.Save(fs);
        }

        _Identity.ImageUrl = newfile;
      } else {
        _Identity.ImageUrl = null;
      }
    }
  }
}

