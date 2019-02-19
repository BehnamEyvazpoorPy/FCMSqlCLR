using Newtonsoft.Json;

namespace MessageHelper.Fcm.Enum
{
    public enum FcmPriority
    {
        [JsonProperty("normal")]
        Normal,
        [JsonProperty("high")]
        High
    }
}