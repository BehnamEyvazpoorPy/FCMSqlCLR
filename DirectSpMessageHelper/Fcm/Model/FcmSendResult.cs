using MessageHelper.Fcm.Enum;
using Newtonsoft.Json;

namespace MessageHelper.Fcm.Model
{
    class FcmSendResult
    {
        public long MessageId { get; set; }

        public FcmSendStatus Status { get; set; }

        public string Error { get; set; }

        public string Address { get; set; }
    }
}
