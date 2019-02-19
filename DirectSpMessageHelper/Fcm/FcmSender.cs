using MessageHelper.Fcm.Enum;
using MessageHelper.Fcm.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessageHelper.Fcm
{
    public class FcmSender
    {
        private const string _fcmServerUrl = "https://fcm.googleapis.com/fcm/send";
        private readonly string _serverKey;
        private readonly string _senderId;

        public FcmSender(string serverKey, string senderId)
        {
            _serverKey = serverKey;
            _senderId = senderId;
        }

        public async Task<FcmResponse> Send(FcmMessage message)
        {
            var httpClient = new HttpClient();

            // Authorization key is needed to Authenticating your request
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Key={_serverKey}");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("sender_id", _senderId);

            var dataJson = JsonConvert.SerializeObject(message,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            var response = await httpClient.PostAsync(_fcmServerUrl, new StringContent(dataJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FcmResponse>(responseBody);

        }

    }
}
