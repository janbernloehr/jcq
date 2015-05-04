// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIActionResultCode.cs" company="Jan-Cornelius Molnar">
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

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public enum SSIActionResultCode : ushort
    {
        Success = 0x0,
        ItemNotFoundInList = 0x2,
        ItemAlreadyExists = 0x3,
        ErrorAddingItem_InvalidId_OR_AllreadyInList_OR_InvalidData = 0xa,
        LimitForThisTypeOfItemsExceeded = 0xc,
        TryingToAddICQContactToAIMList = 0xd,
        CantAddContactBecauseItRequiresAuthorization = 0xe
    }
}