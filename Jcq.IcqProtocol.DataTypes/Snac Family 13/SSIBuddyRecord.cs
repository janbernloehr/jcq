// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SSIBuddyRecord.cs" company="Jan-Cornelius Molnar">
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

using System.Collections.Generic;

namespace JCsTools.JCQ.IcqInterface.DataTypes
{
    public class SSIBuddyRecord : SSIRecord
    {
        private readonly TlvBuddyCommentField _Comment = new TlvBuddyCommentField();
        private readonly TlvLocalEmailAddress _LocalEmailAddress = new TlvLocalEmailAddress();
        private readonly TlvLocalScreenName _LocalScreenName = new TlvLocalScreenName();
        private readonly TlvLocalSmsNumber _LocalSmsNumber = new TlvLocalSmsNumber();
        private readonly TlvPersonalBuddyAlerts _PersonalAlerts = new TlvPersonalBuddyAlerts();

        public SSIBuddyRecord() : base(SSIItemType.BuddyRecord)
        {
        }

        public bool AwaitingAuthorization { get; set; }

        public TlvLocalScreenName LocalScreenName
        {
            get { return _LocalScreenName; }
        }

        public TlvLocalEmailAddress LocalEmailAddress
        {
            get { return _LocalEmailAddress; }
        }

        public TlvLocalSmsNumber LocalSmsNumber
        {
            get { return _LocalSmsNumber; }
        }

        public TlvBuddyCommentField Comment
        {
            get { return _Comment; }
        }

        public TlvPersonalBuddyAlerts PersonalAlerts
        {
            get { return _PersonalAlerts; }
        }

        public override int CalculateDataSize()
        {
            return 0;
        }

        public override void Deserialize(List<byte> data)
        {
            base.Deserialize(data);

            var index = SizeFixPart;

            while (index < data.Count)
            {
                var desc = TlvDescriptor.GetDescriptor(index, data);

                switch (desc.TypeId)
                {
                    case 0x66:
                        AwaitingAuthorization = true;
                        break;
                    case 0x131:
                        _LocalScreenName.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x137:
                        _LocalEmailAddress.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x13a:
                        _LocalSmsNumber.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x13c:
                        _Comment.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                    case 0x13d:
                        _PersonalAlerts.Deserialize(data.GetRange(index, desc.TotalSize));
                        break;
                }

                index += desc.TotalSize;
            }
        }
    }
}