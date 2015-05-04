//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public class EndOfOfflineMessagesResponse : MetaResponse
  {
    public EndOfOfflineMessagesResponse() : base(MetaResponseType.EndOfOfflineMessageResponse)
    {
    }

    public override int CalculateDataSize()
    {
      throw new NotImplementedException();
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = MetaResponse.SizeFixPart;

      _DroppedMessages = data(index);
    }

    private byte _DroppedMessages;
    public byte DroppedMessages {
      get { return _DroppedMessages; }
      set { _DroppedMessages = value; }
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }
  }
}

