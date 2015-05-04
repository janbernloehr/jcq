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
using System.Linq;
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public class Snac010A : Snac
  {
    public Snac010A() : base(0x1, 0xa)
    {
    }

    private MessageCode _MessageCode;
    public MessageCode MessageCode {
      get { return _MessageCode; }
      set { _MessageCode = value; }
    }

    private List<RateClass> _RateClasses = new List<RateClass>();
    public List<RateClass> RateClasses {
      get { return _RateClasses; }
    }

    public override void Deserialize(System.Collections.Generic.List<byte> data)
    {
      base.Deserialize(data);

      int index = Snac.SizeFixPart;
      TlvDescriptor desc;

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

      _MessageCode = (MessageCode)Convert.ToInt32(ByteConverter.ToUInt16(data.GetRange(index, 2)));

      index += 2;

      while (index < data.Count - 1) {
        RateClass rc = new RateClass();

        rc.Deserialize(data.GetRange(index, data.Count - index));

        index += rc.TotalSize;

        _RateClasses.Add(rc);
      }

      SetTotalSize(index);
    }

    public override System.Collections.Generic.List<byte> Serialize()
    {
      throw new NotImplementedException();
    }

    public override int CalculateDataSize()
    {
      return 2 + _RateClasses.Sum(r => r.TotalSize);
    }
  }


  public enum MessageCode
  {
    RateLimitsParametersChanged = 0x1,
    RateLimitsWarningCurrentLevelAlertLevel = 0x2,
    RateLimitHitCurrentLevelLimitLevel = 0x3,
    RateLimitClearCurrentLevelBecomeClearLevel = 0x4
  }
}

