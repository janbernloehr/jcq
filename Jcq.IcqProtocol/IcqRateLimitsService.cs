// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IcqRateLimitsService.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Jcq.Core.Collections;
using Jcq.Core.Contracts.Collections;
using Jcq.IcqProtocol.Contracts;
using Jcq.IcqProtocol.DataTypes;

namespace Jcq.IcqProtocol
{
    public class IcqRateLimitsService : ContextService, IRateLimitsService
    {
        private readonly Dictionary<Tuple<int, int>, long> _mappings;

        public IcqRateLimitsService(IContext context)
            : base(context)
        {
            var connector = context.GetService<IConnector>() as IcqConnector;

            if (connector == null)
                throw new InvalidCastException("Context Connector Service must be of Type IcqConnector");

            RateLimits = new KeyedNotifiyingCollection<long, IcqRateLimitsClass>(r => r.ClassId);
            _mappings = new Dictionary<Tuple<int, int>, long>();

            connector.RegisterSnacHandler(0x1, 0x7, new Action<Snac0107>(AnalyseSnac0107));
            connector.RegisterSnacHandler(0x1, 0xA, new Action<Snac010A>(AnalyseSnac010A));
        }

        public KeyedNotifiyingCollection<long, IcqRateLimitsClass> RateLimits { get; private set; }

        IReadOnlyNotifyingCollection<IRateLimitsClass> IRateLimitsService.RateLimits
        {
            get { return RateLimits; }
        }

        public event EventHandler RateLimitsReceived;

        private void AnalyseSnac010A(Snac010A snac)
        {
            foreach (var rateClass in snac.RateClasses)
            {
                var c = RateLimits[rateClass.ClassId];

                c.AlertLevel = rateClass.AlertLevel;
                c.ClassId = rateClass.ClassId;
                c.ClearLevel = rateClass.ClearLevel;
                c.CurrentLevel = rateClass.CurrentLevel;
                c.CurrentState = rateClass.CurrentState;
                c.DisconnectLevel = rateClass.DisconnectLevel;
                c.LastTime = rateClass.LastTime;
                c.LimitLevel = rateClass.LimitLevel;
                c.MaxLevel = rateClass.MaxLevel;
                c.WindowSize = rateClass.WindowSize;
            }
        }

        private void AnalyseSnac0107(Snac0107 snac)
        {
            foreach (var rateClass in snac.RateClasses)
            {
                IcqRateLimitsClass c;

                if (RateLimits.Contains(rateClass.ClassId))
                {
                    c = RateLimits[rateClass.ClassId];

                    c.AlertLevel = rateClass.AlertLevel;
                    c.ClassId = rateClass.ClassId;
                    c.ClearLevel = rateClass.ClearLevel;
                    c.CurrentLevel = rateClass.CurrentLevel;
                    c.CurrentState = rateClass.CurrentState;
                    c.DisconnectLevel = rateClass.DisconnectLevel;
                    c.LastTime = rateClass.LastTime;
                    c.LimitLevel = rateClass.LimitLevel;
                    c.MaxLevel = rateClass.MaxLevel;
                    c.WindowSize = rateClass.WindowSize;
                    c.DataLastUpdatedFromServer = Environment.TickCount;
                }
                else
                {
                    c = new IcqRateLimitsClass()
                    {
                        AlertLevel = rateClass.AlertLevel,
                        ClassId = rateClass.ClassId,
                        ClearLevel = rateClass.ClearLevel,
                        CurrentLevel = rateClass.CurrentLevel,
                        CurrentState = rateClass.CurrentState,
                        DisconnectLevel = rateClass.DisconnectLevel,
                        LastTime = rateClass.LastTime,
                        LimitLevel = rateClass.LimitLevel,
                        MaxLevel = rateClass.MaxLevel,
                        WindowSize = rateClass.WindowSize
                    };

                    RateLimits.Add(c);
                }
            }

            foreach (var rateGroup in snac.RateGroups)
            {
                var cls = RateLimits.Where(l => l.ClassId == rateGroup.GroupId).FirstOrDefault();


                foreach (var pair in rateGroup.ServiceFamilyIdSubTypeIdPairs)
                {
                    var key = new Tuple<int, int>(pair.FamilyId, pair.SubtypeId);

                    if (_mappings.ContainsKey(key))
                        continue;

                    _mappings.Add(key, rateGroup.GroupId);

                    if (cls != null)
                        cls.AddFamily(pair.FamilyId, pair.SubtypeId);
                }
            }

            if (RateLimitsReceived != null)
                RateLimitsReceived(this, EventArgs.Empty);
        }


        public int Calculate(int serviceTypeId, int subTypeId)
        {
            long classId;

            if (!_mappings.TryGetValue(new Tuple<int, int>(serviceTypeId, subTypeId), out classId)) return 0;

            if (!RateLimits.Contains(classId)) return 0;

            var cls = RateLimits[classId];

            long oldLevel;

            if (cls.LocalLastTime > cls.DataLastUpdatedFromServer)
            {
                oldLevel = cls.Computed == 0 ? cls.MaxLevel : cls.Computed;
            }
            else
            {
                oldLevel = cls.CurrentLevel;
            }

            long newLevel;
            var newTime = Environment.TickCount;

            if (cls.LocalLastTime == 0)
                newLevel = cls.MaxLevel;
            else
            {
                var a = (cls.WindowSize - 1) * oldLevel / (double)cls.WindowSize;
                var b = (newTime - cls.LocalLastTime) / (double)cls.WindowSize;

                newLevel = Convert.ToInt64(a + b);
            }

            cls.LocalLastTime = newTime;
            cls.Computed = newLevel;

            cls.WaitTime = Convert.ToInt32(Math.Max(cls.WindowSize * cls.ClearLevel - (cls.WindowSize - 1) * oldLevel, 0));

            return cls.WaitTime;
        }

        public void EmergencyThrottle()
        {
            foreach (var limit in RateLimits)
            {
                limit.Computed = limit.DisconnectLevel + 1;
                limit.CurrentLevel = limit.DisconnectLevel + 1;
                limit.DataLastUpdatedFromServer = Environment.TickCount;
            }
        }
    }

    public class IcqRateLimitsClass : IRateLimitsClass
    {
        private long _alertLevel;
        private long _classId;
        private long _clearLevel;
        private long _currentLevel;
        private byte _currentState;
        private long _disconnectLevel;
        private long _lastTime;
        private long _limitLevel;
        private long _maxLevel;
        private long _windowSize;
        private long _computed;
        private long _localLastTime;
        private long _dataLastUpdatedFromServer;
        private int _waitTime;

        private List<Tuple<int, int>> _families = new List<Tuple<int, int>>();

        public void AddFamily(int serviceId , int subId)
        {
            _families.Add(new Tuple<int,int>(serviceId, subId));
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


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
    }
}