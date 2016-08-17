// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvatarSelectorViewModel.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using Jcq.IdentityManager.Contracts;

namespace Jcq.Ux.ViewModel
{
    public class AvatarSelectorViewModel : ViewModelBase
    {
        private readonly IIdentity _identity;

        public AvatarSelectorViewModel(IIdentity identity)
        {
            ImageSelector = new ImageSelectorViewModel(ApplicationService.Current.DataStorageDirectory);
            _identity = identity;
        }

        public ImageSelectorViewModel ImageSelector { get; }

        public void SelectImageFile()
        {
            var imageFile = new FileInfo(ImageSelector.SelectedImageFile);

            if (imageFile.Exists)
            {
                string newfile = Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName,
                    string.Format("{0}.[default].jpg", _identity.Identifier));

                AvatarImageService.CreateAvatarImageFromFile(imageFile.FullName, newfile);
                //var sourceBitmap = new BitmapImage(new Uri(imageFile.FullName));

                //var visual = new Image();
                //visual.Source = sourceBitmap;
                //visual.Arrange(new Rect(0, 0, 48, 48));

                //var targetBitmap = new RenderTargetBitmap(48, 48, 96, 96, PixelFormats.Default);

                //targetBitmap.Render(visual);

                //var encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

                //var newfile = Path.Combine(ApplicationService.Current.DataStorageDirectory.FullName,
                //    string.Format("{0}.[default].jpg", _identity.Identifier));

                //using (var fs = new FileStream(newfile, FileMode.Create, FileAccess.Write))
                //{
                //    encoder.Save(fs);
                //}

                _identity.ImageUrl = newfile;
            }
            else
            {
                _identity.ImageUrl = null;
            }
        }
    }
}