using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace MessageHelper.Fcm.Model
{
    public class FcmResponse
    {
        [JsonProperty("multicast_id")]
        public string MulticastId { get; set; }

        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("failure")]
        public int Failure { get; set; }

        [JsonProperty("canonical_ids")]
        public int CanonicalIds { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

    }
}
