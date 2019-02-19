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
    public class FcmGroupHandler : IFcmGroupHandler
    {
        private readonly string _serverKey;
        private readonly string _senderId;
        private readonly string _fcmServerUrl = "https://fcm.googleapis.com/fcm/notification";

        public FcmGroupHandler(string serverKey, string senderId)
        {
            _serverKey = serverKey;
            _senderId = senderId;
        }

        /// <summary>
        /// Add a new device to the user devices group
        /// </summary>
        /// <param name="groupName"> Devices group name</param>
        /// <param name="tokens"> Device token</param>
        public async Task<string> AddTokenToGroup(string groupName, string[] tokens)
        {
            try
            {
                await AddToken(groupName, tokens);
                return null;
            }
            catch (WebException we)
            {
                if (we.Response != null)
                    using (StreamReader reader = new StreamReader(we.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        if (errorResponse.ToUpper().Trim() == "{\"ERROR\":\"NOTIFICATION_KEY NOT FOUND\"}")
                            return await CreateAndAddToken(groupName, tokens);
                        throw new Exception(errorResponse);
                    }
                throw we;
            }

        }

        /// <summary>
        /// Add token to an existing group either throw 400 bad request exception if the group does not exist
        /// </summary>
        private async Task AddToken(string groupName, string[] tokens)
        {
            // Get group key by group name
            string groupKey = await GetGroupKey(groupName);

            var httpClient = new HttpClient();

            // Authorization key is needed to Authenticating your request
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Key={_serverKey}");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("project_id", _senderId);

            var data = new
            {
                operation = "add",
                notification_key_name = groupName,
                notification_key = groupKey,
                registration_ids = tokens
            };
            var dataJson = JsonConvert.SerializeObject(data);
            var response = await httpClient.PostAsync(_fcmServerUrl, new StringContent(dataJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        // Create a new group and add token to it
        private async Task<string> CreateAndAddToken(string groupName, string[] tokens)
        {
            var httpClient = new HttpClient();

            // Authorization key is needed to Authenticating your request
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Key={_serverKey}");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("project_id", _senderId);

            var data = new
            {
                operation = "create",
                notification_key_name = groupName,
                registration_ids = tokens
            };
            var dataJson = JsonConvert.SerializeObject(data);
            var response = await httpClient.PostAsync(_fcmServerUrl, new StringContent(dataJson, Encoding.UTF8, "application/json"));
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FcmDeviceGroup>(responseBody).Key;
        }

        // Get an existence group key
        public async Task<string> GetGroupKey(string groupName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{_fcmServerUrl}?notification_key_name={groupName}");
            request.ContentType = "application/json";
            request.Proxy = null;
            request.Headers.Add($"Authorization: key={_serverKey}");
            request.Headers.Add($"project_id:{_senderId}");
            request.ContentLength = 0;
            request.Proxy = null;

            using (HttpWebResponse tResponse = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream dataStreamResponse = tResponse.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(dataStreamResponse))
                return JsonConvert.DeserializeObject<FcmDeviceGroup>(streamReader.ReadToEnd()).Key;
        }
    }
}
