// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snac010A.cs" company="Jan-Cornelius Molnar">
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
using System.Collections.Generic;
using System.Linq;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class Snac010A : Snac
    {
        private readonly List<RateClass> _RateClasses = new List<RateClass>();

        public Snac010A() : base(0x1, 0xa)
        {
        }

        public MessageCode MessageCode { get; set; }

        public List<RateClass> RateClasses
        {
            get { return _RateClasses; }
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;
            //TlvDescriptor desc;

            //index += 2

            //desc = TlvDescriptor.GetDescriptor(index, data)
            //index += desc.TotalSize

            //desc = TlvDescriptor.GetDescriptor(index, data)
            //index += desc.TotalSize

            //_MessageCode = DirectCast(Convert.ToInt32(ByteConverter.ToUInt16(data.GetRange(index, 2))), MessageCode)
            //index += 2
            //_RateClass.Deserialize(data.GetRange(index, data.Count - index))
            //index += _RateClass.TotalSize

            //Dim verbose As String = String.Format("{0}, {1}", MessageCode.ToString, _RateClass.ToString)

            //Core.Kernel.Logger.Log("ClientRate", TraceEventType.Verbose, verbose)


            // We do not know the precise structure of the first part...
            int length = ByteConverter.ToUInt16(data.GetRange(index, 2));

            index += length + 2;

            MessageCode = (MessageCode) Convert.ToInt32(ByteConverter.ToUInt16(data.GetRange(index, 2)));

            index += 2;

            while (index < data.Count - 1)
            {
                var rc = new RateClass();

                rc.Deserialize(data.GetRange(index, data.Count - index));

                index += rc.TotalSize;

                _RateClasses.Add(rc);
            }

            TotalSize = index;
        }

        public override List<byte> Serialize()
        {
            throw new NotImplementedException();
        }

        public override int CalculateDataSize()
        {
            return 2 + _RateClasses.Sum(r => r.TotalSize);
        }
    }
}