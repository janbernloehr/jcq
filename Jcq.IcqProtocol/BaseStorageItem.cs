// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseStorageItem.cs" company="Jan-Cornelius Molnar">
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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using JCsTools.JCQ.IcqInterface.Annotations;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public abstract class BaseStorageItem : IStorageItem
    {
        private string _identifier;
        private string _name;

        protected BaseStorageItem()
        {
        }

        protected BaseStorageItem(string identifier, string name)
        {
            _identifier = identifier;
            _name = name;
        }

        public string Identifier
        {
            get { return _identifier; }
            set
            {
                _identifier = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return string.IsNullOrEmpty(_name) ? _identifier : _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}