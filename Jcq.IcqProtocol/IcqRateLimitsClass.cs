using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Jcq.IcqProtocol.Contracts;

namespace Jcq.IcqProtocol
{
    public class IcqRateLimitsClass : IRateLimitsClass
    {
        private readonly List<Tuple<int, int>> _families = new List<Tuple<int, int>>();
        private long _alertLevel;
        private long _classId;
        private long _clearLevel;
        private long _computed;
        private long _currentLevel;
        private byte _currentState;
        private long _dataLastUpdatedFromServer;
        private long _disconnectLevel;
        private long _lastTime;
        private long _limitLevel;
        private long _localLastTime;
        private long _maxLevel;
        private int _waitTime;
        private long _windowSize;

        public long DataLastUpdatedFromServer
        {
            get { return _dataLastUpdatedFromServer; }
            set
            {
                _dataLastUpdatedFromServer = value;
                OnPropertyChanged();
            }
        }

        public int WaitTime
        {
            get { return _waitTime; }
            set
            {
                _waitTime = value;
                OnPropertyChanged();
            }
        }

        public string Families
        {
            get
            {
                return string.Join(" ", _families.Select(f => string.Format("{0:X},{1:X}", f.Item1, f.Item2)).ToArray());
            }
        }

        public long ClassId
        {
            get { return _classId; }
            set
            {
                _classId = value;
                OnPropertyChanged();
            }
        }

        public long WindowSize
        {
            get { return _windowSize; }
            set
            {
                _windowSize = value;
                OnPropertyChanged();
            }
        }

        public long ClearLevel
        {
            get { return _clearLevel; }
            set
            {
                _clearLevel = value;
                OnPropertyChanged();
            }
        }

        public long AlertLevel
        {
            get { return _alertLevel; }
            set
            {
                _alertLevel = value;
                OnPropertyChanged();
            }
        }

        public long LimitLevel
        {
            get { return _limitLevel; }
            set
            {
                _limitLevel = value;
                OnPropertyChanged();
            }
        }

        public long DisconnectLevel
        {
            get { return _disconnectLevel; }
            set
            {
                _disconnectLevel = value;
                OnPropertyChanged();
            }
        }

        public long CurrentLevel
        {
            get { return _currentLevel; }
            set
            {
                _currentLevel = value;
                OnPropertyChanged();
            }
        }

        public long MaxLevel
        {
            get { return _maxLevel; }
            set
            {
                _maxLevel = value;
                OnPropertyChanged();
            }
        }

        public long LastTime
        {
            get { return _lastTime; }
            set
            {
                _lastTime = value;
                OnPropertyChanged();
            }
        }

        public byte CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Computed
        {
            get { return _computed; }
            set
            {
                _computed = value;
                OnPropertyChanged();
            }
        }

        public long LocalLastTime
        {
            get { return _localLastTime; }
            set
            {
                _localLastTime = value;
                OnPropertyChanged();
            }
        }

        public void AddFamily(int serviceId, int subId)
        {
            _families.Add(new Tuple<int, int>(serviceId, subId));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}