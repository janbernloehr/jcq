﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonIdentityProvider.cs" company="Jan-Cornelius Molnar">
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
using System.Linq;
using Newtonsoft.Json;

namespace Jcq.IdentityManager
{
    public class JsonIdentityProvider : IdentityProvider
    {
        private readonly FileInfo _jsonFile;

        public JsonIdentityProvider(FileInfo jsonFile)
        {
            _jsonFile = jsonFile;
        }

        public void Load()
        {
            // if no file exists, exit :)
            if (!_jsonFile.Exists)
                return;

            string json;

            using (var reader = new StreamReader(_jsonFile.FullName))
            {
                json = reader.ReadToEnd();
            }

            var identities = JsonConvert.DeserializeObject<Identity[]>(json,
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects});

            foreach (Identity identity in identities)
            {
                Identities.Add(identity);
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(Identities.ToArray(),
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects});

            if (!_jsonFile.Directory.Exists)
                _jsonFile.Directory.Create();

            using (var writer = new StreamWriter(_jsonFile.FullName))
            {
                writer.Write(json);
            }
        }
    }
}