// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnacFamily01Test.cs" company="Jan-Cornelius Molnar">
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

using System.Linq;
using JCsTools.JCQ.IcqInterface.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    [TestClass]
    public class SnacFamily01Test
    {
        [TestMethod]
        public void Snac0101Test()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0xE1, 0xCA, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x99, 0xD0, 0x07, 0x62,
                0x00, 0x05
            };

            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize);
            Assert.AreEqual(data.Length - 6, f.DataSize);

            Assert.AreEqual(FlapChannel.SnacData, f.Channel);

            Assert.AreEqual(0xE1CA, f.DatagramSequenceNumber);

            Assert.IsNotNull(f.DataItems);
            Assert.AreEqual(1, f.DataItems.Count);

            // snac
            var item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof (Snac0101));

            var s = (Snac0101) item;

            Assert.AreEqual(2, s.DataSize);
            Assert.AreEqual(ErrorCode.RequestedServiceUnavailable, s.ErrorCode);
        }

        [TestMethod]
        public void Snac0102Test()
        {
            var data = new byte[]
            {
                0x2A, 0x02, 0x2, 0x3, 0x0, 0x5a, 0x0, 0x1, 0x0, 0x2, 0x0, 0x0, 0x0, 0x0, 0x0, 0x2,
                0x0, 0x1, 0x0, 0x3, 0x1, 0x10, 0x4, 0x7B, 0x0, 0x13, 0x0, 0x2, 0x1, 0x10, 0x4, 0x7B,
                0x0, 0x2, 0x0, 0x1, 0x1, 0x1, 0x4, 0x7B, 0x0, 0x3, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 
                0x0, 0x15, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 0x0, 0x4, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 
                0x0, 0x6, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 0x0, 0x9, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 
                0x0, 0xA, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B, 0x0, 0xB, 0x0, 0x1, 0x1, 0x10, 0x4, 0x7B
            };

            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize);
            Assert.AreEqual(data.Length - 6, f.DataSize);

            Assert.AreEqual(FlapChannel.SnacData, f.Channel);

            Assert.AreEqual(0x0203, f.DatagramSequenceNumber);

            Assert.IsNotNull(f.DataItems);
            Assert.AreEqual(1, f.DataItems.Count);

            // snac
            var item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof (Snac0102));

            var s = (Snac0102) item;

            Assert.AreEqual(80, s.DataSize);

            Assert.IsNotNull(s.Families);
            Assert.AreEqual(10, s.Families.Count);

            var family = s.Families[0];

            Assert.AreEqual(1, family.FamilyNumber);
            Assert.AreEqual(3, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[1];

            Assert.AreEqual(0x13, family.FamilyNumber);
            Assert.AreEqual(2, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[2];

            Assert.AreEqual(2, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0101, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[3];

            Assert.AreEqual(3, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[4];

            Assert.AreEqual(0x15, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[5];

            Assert.AreEqual(4, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[6];

            Assert.AreEqual(6, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[7];

            Assert.AreEqual(9, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[8];

            Assert.AreEqual(0xA, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

            family = s.Families[9];

            Assert.AreEqual(0xB, family.FamilyNumber);
            Assert.AreEqual(1, family.FamilyVersion);
            Assert.AreEqual(0x0110, family.ToolId);
            Assert.AreEqual(0x047b, family.ToolVersion);

        }

        [TestMethod]
        public void Snac0103Test()
        {
            var data = new byte[]
            {
               0x2A, 0x02, 0x3D, 0xFE, 0x00, 0x22, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00, 0x82, 0x95, 0xE5, 0x7A, 
               0x00, 0x01, 0x00, 0x02, 0x00, 0x03, 0x00, 0x04, 0x00, 0x06, 0x00, 0x08, 0x00, 0x09, 0x00, 0x0A, 
               0x00, 0x0B, 0x00, 0x0C, 0x00, 0x13, 0x00, 0x15                         
            };

            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize);
            Assert.AreEqual(data.Length - 6, f.DataSize);

            Assert.AreEqual(FlapChannel.SnacData, f.Channel);

            Assert.AreEqual(0x3DFE, f.DatagramSequenceNumber);

            Assert.IsNotNull(f.DataItems);
            Assert.AreEqual(1, f.DataItems.Count);

            // snac
            var item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof(Snac0103));

            var s = (Snac0103)item;

            Assert.AreEqual(24, s.DataSize);

            Assert.IsNotNull(s.ServerSupportedFamilyIds);
            Assert.AreEqual(12, s.ServerSupportedFamilyIds.Count);

            Assert.AreEqual(1, s.ServerSupportedFamilyIds[0]);
            Assert.AreEqual(2, s.ServerSupportedFamilyIds[1]);
            Assert.AreEqual(3, s.ServerSupportedFamilyIds[2]);
            Assert.AreEqual(4, s.ServerSupportedFamilyIds[3]);
            Assert.AreEqual(6, s.ServerSupportedFamilyIds[4]);
            Assert.AreEqual(8, s.ServerSupportedFamilyIds[5]);
            Assert.AreEqual(9, s.ServerSupportedFamilyIds[6]);
            Assert.AreEqual(0xA, s.ServerSupportedFamilyIds[7]);
            Assert.AreEqual(0xB, s.ServerSupportedFamilyIds[8]);
            Assert.AreEqual(0xC, s.ServerSupportedFamilyIds[9]);
            Assert.AreEqual(0x13, s.ServerSupportedFamilyIds[10]);
            Assert.AreEqual(0x15, s.ServerSupportedFamilyIds[11]);

        }

        [TestMethod]
        public void Snac0106Test()
        {
            var data = new byte[]
            {0x2A, 0x02, 0x22, 0x96, 0x00, 0x0A, 0x00, 0x01, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06};

            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize);
            Assert.AreEqual(data.Length - 6, f.DataSize);

            Assert.AreEqual(FlapChannel.SnacData, f.Channel);

            Assert.AreEqual(0x2296, f.DatagramSequenceNumber);

            Assert.IsNotNull(f.DataItems);
            Assert.AreEqual(1, f.DataItems.Count);

            // snac
            var item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof (Snac0106));

            var s = (Snac0106) item;

            Assert.AreEqual(0, s.DataSize);
        }
    }
}