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

using System.Collections;
using System.ComponentModel;
using JCsTools.JCQ.IcqInterface.Interfaces;

namespace JCsTools.JCQ.IcqInterface
{
    public abstract class BaseStorageItem : IStorageItem, INotifyPropertyChanged
    {
        private readonly Hashtable _Attributes;
        private string _Identifier;
        private string _Name;

        protected BaseStorageItem()
        {
            _Attributes = new Hashtable();
        }

        protected BaseStorageItem(string id, string name)
        {
            _Attributes = new Hashtable();
            _Identifier = id;
            _Name = name;
        }

        public Hashtable Attributes
        {
            get { return _Attributes; }
        }

        public string Identifier
        {
            get { return _Identifier; }
            set
            {
                _Identifier = value;
                OnPropertyChanged("Identifier");
            }
        }

        public string Name
        {
            get { return string.IsNullOrEmpty(_Name) ? _Identifier : _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}