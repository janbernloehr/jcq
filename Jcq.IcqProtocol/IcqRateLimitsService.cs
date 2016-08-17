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
using System.Linq;
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

        public KeyedNotifiyingCollection<long, IcqRateLimitsClass> RateLimits { get; }

        IReadOnlyNotifyingCollection<IRateLimitsClass> IRateLimitsService.RateLimits
        {
            get { return RateLimits; }
        }

        public event EventHandler RateLimitsReceived;


        public int Calculate(int serviceTypeId, int subTypeId)
        {
            long classId;

            if (!_mappings.TryGetValue(new Tuple<int, int>(serviceTypeId, subTypeId), out classId)) return 0;

            if (!RateLimits.Contains(classId)) return 0;

            IcqRateLimitsClass cls = RateLimits[classId];

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
            int newTime = Environment.TickCount;

            if (cls.LocalLastTime == 0)
                newLevel = cls.MaxLevel;
            else
            {
                double a = (cls.WindowSize - 1)*oldLevel/(double) cls.WindowSize;
                double b = (newTime - cls.LocalLastTime)/(double) cls.WindowSize;

                newLevel = Convert.ToInt64(a + b);
            }

            cls.LocalLastTime = newTime;
            cls.Computed = newLevel;

            cls.WaitTime = Convert.ToInt32(Math.Max(cls.WindowSize*cls.ClearLevel - (cls.WindowSize - 1)*oldLevel, 0));

            return cls.WaitTime;
        }

        public void EmergencyThrottle()
        {
            foreach (IcqRateLimitsClass limit in RateLimits)
            {
                limit.Computed = limit.DisconnectLevel + 1;
                limit.CurrentLevel = limit.DisconnectLevel + 1;
                limit.DataLastUpdatedFromServer = Environment.TickCount;
            }
        }

        private void AnalyseSnac010A(Snac010A snac)
        {
            foreach (RateClass rateClass in snac.RateClasses)
            {
                IcqRateLimitsClass c = RateLimits[rateClass.ClassId];

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
            foreach (RateClass rateClass in snac.RateClasses)
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
                    c = new IcqRateLimitsClass
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

            foreach (RateGroup rateGroup in snac.RateGroups)
            {
                IcqRateLimitsClass cls = RateLimits.FirstOrDefault(l => l.ClassId == rateGroup.GroupId);


                foreach (FamilySubtypePair pair in rateGroup.ServiceFamilyIdSubTypeIdPairs)
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
    }
}