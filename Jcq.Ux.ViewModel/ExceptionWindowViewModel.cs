// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionWindowViewModel.cs" company="Jan-Cornelius Molnar">
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
using System.ComponentModel;
using JCsTools.Core;
using JCsTools.Core.Interfaces;

namespace JCsTools.JCQ.ViewModel
{
    public class ExceptionWindowViewModel : INotifyPropertyChanged
    {
        private string _Message;

        public ExceptionWindowViewModel()
        {
            Kernel.Exceptions.ExceptionPublished += OnException;
        }

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;

                OnPropertyChanged("Message");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnException(object sender, ExceptionEventArgs e)
        {
            Message += string.Format("{0:d}: {1}\n\n", DateTime.Now,
                e.ExceptionInfo.Exception);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}