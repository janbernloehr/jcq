// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteConverter.cs" company="Jan-Cornelius Molnar">
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
using System.Text;

namespace Jcq.IcqProtocol.DataTypes
{
    public class ByteConverter
    {
        private static readonly DateTime UnixFileTime = new DateTime(1970, 1, 1, 0, 0, 0);
        private static readonly Encoding IcqEncoding = Encoding.GetEncoding(28599);
        //private static readonly Encoding _icqenc = Encoding.BigEndianUnicode;

        public static byte[] GetBytes(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytes(uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytes(ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesLE(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetBytesLE(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetBytesLE(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetBytes(string value)
        {
            var bytes = IcqEncoding.GetBytes(value);
            return bytes;
        }

        public static byte[] GetBytes(Guid value)
        {
            var bGuidIcq = new byte[16];

            var bGuidNet = value.ToByteArray();

            bGuidIcq[0] = bGuidNet[3];
            bGuidIcq[1] = bGuidNet[2];
            bGuidIcq[2] = bGuidNet[1];
            bGuidIcq[3] = bGuidNet[0];

            bGuidIcq[4] = bGuidNet[5];
            bGuidIcq[5] = bGuidNet[4];

            bGuidIcq[6] = bGuidNet[7];
            bGuidIcq[7] = bGuidNet[6];

            bGuidIcq[8] = bGuidNet[8];
            bGuidIcq[9] = bGuidNet[9];

            bGuidIcq[10] = bGuidNet[10];
            bGuidIcq[11] = bGuidNet[11];
            bGuidIcq[12] = bGuidNet[12];
            bGuidIcq[13] = bGuidNet[13];
            bGuidIcq[14] = bGuidNet[14];
            bGuidIcq[15] = bGuidNet[15];

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

        public static byte[] GetBytesForUInt32Date(DateTime value)
        {
            uint seconds = value < UnixFileTime ? 0 : Convert.ToUInt32(value.Subtract(UnixFileTime).TotalSeconds);

            return GetBytes(seconds);
        }

        public static byte[] GetBytesForUInt64Date(DateTime value)
        {
            ulong seconds = value < UnixFileTime ? 0 : Convert.ToUInt64(value.Subtract(UnixFileTime).TotalSeconds);

            return GetBytes(seconds);
        }

        public static byte[] GetBytesForStringWithLeadingByteLength(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new byte[] {0};
            }
            if (value.Length > byte.MaxValue)
                throw new InvalidOperationException("The string length exceeds the maximum length.");

            //TODO: We assume that the selected encoding encodes each char as exactly one byte.

            var list = new List<byte>(value.Length + 1) {Convert.ToByte(value.Length)};
            list.AddRange(GetBytes(value));

            return list.ToArray();
        }

        public static byte[] GetBytesForStringWithLeadingUInt16Length(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new byte[] {0};
            }
            if (value.Length > uint.MaxValue)
                throw new InvalidOperationException("The string length exceeds the maximum length.");

            //TODO: We assume that the selected encoding encodes each char as exactly one byte.

            var list = new List<byte>(value.Length + 2);
            list.AddRange(GetBytes(Convert.ToUInt16(value.Length)));
            list.AddRange(GetBytes(value));

            return list.ToArray();
        }

        public static byte[] GetBytesForStringWithLeadingUInt32Length(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new byte[] {0};
            }

            //TODO: We assume that the selected encoding encodes each char as exactly one byte.

            var list = new List<byte>(value.Length + 4);
            list.AddRange(GetBytes(Convert.ToUInt32(value.Length)));
            list.AddRange(GetBytes(value));

            return list.ToArray();
        }

        public static List<byte> GetXorHashFromPassword(char[] pwd)
        {
            byte[] roast =
            {
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
            int xorIndex = 0;

            var plainText = new List<byte>(Encoding.GetEncoding(28599).GetBytes(pwd));
            var xoredPassword = new List<byte>();

            for (int i = 0; i <= plainText.Count - 1; i++)
            {
                xoredPassword.Add(Convert.ToByte(roast[xorIndex] ^ plainText[i]));

                if (xorIndex == roast.Length - 1)
                {
                    xorIndex = 0;
                }
                else
                {
                    xorIndex += 1;
                }
            }

            return xoredPassword;
        }

        public static Guid ToGuid(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            var bGuid = new byte[16];

            bGuid[3] = bytes[0];
            bGuid[2] = bytes[1];
            bGuid[1] = bytes[2];
            bGuid[0] = bytes[3];
            bGuid[5] = bytes[4];
            bGuid[4] = bytes[5];
            bGuid[7] = bytes[6];
            bGuid[6] = bytes[7];
            bGuid[8] = bytes[8];
            bGuid[9] = bytes[9];
            bGuid[10] = bytes[10];
            bGuid[11] = bytes[11];
            bGuid[12] = bytes[12];
            bGuid[13] = bytes[13];
            bGuid[14] = bytes[14];
            bGuid[15] = bytes[15];

            return new Guid(bGuid);
        }

        public static ushort ToUInt16(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            bytes.Reverse();
            return BitConverter.ToUInt16(bytes.ToArray(), 0);
        }

        public static uint ToUInt32(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            bytes.Reverse();
            return BitConverter.ToUInt32(bytes.ToArray(), 0);
        }

        public static ulong ToUInt64(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            bytes.Reverse();
            return BitConverter.ToUInt64(bytes.ToArray(), 0);
        }

        public static ushort ToUInt16LE(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            return BitConverter.ToUInt16(bytes.ToArray(), 0);
        }

        public static uint ToUInt32LE(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            return BitConverter.ToUInt32(bytes.ToArray(), 0);
        }

        public static ulong ToUInt64LE(IEnumerable<byte> value)
        {
            var bytes = new List<byte>(value);
            return BitConverter.ToUInt64(bytes.ToArray(), 0);
        }

        public static DateTime ToDateTimeFromUInt32FileStamp(List<byte> bytes)
        {
            long seconds = ToUInt32(bytes.GetRange(0, 4));

            return UnixFileTime.AddSeconds(seconds);
        }

        public static DateTime ToDateTimeFromUInt32FileStamp(IEnumerable<byte> bytes)
        {
            return ToDateTimeFromUInt32FileStamp(bytes.ToList());
        }

        public static DateTime ToDateTimeFromUInt64FileStamp(List<byte> bytes)
        {
            double seconds = ToUInt64(bytes.GetRange(0, 8));

            return UnixFileTime.AddSeconds(seconds);
        }

        public static string ToHex(int value)
        {
            return string.Format("{0:X2}", value);
        }

        public static string ToStringFromByteIndex(int index, List<byte> data)
        {
            byte length = data[index];

            //If data.Count < index + length Then Throw New NotEnoughBytesException("Text", data.Count, index + length)

            string text = ToString(data.GetRange(index + 1, length));

            return text;
        }

        public static string ToStringFromUInt16Index(int index, List<byte> data)
        {
            int length = ToUInt16(data.GetRange(index, 2));

            //If data.Count < index + length Then Throw New NotEnoughBytesException("Text", data.Count, index + length)

            string text = ToString(data.GetRange(index + 2, length));

            return text;
        }

        public static string ToStringFromUInt16LEIndex(int index, List<byte> data)
        {
            int length = ToUInt16LE(data.GetRange(index, 2));

            //If data.Count < index + length Then Throw New NotEnoughBytesException("Text", data.Count, index + length)

            string text = ToString(data.GetRange(index + 2, length));

            return text;
        }

        public static string ToZeroBasedStringFromUInt16LEIndex(int index, List<byte> data)
        {
            int length = ToUInt16LE(data.GetRange(index, 2));

            string text = ToString(data.GetRange(index + 2, length - 1));

            return text;
        }

        public static string ToString(List<byte> bytes)
        {
            return IcqEncoding.GetString(bytes.ToArray());
            //return Encoding.BigEndianUnicode.GetString(bytes.ToArray());
        }
    }
}