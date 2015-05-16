// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonIdentityProvider.cs" company="Jan-Cornelius Molnar">
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

using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace JCsTools.IdentityManager
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

            foreach (var identity in identities)
            {
                Identities.Add(identity);
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Identities.ToArray(),
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