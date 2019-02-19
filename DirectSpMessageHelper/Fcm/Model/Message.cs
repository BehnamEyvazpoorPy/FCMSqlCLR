using MessageHelper.Fcm.Enum;
using System;
using System.Collections.Generic;

namespace MessageHelper.Fcm.Model
{
    class Message
    {
        public long MessageId { get; set; }
        public string MessageBody { get; set; }
        public string CustomData { get; set; }
        public string Address { get; set; }
    }
}
