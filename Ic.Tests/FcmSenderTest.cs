using DirectSpMessageHelper.Fcm;
using DirectSpMessageHelper.Fcm.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace DirectSpMessageHelper.Tests
{
    [TestClass]
    public class FcmSenderTest
    {
        Lazy<string> _serverKey = new Lazy<string>(() => ConfigurationManager.AppSettings["ServerKey"]);
        Lazy<string> _senderId = new Lazy<string>(() => ConfigurationManager.AppSettings["SenderId"]);
        Lazy<string> _deviceId = new Lazy<string>(() => ConfigurationManager.AppSettings["DeviceId"]);
        Lazy<string> _deviceId2 = new Lazy<string>(() => ConfigurationManager.AppSettings["DeviceId1"]);

        /// <summary>
        /// Testing the fcm send by FCM device token
        /// </summary>
        [TestMethod]
        public void SendFcmMessageByToken()
        {

            FcmMessage message = new FcmMessage
            {
                DryRun = true,
                RegistrationIds = new System.Collections.Generic.List<string>()
                {
                    _deviceId.Value,_deviceId2.Value
                },
                Notification = new FcmNotification
                {
                    Title = "test using c# unit test",
                    Body = "test"
                }
            };
            FcmSender sender = new FcmSender(_serverKey.Value, _senderId.Value);
            var response = sender.Send(message);
            Assert.AreNotEqual(response.Result.Results[0].Error, string.Empty);
            Assert.AreNotEqual(response.Result.Results[1].Error, string.Empty);
        }

        /// <summary>
        /// Testing the fcm send by FCM group key
        /// </summary>
        [TestMethod]
        public async void SendFcmMessageByGroupKey()
        {
            // Create a test group
            var groupName = $"Group-{Guid.NewGuid().ToString()}";
            var fcmGroupHandler = new FcmGroupHandler(_serverKey.Value, _senderId.Value);
            var groupKey =await fcmGroupHandler.AddTokenToGroup(groupName, new string[] { _deviceId.Value });

            // Send message by created group
            FcmMessage message = new FcmMessage
            {
                DryRun = true,
                To = groupKey,
                Notification = new FcmNotification
                {
                    Title = "test using c# unit test",
                    Body = "test"
                }
            };
            FcmSender sender = new FcmSender(_serverKey.Value, _senderId.Value);
            var response = sender.Send(message);
            Assert.AreEqual(response.Result.Results, null);

        }
    }
}
