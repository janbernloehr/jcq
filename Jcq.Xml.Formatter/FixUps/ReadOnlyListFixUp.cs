// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyListFixUp.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.Xml.Formatter
{
    /// <summary>
    ///     Represents a fixup for a ReadOnly Property which represents an object of type IList.
    /// </summary>
    public class ReadOnlyListPropertyFixUp : IFixUp
    {
        public ReadOnlyListPropertyFixUp(int temporaryListId, IList targetList)
        {
            TemporaryListId = temporaryListId;
            TargetList = targetList;
        }

        public int TemporaryListId { get; set; }
        public IList TargetList { get; private set; }

        public void Execute(ISerializer serializer)
        {
            var temporaryList = (IList) serializer.GetDeserializeObjectById(TemporaryListId);

            foreach (var item in temporaryList)
            {
                TargetList.Add(item);
            }
        }
    }
}