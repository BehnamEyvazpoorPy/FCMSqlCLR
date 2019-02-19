using Newtonsoft.Json;

namespace MessageHelper.Fcm.Model
{
    public class FcmDeviceGroup
    {
        [JsonProperty("notification_key")]
        public string Key { get; set; }
    }
}
