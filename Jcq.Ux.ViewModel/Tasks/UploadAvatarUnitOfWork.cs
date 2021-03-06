// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadAvatarUnitOfWork.cs" company="Jan-Cornelius Molnar">
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
using System.IO;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.Ux.ViewModel
{
    public static class UploadAvatarUnitOfWork
    {
        public static void Execute()
        {
            var imageFile = new FileInfo(ApplicationService.Current.Identity.ImageUrl);

            byte[] avatar;

            using (FileStream fs = imageFile.OpenRead())
            {
                using (var br = new BinaryReader(fs))
                {
                    avatar = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
            }

            ApplicationService.Current.Context.GetService<IIconService>().UploadIcon(avatar);
        }
    }
}