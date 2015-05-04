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
  public class ByteConverter
  {
    private static readonly System.DateTime _unixfsd = new System.DateTime(1970, 1, 1, 0, 0, 0);
    private static readonly System.Text.Encoding _icqenc = System.Text.Encoding.GetEncoding(28599);

    public static byte[] GetBytes(UInt16 value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return bytes;
    }

    public static byte[] GetBytes(UInt32 value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return bytes;
    }

    public static byte[] GetBytes(UInt64 value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return bytes;
    }

    public static byte[] GetBytesLE(UInt16 value)
    {
      return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytesLE(UInt32 value)
    {
      return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytesLE(UInt64 value)
    {
      return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(string value)
    {
      byte[] bytes = _icqenc.GetBytes(value);
      return bytes;
    }

    public static byte[] GetBytes(System.Guid value)
    {
      System.Byte[] bGuidNet = new System.Byte[15];
      System.Byte[] bGuidIcq = new System.Byte[15];

      bGuidNet = value.ToByteArray;

      bGuidIcq(0) = bGuidNet(3);
      bGuidIcq(1) = bGuidNet(2);
      bGuidIcq(2) = bGuidNet(1);
      bGuidIcq(3) = bGuidNet(0);

      bGuidIcq(4) = bGuidNet(5);
      bGuidIcq(5) = bGuidNet(4);

      bGuidIcq(6) = bGuidNet(7);
      bGuidIcq(7) = bGuidNet(6);

      bGuidIcq(8) = bGuidNet(8);
      bGuidIcq(9) = bGuidNet(9);

      bGuidIcq(6) = bGuidNet(7);
      bGuidIcq(7) = bGuidNet(6);

      bGuidIcq(10) = bGuidNet(10);
      bGuidIcq(11) = bGuidNet(11);
      bGuidIcq(12) = bGuidNet(12);
      bGuidIcq(13) = bGuidNet(13);
      bGuidIcq(14) = bGuidNet(14);
      bGuidIcq(15) = bGuidNet(15);

      return bGuidIcq;
    }

    //Public Shared Function GetBytes(ByVal value As Date) As Byte()
    //    Dim lngSeconds As UInt32

    //    If value < _unixfsd Then
    //        lngSeconds = 0
    //    Else
    //        lngSeconds = Convert.ToUInt32(value.Subtract(_unixfsd).TotalSeconds)
    //    End If

    //    Return GetBytes(lngSeconds)
    //End Function

    public static byte[] GetBytesForUInt32Date(System.DateTime value)
    {
      UInt32 lngSeconds;

      if (value < _unixfsd) {
        lngSeconds = 0;
      } else {
        lngSeconds = Convert.ToUInt32(value.Subtract(_unixfsd).TotalSeconds);
      }

      return GetBytes(lngSeconds);
    }

    public static byte[] GetBytesForUInt64Date(System.DateTime value)
    {
      UInt64 lngSeconds;

      if (value < _unixfsd) {
        lngSeconds = 0;
      } else {
        lngSeconds = Convert.ToUInt64(value.Subtract(_unixfsd).TotalSeconds);
      }

      return GetBytes(lngSeconds);
    }

    public static byte[] GetBytesForStringWithLeadingByteLength(string value)
    {
      List<byte> list;

      if (string.IsNullOrEmpty(value)) {
        return new byte[] { 0 };
      } else {
        list = new List<byte>();
        list.Add(Convert.ToByte(value.Length));
        list.AddRange(GetBytes(value));

        return list.ToArray;
      }
    }

    public static byte[] GetBytesForStringWithLeadingUInt16Length(string value)
    {
      List<byte> list;

      if (string.IsNullOrEmpty(value)) {
        return new byte[] { 0 };
      } else {
        list = new List<byte>();
        list.AddRange(ByteConverter.GetBytes(Convert.ToUInt16(value.Length)));
        list.AddRange(GetBytes(value));

        return list.ToArray;
      }
    }

    public static byte[] GetBytesForStringWithLeadingUInt32Length(string value)
    {
      List<byte> list;

      if (string.IsNullOrEmpty(value)) {
        return new byte[] { 0 };
      } else {
        list = new List<byte>();
        list.AddRange(ByteConverter.GetBytes(Convert.ToUInt32(value.Length)));
        list.AddRange(GetBytes(value));

        return list.ToArray;
      }
    }

    public static List<byte> GetXorHashFromPassword(char[] pwd)
    {
      List<System.Byte> plainText;
      System.Byte[] roast = new System.Byte[] {
        0xf3,
        0x26,
        0x81,
        0xc4,
        0x39,
        0x86,
        0xdb,
        0x92,
        0x71,
        0xa3,
        0xb9,
        0xe6,
        0x53,
        0x7a,
        0x95,
        0x7c
      };
      List<System.Byte> xoredPassword;
      int xorIndex;

      plainText = new List<System.Byte>(System.Text.Encoding.GetEncoding(28599).GetBytes(pwd));
      xoredPassword = new List<System.Byte>();

      for (int i = 0; i <= plainText.Count - 1; i++) {
        xoredPassword.Add(roast(xorIndex) ^ plainText(i));

        if (xorIndex == roast.Length - 1) {
          xorIndex = 0;
        } else {
          xorIndex += 1;
        }
      }

      return xoredPassword;
    }

    public static Guid ToGuid(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      byte[] bGuid = new byte[15];

      bGuid(3) = bytes(0);
      bGuid(2) = bytes(1);
      bGuid(1) = bytes(2);
      bGuid(0) = bytes(3);
      bGuid(5) = bytes(4);
      bGuid(4) = bytes(5);
      bGuid(7) = bytes(6);
      bGuid(6) = bytes(7);
      bGuid(8) = bytes(8);
      bGuid(9) = bytes(9);
      bGuid(10) = bytes(10);
      bGuid(11) = bytes(11);
      bGuid(12) = bytes(12);
      bGuid(13) = bytes(13);
      bGuid(14) = bytes(14);
      bGuid(15) = bytes(15);

      return new Guid(bGuid);
    }

    public static UInt16 ToUInt16(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      bytes.Reverse();
      return BitConverter.ToUInt16(bytes.ToArray, 0);
    }

    public static UInt32 ToUInt32(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      bytes.Reverse();
      return BitConverter.ToUInt32(bytes.ToArray, 0);
    }

    public static UInt64 ToUInt64(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      bytes.Reverse();
      return BitConverter.ToUInt64(bytes.ToArray, 0);
    }

    public static UInt16 ToUInt16LE(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      return BitConverter.ToUInt16(bytes.ToArray, 0);
    }

    public static UInt32 ToUInt32LE(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      return BitConverter.ToUInt32(bytes.ToArray, 0);
    }

    public static UInt64 ToUInt64LE(IEnumerable<byte> value)
    {
      List<byte> bytes = new List<byte>(value);
      return BitConverter.ToUInt64(bytes.ToArray, 0);
    }

    public static DateTime ToDateTimeFromUInt32FileStamp(System.Collections.Generic.List<byte> bytes)
    {
      long lngSeconds;

      lngSeconds = ToUInt32(bytes.GetRange(0, 4));

      return _unixfsd.AddSeconds(lngSeconds);
    }

    public static DateTime ToDateTimeFromUInt64FileStamp(System.Collections.Generic.List<byte> bytes)
    {
      decimal lngSeconds;

      lngSeconds = ToUInt64(bytes.GetRange(0, 8));

      return _unixfsd.AddSeconds(lngSeconds);
    }

    public static string ToHex(int value)
    {
      return string.Format("{0:X2}", value);
    }

    public static string ToStringFromByteIndex(int index, List<byte> data)
    {
      byte length;
      string text;

      length = data(index);

      //If data.Count < index + length Then Throw New NotEnoughBytesException("Text", data.Count, index + length)

      text = ToString(data.GetRange(index + 1, length));

      return text;
    }

    public static string ToStringFromUInt16Index(int index, System.Collections.Generic.List<byte> data)
    {
      int length;
      string text;

      length = ToUInt16(data.GetRange(index, 2));

      //If data.Count < index + length Then Throw New NotEnoughBytesException("Text", data.Count, index + length)

      text = ToString(data.GetRange(index + 2, length));

      return text;
    }

    public static string ToStringFromUInt16LEIndex(int index, System.Collections.Generic.List<byte> data)
    {
      int length;
      string text;

      length = ToUInt16LE(data.GetRange(index, 2));

      //If data.Count < index + length Then Throw New NotEnoughBytesException("Text", data.Count, index + length)

      text = ToString(data.GetRange(index + 2, length));

      return text;
    }

    public static string ToZeroBasedStringFromUInt16LEIndex(int index, System.Collections.Generic.List<byte> data)
    {
      int length;
      string text;

      length = ToUInt16LE(data.GetRange(index, 2));

      text = ToString(data.GetRange(index + 2, length - 1));

      return text;
    }

    public static string ToString(List<byte> bytes)
    {
      return _icqenc.GetString(bytes.ToArray);
    }
  }
}

